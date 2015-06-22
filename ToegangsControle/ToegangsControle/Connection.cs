using System;
using System.Collections.Generic;
using Oracle.DataAccess.Client;

namespace ToegangsControle
{
    /// <summary>
    ///     This class does everything regarding the connection; creates it, opens it and closes it.
    /// </summary>
    public class Connection
    {
        private readonly OracleConnection _conn = new OracleConnection();

        /// <summary>
        ///     Tries to open a connection with the database, returns true if succeeded, false if failed.
        /// </summary>
        /// <returns></returns>
        public bool NewConnection()
        {
            // Athena
            //const string user = "dbi321380";
            //const string pw = "HRs7Usr4Bz";
            //const string test = "fhictora";

            //_conn.ConnectionString = "User Id=" + user + ";Password=" + pw + ";Data Source=" +
            //                         "//192.168.15.50:1521/" + test + ";";

            // Local
            const string user = "system";
            const string pw = "wachtwoord";
            const string test = "xe";

            _conn.ConnectionString = "User Id=" + user + ";Password=" + pw + ";Data Source=" +
                                     "//localhost:1521/" + test + ";";
            try
            {
                _conn.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     CloseConnection() does what it says.
        /// </summary>
        public void CloseConnection()
        {
            _conn.Close();
        }

        /// <summary>
        ///     Sends the given query to the database, returns it's result as a List of dictionary objects.
        ///     Most commonly used with SELECT statements
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            var result = new List<Dictionary<string, object>>();

            var connection = new Connection();
            connection.CloseConnection();


            if (connection.NewConnection())
            {
                var resultReader = new OracleCommand(query, connection._conn).ExecuteReader();

                while (resultReader.Read())
                {
                    var row = new Dictionary<string, object>();

                    //Loop through fields, add them to the row

                    for (var fieldId = 0; fieldId < resultReader.FieldCount; fieldId++)
                        row.Add(resultReader.GetName(fieldId), resultReader.GetValue(fieldId));

                    result.Add(row);
                }
                connection._conn.Close();
                return result;


                //Loop through files, add them to result
            }
            connection._conn.Close();
            return result;
        }

        /// <summary>
        ///     Execute is for every query that shouldn't return something, so insert and update statements go
        ///     through this method.
        /// </summary>
        /// <param name="sql"></param>
        public void Execute(string sql)
        {
            var query = sql;


            if (!NewConnection()) return;
            // Command opzetten voor het uitvoeren van de query
            var cmd = new OracleCommand(query, _conn);

            // Query uitvoeren, er wordt geen waarde terug gegeven
            cmd.ExecuteNonQuery();
            _conn.Close();
        }
    }
}