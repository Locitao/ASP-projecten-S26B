using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Mediasharing
{
    public sealed class Database
    {
        #region Fields

        private string username;
        private string password;
        private string host;
        private OracleConnection connect = new OracleConnection();

        // Singleton
        private static readonly Lazy<Database> instance =
            new Lazy<Database>(() => new Database());

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize the connection with the database
        /// </summary>
        private Database()
        {
            Initialize();
        }

        #endregion

        #region Properties

        public static Database Instance { get { return instance.Value; } }

        #endregion

        #region Methods - Connection

        private void Initialize()
        {
            username = "ICT4EVENTS";
            password = "ICT4EVENTS";
            host = "localhost/xe";

            string connectionstring = string.Format("Data Source= {0};User ID={1};Password={2};", host, username,
                password);

            try
            {
                connect = new OracleConnection(connectionstring);
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.Write("Couldn't create connectionstring!");
                System.Diagnostics.Debug.Write(ex);
                throw new Exception("There was an error to make a new connectionstring!\nCode: " + ex.ErrorCode +
                                    "\nMessage: " + ex.Message);
            }

        }

        /// <summary>
        /// Try to open the connection with the database
        /// </summary>
        /// <returns>Returns a bool, true = connection success, false = connection failed</returns>
        public void OpenConnection()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Opening Database Connection");
                connect.Open();
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                if (ex.Message == "Connection is already open")
                {
                    return;
                }
                throw;
            }
        }

        /// <summary>
        /// Try to close the connection with the database
        /// </summary>
        /// <returns>Return a bool, true = connection close success, false = connection close failed</returns>
        public bool CloseConnection()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Closing Database Connection");
                connect.Close();
                System.Diagnostics.Debug.WriteLine("---------------");
                return true;
            }
            catch (OracleException ex)
            {
                string error = "ERROR DETECTED!\nCode: " + ex.ErrorCode + "\nMessage: " + ex.Message;
                System.Diagnostics.Debug.WriteLine(error);
                return false;
            }
        }
        #endregion

        #region Selects with DataSet
        public DataSet GetData(string query)
        {
            try
            {
                OpenConnection();
                OracleDataAdapter o_adapter = new OracleDataAdapter(query, connect);
                DataSet dataSet = new DataSet();
                o_adapter.Fill(dataSet);
                return dataSet;
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("---------- ERROR WHILE EXECUTING QUERY ----------");
                System.Diagnostics.Debug.WriteLine("Error code: {0}", ex.ErrorCode);
                System.Diagnostics.Debug.WriteLine("Error message: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("---------- END OF EXCEPTION ----------");
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
        #endregion

        #region Selects with Dictionary List
        public List<Dictionary<string, object>> GetMessages()
        {
            OracleCommand cmd = new OracleCommand("SELECT bij.\"ID\" AS ID, acc.\"gebruikersnaam\" AS GEBRUIKERSNAAM, ber.\"titel\" AS TITEL, ber.\"inhoud\" AS INHOUD " +
                                                  "FROM BIJDRAGE bij, BERICHT ber, ACCOUNT acc " +
                                                  "WHERE bij.\"account_id\" = acc.\"ID\" AND ber.\"bijdrage_id\" = bij.\"ID\" " +
                                                  "AND bij.\"ID\" NOT IN " +
                                                                            "(SELECT bb.\"bericht_id\" " +
                                                                            "FROM BIJDRAGE_BERICHT bb, BIJDRAGE bij " +
                                                                            "WHERE bb.\"bijdrage_id\" = bij.\"ID\" " +
                                                                            "AND bij.\"soort\" = 'bericht')");
            return ExecuteQuery(cmd);
        }

        public List<Dictionary<string, object>> GetReactions(int messageId)
        {
            OracleCommand cmd = new OracleCommand("SELECT bij.\"ID\" AS ID, acc.\"gebruikersnaam\" AS GEBRUIKERSNAAM, ber.\"titel\" AS TITEL, ber.\"inhoud\" AS INHOUD " +
                                                  "FROM BIJDRAGE bij, BERICHT ber, ACCOUNT acc WHERE bij.\"account_id\" = acc.\"ID\" " +
                                                  "AND ber.\"bijdrage_id\" = bij.\"ID\" " +
                                                  "AND bij.\"ID\" IN " +
                                                                        "(SELECT bb.\"bericht_id\" " +
                                                                        "FROM BIJDRAGE_BERICHT " +
                                                                        "WHERE \"bericht_id\" = :messageId)");
            cmd.Parameters.Add("messageId", messageId);

            return ExecuteQuery(cmd);
        }
        #endregion

        #region Execute and NonExecute
        private bool Execute(OracleCommand cmd)
        {
            System.Diagnostics.Debug.WriteLine("---------------");
            System.Diagnostics.Debug.WriteLine("Attempting to execute query: " + cmd.CommandText);
            try
            {
                OpenConnection();
                cmd.Connection = connect;
                cmd.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine("COMPLETE");
                return true;
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("---------- ERROR WHILE EXECUTING QUERY ----------");
                System.Diagnostics.Debug.WriteLine("Error while executing query: " + cmd.CommandText);
                System.Diagnostics.Debug.WriteLine("Error code: {0}", ex.ErrorCode);
                System.Diagnostics.Debug.WriteLine("Error message: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("---------- END OF EXCEPTION ----------");
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        private List<Dictionary<string, object>> ExecuteQuery(OracleCommand cmd)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            System.Diagnostics.Debug.WriteLine("---------------");
            System.Diagnostics.Debug.WriteLine("Attempting to execute query: " + cmd.CommandText);
            try
            {
                OpenConnection();

                cmd.Connection = connect;

                using (OracleDataReader resultReader = cmd.ExecuteReader())
                {
                    //loop through the rows and add them to the result
                    while (resultReader.Read())
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();

                        //loop through the fields and add them to the row
                        for (int fieldId = 0; fieldId < resultReader.FieldCount; fieldId++)
                            row.Add(resultReader.GetName(fieldId), resultReader.GetValue(fieldId));

                        result.Add(row);
                    }
                }
                System.Diagnostics.Debug.WriteLine("COMPLETE");
                return result;
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("---------- ERROR WHILE EXECUTING QUERY ----------");
                System.Diagnostics.Debug.WriteLine("Error while executing query: {0}", cmd.CommandText);
                System.Diagnostics.Debug.WriteLine("Error code: {0}", ex.ErrorCode);
                System.Diagnostics.Debug.WriteLine("Error message: {0}", ex.Message);
                System.Diagnostics.Debug.WriteLine("---------- END OF EXCEPTION ----------");
            }
            finally
            {
                CloseConnection();
            }
            return null;
        }


        #endregion
    }
}