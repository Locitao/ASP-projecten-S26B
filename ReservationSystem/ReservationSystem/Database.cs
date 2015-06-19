using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Oracle.DataAccess.Client;

namespace ReservationSystem
{
    /// <summary>
    ///     This class will be used to connect to the database, and close the connection again.
    /// </summary>
    public class Database
    {
        private const string User = "system";
        private const string Pw = "wachtwoord";
        private const string Test = "xe";

        /// <summary>
        ///     Fields.
        /// </summary>
        private readonly OracleConnection _conn = new OracleConnection();

        private readonly OracleConnection _conn2 = new OracleConnection();

        /// <summary>
        ///     This method tries to open the connection.
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
        ///     Closes the connection.
        /// </summary>
        public void CloseConnection()
        {
            _conn.Close();
            _conn2.Close();
        }

        /// <summary>
        ///     Method to get data out of the database.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A list of dictionary objects with database information.</returns>
        public static List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            var result = new List<Dictionary<string, object>>();

            var database = new Database();
            database.CloseConnection();

            if (database.NewConnection())
            {
                var reader = new OracleCommand(query, database._conn).ExecuteReader();

                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();

                    //Loop through the fields, add them to row

                    for (var fieldId = 0; fieldId < reader.FieldCount; fieldId++)
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
        ///     Executes a string that is given to it.
        /// </summary>
        /// <param name="sql">The string that is passed along.</param>
        public void Execute_withString(string sql)
        {
            var query = sql;


            if (!NewConnection()) return;
            // Command opzetten voor het uitvoeren van de query
            var cmd = new OracleCommand(query, _conn);

            // Query uitvoeren, er wordt geen waarde terug gegeven
            cmd.ExecuteNonQuery();
            _conn.Close();
        }

        /// <summary>
        ///     Runs the given command on the database.
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
                Debug.WriteLine(e.Message + e.ErrorCode);
                return false;
            }

            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        ///     Inserts a reservation into the database.
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
                var cmd = new OracleCommand("Insert_Reservation", _conn2)
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
        ///     Will couple a reservation to a visitor's wristband.
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
        ///     Inserts a new person into the database.
        /// </summary>
        /// <param name="p">Instance of the Person class.</param>
        public void Insert_Person(Person p)
        {
            if (!NewConnection()) return;

            var cmd = new OracleCommand("Insert_Persoon", _conn2) {CommandType = CommandType.StoredProcedure};

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
        ///     Inserts an account into the database.
        /// </summary>
        /// <param name="username">Users' chosen username</param>
        /// <param name="email">Email of the user.</param>
        /// <returns>True or false</returns>
        public bool Insert_Account(string username, string email)
        {
            try
            {
                if (!NewConnection()) return false;
                var cmd =
                    new OracleCommand(
                        "INSERT INTO ACCOUNT (\"ID\", \"gebruikersnaam\", \"email\", \"activatiehash\", \"geactiveerd\") VALUES(null, :gebruikersnaam, :email, :activatiehash, :geactiveerd)",
                        _conn2);
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
        ///     Creates a new activation hash for use with a users' account.
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
        ///     Generates a new code for a new wrist band.
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
        ///     Just a test.
        /// </summary>
        /// <returns>Move along citizen.</returns>
        public List<Dictionary<string, object>> Select_Test_Personen()
        {
            const string sql = "SELECT \"locatie_id\" FROM plek";
            var data = ExecuteQuery(sql);
            return data;
        }

        /// <summary>
        ///     Find a persons' ID.
        /// </summary>
        /// <param name="voornaam"></param>
        /// <param name="achternaam"></param>
        /// <param name="straat"></param>
        /// <returns></returns>
        public int Person_Id(string voornaam, string achternaam, string straat)
        {
            try
            {
                var id = 0;
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
        ///     Find the latest wristband that was added to the database.
        /// </summary>
        /// <returns></returns>
        public int Max_Polsbandje()
        {
            var id = 0;
            const string sql = "SELECT MAX(\"ID\") FROM POLSBANDJE";
            var data = ExecuteQuery(sql);

            foreach (var x in data)
            {
                id = Convert.ToInt32(x["MAX(\"ID\")"]);
            }
            return id;
        }

        /// <summary>
        ///     Find the latest reservation that was added to the database.
        /// </summary>
        /// <returns></returns>
        public int Max_Res()
        {
            var id = 0;
            const string sql = "SELECT MAX(\"ID\") FROM RESERVERING";
            var data = ExecuteQuery(sql);

            foreach (var x in data)
            {
                id = Convert.ToInt32(x["MAX(\"ID\")"]);
            }
            return id;
        }

        /// <summary>
        ///     Checks if the given username already exists.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool Check_Username(string username)
        {
            const string sql = "SELECT \"gebruikersnaam\" FROM \"ACCOUNT\"";
            var data = ExecuteQuery(sql);

            var unames = data.Select(x => Convert.ToString(x["gebruikersnaam"])).ToList();

            return unames.Any(s => username == s);
        }

        /// <summary>
        ///     Returns a list of all locations that haven't been reserved yet.
        /// </summary>
        /// <returns></returns>
        public List<Location> Find_Locations()
        {
            const string sql =
                "SELECT ID, \"nummer\", \"capaciteit\" FROM PLEK WHERE ID NOT IN (SELECT \"plek_id\" FROM PLEK_RESERVERING)";
            var data = ExecuteQuery(sql);

            var locations = new List<Location>();

            foreach (var x in data)
            {
                var id = Convert.ToInt32(x["ID"]);
                var number = Convert.ToInt32(x["nummer"]);
                var capacity = Convert.ToInt32(x["capaciteit"]);
                var l = new Location(id, capacity, number);
                locations.Add(l);
            }

            return locations;
        }

        /// <summary>
        ///     Inserts a record into reservering_polsbandje.
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        public bool Insert_Res_Band(int accId)
        {
            if (!New_Wristband()) return false;

            var band = Max_Polsbandje();
            var res = Max_Res();
            const int present = 1;

            if (!NewConnection()) return false;

            var cmd =
                new OracleCommand(
                    "INSERT INTO RESERVERING_POLSBANDJE (ID, \"reservering_id\", \"polsbandje_id\", \"account_id\", \"aanwezig\") VALUES (null, :resid, :band_id, :acc_id, :present)",
                    _conn2);
            cmd.Parameters.Add(":resid", OracleDbType.Int32).Value = res;
            cmd.Parameters.Add(":band_id", OracleDbType.Int32).Value = band;
            cmd.Parameters.Add(":acc_id", OracleDbType.Int32).Value = accId;
            cmd.Parameters.Add(":present", OracleDbType.Int32).Value = present;

            Execute(cmd);
            return true;
        }

        /// <summary>
        ///     Find the ID of the account with the given username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int Find_Acc(string username)
        {
            var x = 0;

            var sql = "SELECT ID FROM ACCOUNT WHERE \"gebruikersnaam\" = '" + username + "'";
            var data = ExecuteQuery(sql);

            foreach (var y in data)
            {
                x = Convert.ToInt32(y["ID"]);
            }
            return x;
        }

        /// <summary>
        ///     Insert a Reservation_Spot in the database, to set the given spot on reserved.
        /// </summary>
        /// <param name="resId"></param>
        /// <param name="locId"></param>
        /// <returns></returns>
        public bool Insert_Res_Spot(int resId, int locId)
        {
            if (!NewConnection()) return false;

            var cmd =
                new OracleCommand(
                    "INSERT INTO PLEK_RESERVERING (ID, \"plek_id\", \"reservering_id\") VALUES (null, :location, :reservation)",
                    _conn2);
            cmd.Parameters.Add(":location", OracleDbType.Int32).Value = locId;
            cmd.Parameters.Add(":reservation", OracleDbType.Int32).Value = resId;
            Execute(cmd);
            return true;
        }

        /// <summary>
        ///     Find a list of all products that haven't been rented out yet.
        /// </summary>
        /// <returns></returns>
        public List<Product> Find_Products()
        {
            var l = new List<Product>();

            const string sql =
                "SELECT pe.ID, p.\"merk\", p.\"prijs\", pc.\"naam\" FROM PRODUCTEXEMPLAAR pe, PRODUCT p, PRODUCTCAT pc WHERE pe.ID NOT IN (SELECT \"productexemplaar_id\" FROM VERHUUR) AND pe.\"product_id\" = p.ID AND p.\"productcat_id\" = pc.ID";

            var data = ExecuteQuery(sql);

            foreach (var x in data)
            {
                var brand = Convert.ToString(x["merk"]);
                var price = Convert.ToInt32(x["prijs"]);
                var id = Convert.ToInt32(x["ID"]);
                var name = Convert.ToString(x["naam"]);

                var p = new Product(brand, price, id, name);
                l.Add(p);
            }

            return l;
        }

        /// <summary>
        ///     Inserts a reservation of a material with the given dates into the database.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="price"></param>
        /// <param name="paid"></param>
        /// <returns>True or false if it succeeded or not.</returns>
        public bool Insert_Mat_Res(int productId, DateTime start, DateTime end, int price, int paid)
        {
            var resId = Max_Res();

            if (!NewConnection()) return false;
            var cmd =
                new OracleCommand(
                    "INSERT INTO VERHUUR (ID, \"productexemplaar_id\", \"res_pb_id\", \"datumIn\", \"datumUit\", \"prijs\", \"betaald\") VALUES (null, :peId, :resId, :dateIn, :dateOut, :price, :paid)",
                    _conn2);
            cmd.Parameters.Add(":peId", OracleDbType.Int32).Value = productId;
            cmd.Parameters.Add(":resId", OracleDbType.Int32).Value = resId;
            cmd.Parameters.Add(":dateIn", OracleDbType.Date).Value = start;
            cmd.Parameters.Add("dateOut", OracleDbType.Date).Value = end;
            cmd.Parameters.Add(":price", OracleDbType.Int32).Value = price;
            cmd.Parameters.Add(":paid", OracleDbType.Int32).Value = paid;

            Execute(cmd);
            return true;
        }
    }
}