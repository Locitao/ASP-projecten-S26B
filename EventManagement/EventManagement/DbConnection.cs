using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Oracle.DataAccess.Client;

namespace MaterialRenting
{
    public class DbConnection
    {
        //Lazy instance of database, code example from cas eliens.
        private static readonly Lazy<DbConnection> instance = new Lazy<DbConnection>(() => new DbConnection());
        private readonly OracleConnection _connection = new OracleConnection();

        private DbConnection()
        {
            _connection.ConnectionString = "User Id=proftaak;Password=proftaak;Data Source=localhost/XE";
        }

        public static DbConnection Instance
        {
            get { return instance.Value; }
        }

        private void OpenConnection()
        {
            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CloseConnection()
        {
            _connection.Close();
        }

        public List<Dictionary<string, object>> ExecuteQuery(OracleCommand oc)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            try
            {
                OpenConnection();
                oc.Connection = _connection;
                using (OracleDataReader reader = oc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();

                        for (int fieldId = 0; fieldId < reader.FieldCount; fieldId++)
                        {
                            row.Add(reader.GetName(fieldId), reader.GetValue(fieldId));
                        }
                        result.Add(row);
                    }
                    return result;
                }
            }
            catch (OracleException ex)
            {
                Debug.WriteLine(ex.ErrorCode + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return null;
        }

        public void Execute(OracleCommand cmd)
        {
            try
            {
                OpenConnection();
                cmd.Connection = _connection;
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                Debug.WriteLine(ex.Message + ex.ErrorCode);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}