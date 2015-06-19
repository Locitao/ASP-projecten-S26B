using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Oracle.DataAccess.Client;


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
        private const string User = "ICT4EVENTS";
        private const string Pw = "ICT4EVENTS";
        private const string Test = "xe";

        /// <summary>
        /// This method tries to open the connection.
        /// </summary>
        /// <returns>A bool to check if opening the connection succeeded.</returns>
        public bool NewConnection()
        {
            _conn.ConnectionString = "User Id=" + User + ";Password=" + Pw + ";Data Source=" +
                                    "//localhost:1521/" + Test + ";";
            _conn2.ConnectionString = "User Id=" + User + ";Password=" + Pw + ";Data Source=" +
                                    "//localhost:1521/" + Test + ";";
            
            try
            {
                _conn.Open();
                _conn2.Open();
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
            //CloseConnection();
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
        public string Insert_Reservation(int persoonId, DateTime startDateTime, DateTime endDateTime, int betaald)
        {

            try
            {
                const string x = "Connection failed";
                if (!NewConnection()) return x;
                OracleCommand cmd = new OracleCommand("Insert_Reservation", _conn2)
                {
                    CommandType = CommandType.StoredProcedure
                };

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
        public bool New_Wristband()
        {
            var sql = "INSERT INTO POLSBANDJE (ID, \"barcode\", \"actief\") VALUES (null, '" + Get_Band_Code() +
                      "', '1')";
            ExecuteQuery(sql);
            return true;
        }

        /// <summary>
        /// Inserts a new person into the database.
        /// </summary>
        /// <param name="p">Instance of the Person class.</param>
        public void Insert_Person(Person p)
        {

            if (!NewConnection()) return;

            OracleCommand cmd = new OracleCommand("Insert_Persoon", _conn2) {CommandType = CommandType.StoredProcedure};

            cmd.Parameters.Add("VOORNAAM", OracleDbType.NVarchar2).Value = p.Voornaam;
            cmd.Parameters.Add("TUSSENVOEGSEL", OracleDbType.NVarchar2).Value = p.Tussenvoegsel;
            cmd.Parameters.Add("ACHTERNAAM", OracleDbType.NVarchar2).Value = p.Achternaam;
            cmd.Parameters.Add("STRAAT", OracleDbType.NVarchar2).Value = p.Straat;
            cmd.Parameters.Add("HUISNR", OracleDbType.NVarchar2).Value = p.Huisnr;
            cmd.Parameters.Add("WOONPLAATS", OracleDbType.NVarchar2).Value = p.Woonplaats;
            cmd.Parameters.Add("BANKNR", OracleDbType.NVarchar2).Value = p.Banknr;

            Execute(cmd);
        }

        /// <summary>
        /// Inserts an account into the database.
        /// </summary>
        /// <param name="username">Users' chosen username</param>
        /// <param name="email">Email of the user.</param>
        /// <returns>True or false</returns>
        public bool Insert_Account(string username, string email)
        {
            try
            {
                if (!NewConnection()) return false;
                OracleCommand cmd = new OracleCommand("INSERT INTO ACCOUNT (\"ID\", \"gebruikersnaam\", \"email\", \"activatiehash\", \"geactiveerd\") VALUES(null, :gebruikersnaam, :email, :activatiehash, :geactiveerd)", _conn2);
                cmd.Parameters.Add(":gebruikersnaam", OracleDbType.NVarchar2).Value = username;
                cmd.Parameters.Add(":email", OracleDbType.NVarchar2).Value = email;
                cmd.Parameters.Add(":activatiehash", OracleDbType.NVarchar2).Value = Get_ActivationHash();
                cmd.Parameters.Add(":geactiveerd", OracleDbType.Int32).Value = 1;

                Execute(cmd);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a new activation hash for use with a users' account.   
        /// </summary>
        /// <returns>Random hash (string).</returns>
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

        /// <summary>
        /// Generates a new code for a new wrist band. 
        /// </summary>
        /// <returns>String of the band code.</returns>
        private string Get_Band_Code()
        {
            const string chars = "0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, 20).Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }

        /// <summary>
        /// Just a test.
        /// </summary>
        /// <returns>Move along citizen.</returns>
        public List<Dictionary<string, object>> Select_Test_Personen()
        {
            const string sql = "SELECT \"locatie_id\" FROM plek";
            var data = ExecuteQuery(sql);
            return data;
        }

        /// <summary>
        /// Find a persons' ID.
        /// </summary>
        /// <param name="voornaam"></param>
        /// <param name="achternaam"></param>
        /// <param name="straat"></param>
        /// <returns></returns>
        public int Person_Id(string voornaam, string achternaam, string straat)
        {
            try
            {
                int id = 0;
                var sql = "SELECT \"ID\" FROM PERSOON WHERE \"voornaam\" = '" + voornaam + "' AND \"achternaam\" = '" +
                          achternaam + "' AND \"straat\" = '" + straat + "')";
                var data = ExecuteQuery(sql);

                foreach (var x in data)
                {
                    id = Convert.ToInt32(x["ID"]);
                }
                return id;

            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Find the latest wristband that was added to the database.
        /// </summary>
        /// <returns></returns>
        public int Max_Polsbandje()
        {
            int id = 0;
            const string sql = "SELECT MAX(\"ID\") FROM POLSBANDJE";
            var data = ExecuteQuery(sql);

            foreach (var x in data)
            {
                id = Convert.ToInt32(x["MAX(\"ID\")"]);
            }
            return id;
        }

        /// <summary>
        /// Find the latest reservation that was added to the database.
        /// </summary>
        /// <returns></returns>
        public int Max_Res()
        {
            int id = 0;
            const string sql = "SELECT MAX(\"ID\") FROM RESERVERING";
            var data = ExecuteQuery(sql);

            foreach (var x in data)
            {
                id = Convert.ToInt32(x["MAX(\"ID\")"]);
            }
            return id;
        }

        /// <summary>
        /// Checks if the given username already exists.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool Check_Username(string username)
        {
            const string sql = "SELECT \"gebruikersnaam\" FROM \"ACCOUNT\"";
            var data = ExecuteQuery(sql);

            List<string> unames = data.Select(x => Convert.ToString(x["gebruikersnaam"])).ToList();

            return unames.Any(s => username == s);
        }

        /// <summary>
        /// Returns a list of all locations that haven't been reserved yet.
        /// </summary>
        /// <returns></returns>
        public List<Location> Find_Locations()
        {
            const string sql =
                "SELECT ID, \"nummer\", \"capaciteit\" FROM PLEK WHERE ID NOT IN (SELECT \"plek_id\" FROM PLEK_RESERVERING)";
            var data = ExecuteQuery(sql);

            List<Location> locations = new List<Location>();

            foreach (var x in data)
            {
                int id = Convert.ToInt32(x["ID"]);
                int number = Convert.ToInt32(x["nummer"]);
                int capacity = Convert.ToInt32(x["capaciteit"]);
                Location l = new Location(id, capacity, number);
                locations.Add(l);
            }

            return locations;
        }

        /// <summary>
        /// Inserts a record into reservering_polsbandje.
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        public bool Insert_Res_Band(int accId)
        {
            if (!New_Wristband()) return false;

            int band = Max_Polsbandje();
            int res = Max_Res();
            const int present = 1;

            if (!NewConnection()) return false;

            OracleCommand cmd = new OracleCommand("INSERT INTO RESERVERING_POLSBANDJE (ID, \"reservering_id\", \"polsbandje_id\", \"account_id\", \"aanwezig\") VALUES (null, :resid, :band_id, :acc_id, :present)");
            cmd.Parameters.Add(":resid", _conn2).Value = res;
            cmd.Parameters.Add(":band_id", _conn2).Value = band;
            cmd.Parameters.Add(":acc_id", _conn2).Value = accId;
            cmd.Parameters.Add(":present", _conn2).Value = present;

            Execute(cmd);
            return true;
        }

        /// <summary>
        /// Find the ID of the account with the given username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int Find_Acc(string username)
        {
            int x = 0;

            var sql = "SELECT ID FROM ACCOUNT WHERE \"gebruikersnaam\" = '" + username + "'";
            var data = ExecuteQuery(sql);

            foreach (var y in data)
            {
                x = Convert.ToInt32(y["ID"]);
            }
            return x;
        }

        public bool Insert_Res_Spot(int resId, int locId)
        {
            OracleCommand cmd = new OracleCommand("INSERT INTO PLEK_RESERVERING (ID, \"plek_id\", \"reservering_id\") VALUES (null, :location, :reservation)");
            cmd.Parameters.Add(":location", _conn2).Value = locId;
            cmd.Parameters.Add(":reservation", _conn2).Value = resId;
            Execute(cmd);
            return true;
        }

        public List<Product> Find_Products()
        {
            List<Product> l = new List<Product>();

            const string sql =
                "SELECT pe.ID, p.\"merk\", p.\"prijs\", pc.\"naam\" FROM PRODUCTEXEMPLAAR pe, PRODUCT p, PRODUCTCAT pc WHERE pe.ID NOT IN (SELECT \"productexemplaar_id\" FROM VERHUUR) AND pe.\"product_id\" = p.ID AND p.\"product_id\" = p.ID AND p.\"productcat_id\" = pc.ID";

            var data = ExecuteQuery(sql);

            foreach (var x in data)
            {
                string brand = Convert.ToString(x["merk"]);
                int price = Convert.ToInt32(x["prijs"]);
                int id = Convert.ToInt32(x["ID"]);
                string name = Convert.ToString(x["naam"]);

                Product p = new Product(brand, price, id, name);
                l.Add(p);
            }

            return l;
        }

        
    }

}