using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Oracle.DataAccess;


namespace Reservering
{
    /// <summary>
    /// This class will be used to connect to the database, and close the connection again.
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Fields.
        /// </summary>
        readonly OracleConnection _conn = new OracleConnection();
        readonly  OracleConnection _conn2 = new OracleConnection();
        private const string User = "system";
        private const string Pw = "wachtwoord";
        private const string Test = "xe";

        /// <summary>
        /// This method tries to open the connection.
        /// </summary>
        /// <returns>A bool to check if opening the connection succeeded.</returns>
        public bool NewConnection()
        {
            _conn.ConnectionString = "User Id=" + User + ";Password=" + Pw + ";Data Source=" +
                                    "//localhost:1521/" + Test + ";";
            
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
        /// Closes the connection.
        /// </summary>
        public void CloseConnection()
        {
            _conn.Close();
            _conn2.Close();
        }

        /// <summary>
        /// Method to get data out of the database.
        /// </summary>
        /// <param name="query"></param>    
        /// <returns>A list of dictionary objects with database information.</returns>
        public static List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            Database database = new Database();
            database.CloseConnection();
            
            if (database.NewConnection())
            {
                try
                {
                    OracleDataReader reader = new OracleCommand(query, database._conn).ExecuteReader();

                    while (reader.Read())
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        
                        //Loop through the fields, add them to row

                        for (int fieldId = 0; fieldId < reader.FieldCount; fieldId++)
                            row.Add(reader.GetName(fieldId), reader.GetValue(fieldId));

                        result.Add(row);
                    }
                    database._conn.Close();
                    return result;
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
            database._conn.Close();
            return result;
        }

        /// <summary>
        /// Executes a string that is given to it.
        /// </summary>
        /// <param name="sql">The string that is passed along.</param>
        public void Execute_withString(string sql)
        {
            string query = sql;


            if (!NewConnection()) return;
            // Command opzetten voor het uitvoeren van de query
            OracleCommand cmd = new OracleCommand(query, _conn);

            // Query uitvoeren, er wordt geen waarde terug gegeven
            cmd.ExecuteNonQuery();
            _conn.Close();
        }

        /// <summary>
        /// Runs the given command on the database.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns>True or false.</returns>
        public bool Execute(OracleCommand cmd)
        {
            try
            {
                //NewConnection();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (OracleException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + e.ErrorCode);
                return false;
            }

            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Inserts a reservation into the database.
        /// </summary>
        /// <param name="persoonId">ID of the person to whom the reservation belongs.</param>
        /// <param name="startDateTime">Start time/date of the reservation.</param>
        /// <param name="endDateTime">End time/date of the reservation.</param>
        /// <param name="betaald">Paid or not.</param>
        /// <returns>Succes string or error message.</returns>
        public string Insert_Reservering(int persoonId, DateTime startDateTime, DateTime endDateTime, int betaald)
        {

            try
            {
                const string x = "Connection failed";
                if (!NewConnection()) return x;
                OracleCommand cmd = new OracleCommand("Insert_Reservering", _conn2);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("PERSOON_ID", OracleDbType.Int32).Value = persoonId;
                cmd.Parameters.Add("DATUMSTART", OracleDbType.Date).Value = startDateTime;
                cmd.Parameters.Add("DATUMEINDE", OracleDbType.Date).Value = endDateTime;
                cmd.Parameters.Add("BETAALD", OracleDbType.Int32).Value = betaald;

                Execute(cmd);

                const string result = "Reservering aangemaakt!";
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// Will couple a reservation to a visitor's wristband.
        /// </summary>
        /// <returns>True or false.</returns>
        public bool Insert_Res_Bandje()
        {
            return true;
        }

        /// <summary>
        /// Inserts a new person into the database.
        /// </summary>
        /// <param name="p">Instance of the Person class.</param>
        public void Insert_Person(Person p)
        {

            if (!NewConnection()) return;

            OracleCommand cmd = new OracleCommand("Insert_Persoon", _conn2);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("VOORNAAM", OracleDbType.NVarchar2).Value = p.Voornaam;
            cmd.Parameters.Add("TUSSENVOEGSEL", OracleDbType.NVarchar2).Value = p.Tussenvoegsel;
            cmd.Parameters.Add("ACHTERNAAM", OracleDbType.NVarchar2).Value = p.Achternaam;
            cmd.Parameters.Add("STRAAT", OracleDbType.NVarchar2).Value = p.Straat;
            cmd.Parameters.Add("HUISNR", OracleDbType.NVarchar2).Value = p.Huisnr;
            cmd.Parameters.Add("WOONPLAATS", OracleDbType.NVarchar2).Value = p.Woonplaats;
            cmd.Parameters.Add("BANKNR", OracleDbType.NVarchar2).Value = p.Banknr;

            Execute(cmd);
        }



        private bool Insert_Account(string username, string email)
        {
            try
            {
                if (!NewConnection()) return false;
                OracleCommand cmd = new OracleCommand("INSERT INTO ACCOUNT (\"ID\", \"GEBRUIKERSNAAM\", \"EMAIL\", \"ACTIVATIEHASH\", \"GEACTIVEERD\") VALUES(null, :gebruikersnaam, :email, :activatiehash, :geactiveerd");
                cmd.Parameters.Add(":gebruikersnaam", _conn2).Value = username;
                cmd.Parameters.Add(":email", _conn2).Value = email;
                cmd.Parameters.Add(":activatiehash", _conn2).Value = Get_ActivationHash();
                cmd.Parameters.Add(":geactiveerd", _conn2).Value = 1;

                Execute(cmd);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string Get_ActivationHash()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 20)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public int Select_Persoon(string voornaam, string achternaam)
        {
            var sql = "SELECT ID FROM PERSOON WHERE voornaam = '" + voornaam + "' AND achternaam = '" + achternaam + "'";
            var id = 0;
            var data = ExecuteQuery(sql);

            foreach (Dictionary<string, object> row in data)
            {
                id = Convert.ToInt32(row["ID"]);
            }
            return id;
        }

        public List<Dictionary<string, object>> Select_Test_Personen()
        {
            const string sql = "SELECT \"locatie_id\" FROM plek";
            var data = Database.ExecuteQuery(sql);
            return data;
        }

        public string Person_Id(string voornaam, string achternaam, string straat)
        {
            try
            {
                string id = "";
                var sql = "SELECT \"ID\" FROM PERSOON WHERE \"voornaam\" = '" + voornaam + "' AND \"achternaam\" = '" +
                          achternaam + "' AND \"straat\" = '" + straat + "')";
                var data = Database.ExecuteQuery(sql);

                foreach (var x in data)
                {
                    id = Convert.ToString(x["ID"]);
                }
                return id;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public int Max_Polsbandje()
        {
            int id = 0;
            const string sql = "SELECT MAX(\"ID\") FROM POLSBANDJE";
            var data = Database.ExecuteQuery(sql);

            foreach (var x in data)
            {
                id = Convert.ToInt32(x["MAX(\"ID\")"]);
            }
            return id;
        }
    }

}