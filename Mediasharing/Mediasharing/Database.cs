using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;

namespace Mediasharing
{
    /// <summary>
    /// This class contains all the methods to get and insert data from and to the database.
    /// </summary>
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

        /// <summary>
        /// Initializes the connection.
        /// </summary>
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

        /// <summary>
        /// Gets a DataSet.
        /// </summary>
        /// <param name="query"> the input qeury used to fill the dataset with </param>
        /// <returns></returns>
        public DataSet GetData(string query)
        {
            try
            {
                OpenConnection();
                OracleDataAdapter oAdapter = new OracleDataAdapter(query, _connect);
                DataSet dataSet = new DataSet();
                oAdapter.Fill(dataSet);
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

        /// <summary>
        /// Gets all the messages that aren't are reply.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get's all the reactions to the message, found by messageId.
        /// </summary>
        /// <param name="messageId">the Id of the selected message</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets all likes corresponding to the selected id, this id is of a "bijdrage",
        /// so can be either a "bestand, "categorie", or "bericht".
        /// a catego
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if the user liked the "bijdrage" with selected id.
        /// Returns the count, if the count is bigger than 0 the item is liked.
        /// </summary>
        /// <param name="id"> id of the "bijdrage" </param>
        /// <param name="userId"> id of the user </param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if the user reported the "bijdrage" with selected id.
        /// Returns the count, if the count is bigger than 0 the item is liked.
        /// </summary>
        /// <param name="id"> id of the "bijdrage" </param>
        /// <param name="userId"> id of the user </param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the items "bestandlocatie" with the selected "bijdrage" id.
        /// </summary>
        /// <param name="itemId"> id of the "bijdrage" </param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetItem(int itemId)
        {
            OracleCommand cmd = new OracleCommand("SELECT \"bestandslocatie\" AS BESTANDSLOCATIE " +
                                                  "FROM BESTAND " +
                                                  "WHERE \"bijdrage_id\" = :itemId");

            cmd.Parameters.Add("itemId", itemId);

            return ExecuteQuery(cmd);
        }

        /// <summary>
        /// Get the messages of the selected "bijdrage", using the selected "bijdrage" id.
        /// </summary>
        /// <param name="itemMessageId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the max "bijdrage" id from the database,
        /// needed to insert either a "categorie", "bericht", or "bestand".
        /// </summary>
        /// <returns></returns>
        public int GetMaxBijdrageId()
        {
            OracleCommand cmd = new OracleCommand("SELECT MAX(\"ID\") AS MAXID " +
                                                  "FROM BIJDRAGE");

            List<Dictionary<string, object>> output = ExecuteQuery(cmd);
            int maxId = Convert.ToInt32(output[0]["MAXID"]);
            return maxId;
        }

        /// <summary>
        /// Gets the max "bericht" id from the database,
        /// </summary>
        /// <returns></returns>
        public int GetMaxBerichtId()
        {
            OracleCommand cmd = new OracleCommand("SELECT MAX(\"bijdrage_id\") AS MAXID " +
                                                  "FROM BERICHT");

            List<Dictionary<string, object>> output = ExecuteQuery(cmd);
            int maxId = Convert.ToInt32(output[0]["MAXID"]);
            return maxId;
        }

        /// <summary>
        /// Gets the category id of a "bijdrage" using the selected "bijdrage" id.
        /// </summary>
        /// <param name="id"> id of the "bijdrage"</param>
        /// <returns></returns>
        public int GetCategoryIdWithItemId(int id)
        {
            OracleCommand cmd = new OracleCommand("SELECT \"categorie_id\" AS ID " +
                                                  "FROM BESTAND WHERE \"bijdrage_id\" = :id");

            cmd.Parameters.Add("id", id);

            List<Dictionary<string, object>> output = ExecuteQuery(cmd);
            int categoryId = Convert.ToInt32(output[0]["ID"]);
            return categoryId;
        }

        #endregion

        #region Inserts

        /// <summary>
        /// Inserts a like into the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Inserts a report into the databse.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Insert a message linked to selected category.
        /// </summary>
        /// <param name="title"> title of the message </param>
        /// <param name="content"> content of the message </param>
        /// <param name="categoryId"> the id of the selected category </param>
        /// <param name="userId"> the id of the current user </param>
        /// <returns></returns>
        public bool InsertMessageCategory(string title , string content , int categoryId , int userId, string currentDate)
        {
            try
            {
                //Create a bijdrage, needed to create a bericht.
                OracleCommand cmd =
                    new OracleCommand("INSERT INTO BIJDRAGE" +
                                      "(\"ID\", \"account_id\", \"datum\", \"soort\") VALUES " +
                                      "(NULL, :userId, :currentDate, 'bericht')");

                cmd.Parameters.Add("userId", userId);
                cmd.Parameters.Add(":currentDate", currentDate);
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

        /// <summary>
        /// Inserts a reaction into the database.
        /// </summary>
        /// <param name="userId"> id of the user </param>
        /// <param name="title"> title of the reaction </param>
        /// <param name="content"> content of the reaction </param>
        /// <returns></returns>
        public bool InsertReaction(int messageId, int userId, string title, string content, string currentDate)
        {
            try
            {
                //Create a bijdrage, needed to create a bericht.
                OracleCommand cmd =
                    new OracleCommand("INSERT INTO BIJDRAGE" +
                                      "(\"ID\", \"account_id\", \"datum\", \"soort\") VALUES " +
                                      "(NULL, :userId, :currentDate, 'bericht')");

                cmd.Parameters.Add("userId", userId);
                cmd.Parameters.Add("currentDate", currentDate);
                Execute(cmd);

                //We need the id we just created, let's get it from the database.
                int reactionId = GetMaxBijdrageId();

                  //Create a bericht with the bijdrage id from the above query.
                OracleCommand cmdTwo =
                    new OracleCommand("INSERT INTO BERICHT" +
                                      "(\"bijdrage_id\", \"titel\", \"inhoud\") VALUES " +
                                      "(:reactionId, :title, :content)");

                cmdTwo.Parameters.Add("reactionId", reactionId);
                cmdTwo.Parameters.Add("title", title);
                cmdTwo.Parameters.Add("content", content);
                Execute(cmdTwo);

                OracleCommand cmdThree=
                     new OracleCommand("INSERT INTO BIJDRAGE_BERICHT" +
                                       "(\"bijdrage_id\", \"bericht_id\") VALUES " +
                                       "(:messageId, :reactionId)");

                cmdThree.Parameters.Add("messageId", messageId);
                cmdThree.Parameters.Add("reactionId", reactionId);
                Execute(cmdThree);

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

        /// <summary>
        /// Inserts a category into the database.
        /// </summary>
        /// <param name="name"> the name of the category </param>
        /// <param name="userId"> the id of the user </param>
        /// <returns></returns>
        public bool InsertCategory(string name, int userId, string currentDate)
        {
            try
            {
                //Create a bijdrage, needed to create a bericht.
                OracleCommand cmd =
                    new OracleCommand("INSERT INTO BIJDRAGE" +
                                      "(\"ID\", \"account_id\", \"datum\", \"soort\") VALUES " +
                                      "(NULL, :userId, :currentDate, 'categorie')");

                cmd.Parameters.Add("userId", userId);
                cmd.Parameters.Add("currentDate", currentDate);
                Execute(cmd);

                //We need the id we just created, let's get it from the database.
                int categoryId = GetMaxBijdrageId();

                //Create the corresponding category.
                OracleCommand cmdTwo =
                    new OracleCommand("INSERT INTO CATEGORIE" +
                                      "(\"bijdrage_id\", \"categorie_id\", \"naam\") VALUES " +
                                      "(:categoryId, NULL, :name')");

                cmdTwo.Parameters.Add("categoryId", categoryId);
                cmdTwo.Parameters.Add("name", name);
                Execute(cmdTwo);
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
            return true;
        }

        /// <summary>
        /// Inserts an item(bestand) into the database.
        /// </summary>
        /// <param name="userId"> id of the user</param>
        /// <param name="categoryId"> id of the category the item should be inserted in </param>
        /// <param name="title"> title of the message linked to the file </param>
        /// <param name="content"> content of the message linked to the file </param>
        /// <param name="fileLocation"> save location of the file on the server </param>
        /// <param name="size"> size of the image </param>
        /// <returns></returns>
        public bool InsertItem(int userId, int categoryId, string title, string content, string fileLocation, int size, string currentDate)
        {
            try
            {
                //Create a bijdrage, needed to create a bestand.
                OracleCommand cmd =
                    new OracleCommand("INSERT INTO BIJDRAGE" +
                                      "(\"ID\", \"account_id\", \"datum\", \"soort\") VALUES " +
                                      "(NULL, :userId, :currentDate, 'bestand')");

                cmd.Parameters.Add("userId", userId);
                cmd.Parameters.Add("currentDate", currentDate);
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
                                      "(NULL, :userId, :currentDate, 'bericht')");

                cmdThree.Parameters.Add("userId", userId);
                cmdThree.Parameters.Add("currentDate", currentDate);
                Execute(cmdThree);

                //We need the id we just created, let's get it from the database.
                int messageId = GetMaxBijdrageId();

                //Create a bericht with the bijdrage id from the above query.
                OracleCommand cmdFour =
                    new OracleCommand("INSERT INTO BERICHT" +
                                      "(\"bijdrage_id\", \"titel\", \"inhoud\") VALUES " +
                                      "(:messageId, :title, :content)");

                cmdFour.Parameters.Add("messageId", messageId);
                cmdFour.Parameters.Add("title", title);
                cmdFour.Parameters.Add("content", content);
                Execute(cmdFour);

                //Finally, we link the message(bericht) and file(bestand) togheter in the BIJDRAGE_BESTAND table.
                OracleCommand cmdFive =
                    new OracleCommand("INSERT INTO BIJDRAGE_BERICHT" +
                                      "(\"bijdrage_id\", \"bericht_id\") VALUES " +
                                      "(:fileId, :messageId)");

                cmdFive.Parameters.Add("fileId", fileId);
                cmdFive.Parameters.Add("messageId", messageId);
                Execute(cmdFive);

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

        /// <summary>
        /// Deletes a like from the database.
        /// </summary>
        /// <param name="id"> id of the "bijdrage" from which the like has to be deleted </param>
        /// <param name="userId"> id of the user </param>
        /// <returns></returns>
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

        #region Non Execute and Execute

        /// <summary>
        /// Execute a query with a return value.
        /// </summary>
        /// <param name="cmd"> OracleCommand </param>
        /// <returns></returns>
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

        /// <summary>
        /// Executes a query without a return value.
        /// </summary>
        /// <param name="cmd"> OracleCommand </param>
        /// <returns></returns>
        private void Execute(OracleCommand cmd)
        {
            System.Diagnostics.Debug.WriteLine("---------------");
            System.Diagnostics.Debug.WriteLine("Attempting to execute query: " + cmd.CommandText);
            try
            {
                OpenConnection();
                cmd.Connection = _connect;
                cmd.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine("COMPLETE");
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("---------- ERROR WHILE EXECUTING QUERY ----------");
                System.Diagnostics.Debug.WriteLine("Error while executing query: " + cmd.CommandText);
                System.Diagnostics.Debug.WriteLine("Error code: {0}", ex.ErrorCode);
                System.Diagnostics.Debug.WriteLine("Error message: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("---------- END OF EXCEPTION ----------");
            }
            finally
            {
                CloseConnection();
            }
        }

        #endregion
    }
}