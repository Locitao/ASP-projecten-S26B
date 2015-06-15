using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Oracle.DataAccess;


namespace MateriaalBeheer
{
    public class DbConnection
    {
        #region Fields
        private OracleConnection connection = new OracleConnection();
        //Lazy instance of database, code example from cas eliens.
        private static readonly Lazy<DbConnection> instance = new Lazy<DbConnection>(() => new DbConnection());
        #endregion

        #region Properties
        public static DbConnection Instance { get { return instance.Value; } }
        #endregion

        private DbConnection()
        {
            
        }


        private void OpenConnection()
        {

            connection.ConnectionString = "User Id=proftaak;Password=proftaak;Data Source=localhost/XE";

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CloseConnection()
        {
            connection.Close();
        }

        public List<Dictionary<string, object>> GetGroupIdWithAdvertentieId()
        {
            OracleCommand oc = new OracleCommand("select pe.\"barcode\", v.\"prijs\" from productexemplaar pe, verhuur v where pe.\"ID\" = 1");

            //oc.Parameters.Add("groepId", advId);

            return ExecuteQuery(oc);
        }

        public List<Dictionary<string, object>> GetMaterials()
        {
            OracleCommand oc = new OracleCommand("select p.\"merk\", p.\"serie\" from verhuur v, productexemplaar pe, product p where p.\"ID\" = pe.\"product_id\" and pe.\"ID\" = v.\"res_pb_id\"");
            return ExecuteQuery(oc);
        }


        public List<Dictionary<string, object>> ExecuteQuery(OracleCommand oc)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            try
            {
                OpenConnection();
                oc.Connection = connection;
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
                System.Diagnostics.Debug.WriteLine(ex.ErrorCode + ex.Message);
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
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.Message + ex.ErrorCode);
            }
            finally
            {
                CloseConnection();
            }
        }
    }

    
}