﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Mediasharing
{
    public sealed class Database
    {
        #region Fields

        private string _username;
        private string _password;
        private string _host;
        private OracleConnection _connect = new OracleConnection();

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
            _username = "ICT4EVENTS";
            _password = "ICT4EVENTS";
            _host = "localhost/xe";

            string connectionstring = string.Format("Data Source= {0};User ID={1};Password={2};", _host, _username,
                _password);

            try
            {
                _connect = new OracleConnection(connectionstring);
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
                _connect.Open();
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
                _connect.Close();
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
                OracleDataAdapter o_adapter = new OracleDataAdapter(query, _connect);
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
                                                                            "AND (bij.\"soort\" = 'bericht' " +
                                                                            "OR bij.\"soort\" = 'bestand'))");
            return ExecuteQuery(cmd);
        }

        public List<Dictionary<string, object>> GetReactions(int messageId)
        {
            OracleCommand cmd = new OracleCommand("SELECT bij.\"ID\" AS ID, acc.\"gebruikersnaam\" AS GEBRUIKERSNAAM, ber.\"titel\" AS TITEL, ber.\"inhoud\" AS INHOUD " +
                                                  "FROM BIJDRAGE bij, BERICHT ber, ACCOUNT acc WHERE bij.\"account_id\" = acc.\"ID\" " +
                                                  "AND ber.\"bijdrage_id\" = bij.\"ID\" " +
                                                  "AND bij.\"ID\" IN " +
                                                                        "(SELECT \"bericht_id\" " +
                                                                        "FROM BIJDRAGE_BERICHT " +
                                                                        "WHERE \"bijdrage_id\" = :messageId)");
            cmd.Parameters.Add("messageId", messageId);

            return ExecuteQuery(cmd);
        }

        public List<Dictionary<string, object>> GetLikes (int id)
        {
            OracleCommand cmd = new OracleCommand("SELECT COUNT(*) AS LIKES " +
                                                  "FROM ACCOUNT_BIJDRAGE ab, BIJDRAGE bij " +
                                                  "WHERE bij.\"ID\" = ab.\"bijdrage_id\" " +
                                                  "AND ab.\"like\" = 1 " +
                                                  "AND bij.\"ID\" = :id");
            cmd.Parameters.Add("id", id);

            return ExecuteQuery(cmd);
        }

        public List<Dictionary<string, object>> GetLikedByUser(int id, int userId)
        {
            OracleCommand cmd =
                new OracleCommand("SELECT COUNT(*) AS LIKED " +
                                  "FROM ACCOUNT_BIJDRAGE ab, BIJDRAGE bij, ACCOUNT acc " +
                                  "WHERE bij.\"ID\" = ab.\"bijdrage_id\" " +
                                  "AND acc.\"ID\" = ab.\"account_id\" " +
                                  "AND ab.\"like\" = 1 " +
                                  "AND bij.\"ID\" = :id " +
                                  "AND ab.\"account_id\" = :userId");

            cmd.Parameters.Add("id", id);
            cmd.Parameters.Add("userId", userId);

            return ExecuteQuery(cmd);
        }

        public List<Dictionary<string, object>> GetReportedByUser(int id, int userId)
        {
            OracleCommand cmd =
                new OracleCommand("SELECT COUNT(*) AS REPORTED " +
                                  "FROM ACCOUNT_BIJDRAGE ab, BIJDRAGE bij, ACCOUNT acc " +
                                  "WHERE bij.\"ID\" = ab.\"bijdrage_id\" " +
                                  "AND acc.\"ID\" = ab.\"account_id\" " +
                                  "AND ab.\"ongewenst\" = 1 " +
                                  "AND bij.\"ID\" = :id " +
                                  "AND ab.\"account_id\" = :userId");

            cmd.Parameters.Add("id", id);
            cmd.Parameters.Add("userId", userId);

            return ExecuteQuery(cmd);
        }

        public List<Dictionary<string, object>> GetItem(int itemId)
        {
            OracleCommand cmd = new OracleCommand("SELECT \"bestandslocatie\" AS BESTANDSLOCATIE " +
                                                  "FROM BESTAND " +
                                                  "WHERE \"bijdrage_id\" = :itemId");

            cmd.Parameters.Add("itemId", itemId);

            return ExecuteQuery(cmd);
        }

        public List<Dictionary<string, object>> GetItemMessages(int itemMessageId)
        {
            OracleCommand cmd = new OracleCommand("SELECT bij.\"ID\" AS ID, acc.\"gebruikersnaam\" AS GEBRUIKERSNAAM, ber.\"titel\" AS TITEL, ber.\"inhoud\" AS INHOUD " +
                                                  "FROM BIJDRAGE bij, BERICHT ber, ACCOUNT acc " +
                                                  "WHERE bij.\"account_id\" = acc.\"ID\" " +
                                                  "AND ber.\"bijdrage_id\" = bij.\"ID\" " +
                                                  "AND bij.\"ID\" IN " +
                                                                        "(SELECT \"bericht_id\" " +
                                                                        "FROM BIJDRAGE_BERICHT " +
                                                                        "WHERE \"bijdrage_id\" = :itemMessageId)");
            cmd.Parameters.Add("itemMessageId", itemMessageId);

            return ExecuteQuery(cmd);
        }

        public int GetMaxBijdrageId()
        {
            OracleCommand cmd = new OracleCommand("SELECT MAX(\"ID\") AS MAXID FROM BIJDRAGE");
            List<Dictionary<string, object>> output = ExecuteQuery(cmd);
            int maxId = Convert.ToInt32(output[0]["MAXID"]);
            return maxId;
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
                cmd.Connection = _connect;
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

        #region Inserts
        public bool InsertLike(int id, int userId)
        {
            try
            {
                OracleCommand cmd =
                    new OracleCommand("INSERT INTO ACCOUNT_BIJDRAGE" +
                                      "(\"ID\", \"account_id\", \"bijdrage_id\", \"like\", \"ongewenst\") VALUES " +
                                      "(NULL, :userId, :id, 1, 0)");

                cmd.Parameters.Add("userId", userId);
                cmd.Parameters.Add("id", id);

                Execute(cmd);
                return true;
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("---------- ERROR WHILE EXECUTING QUERY ----------");
                System.Diagnostics.Debug.WriteLine("Error while executing query");
                System.Diagnostics.Debug.WriteLine("Error code: {0}", ex.ErrorCode);
                System.Diagnostics.Debug.WriteLine("Error message: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("---------- END OF EXCEPTION ----------");
                return false;
            }
 
        }

        public bool InsertReport(int id, int userId)
        {
            try
            {
                OracleCommand cmd =
                    new OracleCommand("INSERT INTO ACCOUNT_BIJDRAGE" +
                                      "(\"ID\", \"account_id\", \"bijdrage_id\", \"like\", \"ongewenst\") VALUES " +
                                      "(NULL, :userId, :id, 0, 1)");

                cmd.Parameters.Add("userId", userId);
                cmd.Parameters.Add("id", id);

                Execute(cmd);
                return true;
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("---------- ERROR WHILE EXECUTING QUERY ----------");
                System.Diagnostics.Debug.WriteLine("Error while executing query");
                System.Diagnostics.Debug.WriteLine("Error code: {0}", ex.ErrorCode);
                System.Diagnostics.Debug.WriteLine("Error message: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("---------- END OF EXCEPTION ----------");
                return false;
            }
 
        }

        public bool InsertMessageCategory(string title , string content , int categoryId , int userId)
        {
            try
            {
                //Create a bijdrage, needed to create a bericht.
                OracleCommand cmd =
                    new OracleCommand("INSERT INTO BIJDRAGE" +
                                      "(\"ID\", \"account_id\", \"datum\", \"soort\") VALUES " +
                                      "(NULL, :userId, sysdate, 'bericht')");

                cmd.Parameters.Add("userId", userId);
                Execute(cmd);

                //We need the id we just created, let's get it from the database.
                int messageId = GetMaxBijdrageId();

                //Create a bericht with the bijdrage id from the above query.
                OracleCommand cmdTwo =
                    new OracleCommand("INSERT INTO BERICHT" +
                                      "(\"bijdrage_id\", \"titel\", \"inhoud\") VALUES " +
                                      "(:messageId, :title, :content)");

                cmdTwo.Parameters.Add("messageId", messageId);
                cmdTwo.Parameters.Add("title", title);
                cmdTwo.Parameters.Add("content", content);
                Execute(cmdTwo);

                return true;
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("---------- ERROR WHILE EXECUTING QUERY ----------");
                System.Diagnostics.Debug.WriteLine("Error while executing query");
                System.Diagnostics.Debug.WriteLine("Error code: {0}", ex.ErrorCode);
                System.Diagnostics.Debug.WriteLine("Error message: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("---------- END OF EXCEPTION ----------");
                return false;
            }
        }

        public bool InsertItem(int userId, int categoryId, string title, string content, string fileLocation, int size)
        {
            try
            {
                //Create a bijdrage, needed to create a bestand.
                OracleCommand cmd =
                    new OracleCommand("INSERT INTO BIJDRAGE" +
                                      "(\"ID\", \"account_id\", \"datum\", \"soort\") VALUES " +
                                      "(NULL, :userId, sysdate, 'bestand')");

                cmd.Parameters.Add("userId", userId);
                Execute(cmd);

                //We need the id we just created, let's get it from the database.
                int fileId = GetMaxBijdrageId();

                //Create a bericht with the bijdrage id from the above query.
                OracleCommand cmdTwo =
                    new OracleCommand("INSERT INTO BESTAND" +
                                      "(\"bijdrage_id\", \"categorie_id\", \"bestandslocatie\", \"grootte\") VALUES " +
                                      "(:fileId, :categoryId, :fileLocation, :size)");

                cmdTwo.Parameters.Add("fileId", fileId);
                cmdTwo.Parameters.Add("categoryId", categoryId);
                cmdTwo.Parameters.Add("fileLocation", fileLocation);
                cmdTwo.Parameters.Add("size", size);
                Execute(cmdTwo);

                //Let's create the message if the user created one.
                OracleCommand cmdThree =
                    new OracleCommand("INSERT INTO BIJDRAGE" +
                                      "(\"ID\", \"account_id\", \"datum\", \"soort\") VALUES " +
                                      "(NULL, :userId, sysdate, 'bericht')");

                 //We need the id we just created, let's get it from the database.
                int messageId = GetMaxBijdrageId();

                //Create a bericht with the bijdrage id from the above query.
                OracleCommand cmdFour =
                    new OracleCommand("INSERT INTO BERICHT" +
                                      "(\"bijdrage_id\", \"titel\", \"inhoud\") VALUES " +
                                      "(:messageId, :title, :content)");

                cmdTwo.Parameters.Add("messageId", messageId);
                cmdTwo.Parameters.Add("title", title);
                cmdTwo.Parameters.Add("content", content);
                Execute(cmdTwo);

                //Finally, we link the message(bericht) and file(bestand) togheter in the BIJDRAGE_BESTAND table.
                OracleCommand cmdFive =
                    new OracleCommand("INSERT INTO BIJDRAGE_BESTAND" +
                                      "(\"bijdrage_id\", \"bericht_id\") VALUES " +
                                      "(:fileId, :messageId)");

                cmdTwo.Parameters.Add("fileId", fileId);
                cmdTwo.Parameters.Add("messageId", messageId);

                return true;
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("---------- ERROR WHILE EXECUTING QUERY ----------");
                System.Diagnostics.Debug.WriteLine("Error while executing query");
                System.Diagnostics.Debug.WriteLine("Error code: {0}", ex.ErrorCode);
                System.Diagnostics.Debug.WriteLine("Error message: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("---------- END OF EXCEPTION ----------");
                return false;
            }
        }
        #endregion

        #region Deletes
        public bool DeleteLike(int id, int userId)
        {
            try
            {
                OracleCommand cmd =
                new OracleCommand("DELETE FROM ACCOUNT_BIJDRAGE WHERE \"account_id\" = :userId AND \"bijdrage_id\" = :id AND \"like\" = 1");

                cmd.Parameters.Add("userId", userId);
                cmd.Parameters.Add("id", id);

                Execute(cmd);
                return true;
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("---------- ERROR WHILE EXECUTING QUERY ----------");
                System.Diagnostics.Debug.WriteLine("Error while executing query");
                System.Diagnostics.Debug.WriteLine("Error code: " + ex.ErrorCode);
                System.Diagnostics.Debug.WriteLine("Error message: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("---------- END OF EXCEPTION ----------");
                return false;
            }
        }
        #endregion

        private List<Dictionary<string, object>> ExecuteQuery(OracleCommand cmd)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            System.Diagnostics.Debug.WriteLine("---------------");
            System.Diagnostics.Debug.WriteLine("Attempting to execute query: " + cmd.CommandText);
            try
            {
                OpenConnection();

                cmd.Connection = _connect;

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