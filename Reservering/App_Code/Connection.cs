using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Oracle.DataAccess;


namespace Reservering
{
    public class Connection
    {
        OracleConnection conn = new OracleConnection();
        private const string user = "system";
        private const string pw = "wachtwoord";
        private const string test = "xe";

        public bool NewConnection()
        {
            conn.ConnectionString = "User Id=" + user + ";Password=" + pw + ";Data Source=" +
                                    "//localhost:1521/" + test + ";";

            try
            {
                conn.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void CloseConnection()
        {
            conn.Close();
        }

        public static List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            Connection connection = new Connection();
        }
    }

}