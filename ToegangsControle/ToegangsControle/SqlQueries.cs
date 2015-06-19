using System;
using System.Collections.Generic;

namespace ToegangsControle
{
    /// <summary>
    ///     This class uses All te statements for SQL
    /// </summary>
    public class SqlQueries
    {
        private readonly Connection connect = new Connection();

        /// <summary>
        ///     All the select statements.
        /// </summary>
        public List<Dictionary<string, object>> Select_Reservation()
        {
            var sql =
                "SELECT rp.\"reservering_id\",rp.\"account_id\", p.\"barcode\", a.\"gebruikersnaam\", r.\"betaald\", rp.\"aanwezig\" FROM RESERVERING_POLSBANDJE rp, POLSBANDJE p, ACCOUNT a, RESERVERING r WHERE p.\"ID\" = rp.\"polsbandje_id\" AND r.\"ID\" = rp.\"reservering_id\" AND a.\"ID\" = rp.\"account_id\"";

            var data = Connection.ExecuteQuery(sql);
            return data;
        }

        /// <summary>
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> Select_reservationUserID(string userID)
        {
            var sql =
                "SELECT rp.\"reservering_id\",rp.\"account_id\", p.\"barcode\", a.\"gebruikersnaam\", r.\"betaald\", rp.\"aanwezig\" FROM RESERVERING_POLSBANDJE rp, POLSBANDJE p, ACCOUNT a, RESERVERING r WHERE p.\"ID\" = rp.\"polsbandje_id\" AND r.\"ID\" = rp.\"reservering_id\" AND a.\"ID\" = rp.\"account_id\" AND rp.\"account_id\" = '" +
                userID + "'";

            var data = Connection.ExecuteQuery(sql);
            return data;
        }

        public List<Dictionary<string, object>> Select_allAttendees()
        {
            var sql =
                "SELECT rp.\"reservering_id\", a.ID, a.\"gebruikersnaam\", rp.\"aanwezig\" FROM account a, reservering_polsbandje rp WHERE a.ID = rp.\"account_id\" AND rp.\"aanwezig\" = 1";

            var data = Connection.ExecuteQuery(sql);
            return data;
        }

        public string checkPayed(string reservationID)
        {
            var betaald = "2";
            var sql = "SELECT r.\"betaald\" FROM RESERVERING r WHERE r.\"ID\" = " + reservationID;

            var data = Connection.ExecuteQuery(sql);
            foreach (var row in data)
            {
                betaald = Convert.ToString(row["betaald"]);
            }

            return betaald;
        }

        public string checkPayedOnUserID(string userID)
        {
            var betaald = "2";
            var sql =
                "SELECT r.\"betaald\" FROM RESERVERING r, RESERVERING_POLSBANDJE rp WHERE r.\"ID\" = rp.\"reservering_id\" AND rp.\"account_id\" = " +
                userID;

            var data = Connection.ExecuteQuery(sql);
            foreach (var row in data)
            {
                betaald = Convert.ToString(row["betaald"]);
            }

            return betaald;
        }

        public string checkPresent(string userID)
        {
            var aanwezig = "";
            var sql = "SELECT \"aanwezig\" FROM RESERVERING_POLSBANDJE WHERE \"account_id\" = " + userID;

            var data = Connection.ExecuteQuery(sql);
            foreach (var row in data)
            {
                aanwezig = Convert.ToString(row["aanwezig"]);
            }
            return aanwezig;
        }

        public string getUserIDFromBarcode(string barcode)
        {
            var userID = "";
            var sql =
                "SELECT rp.\"account_id\" FROM POLSBANDJE p JOIN RESERVERING_POLSBANDJE rp ON p.ID = rp.\"polsbandje_id\" WHERE p.\"barcode\" = " +
                barcode;

            var data = Connection.ExecuteQuery(sql);
            foreach (var row in data)
            {
                userID = Convert.ToString(row["account_id"]);
            }
            return userID;
        }

        /// <summary>
        ///     All the update statements.
        /// </summary>
        public string Update_Payed(string reservationID)
        {
            try
            {
                var payed = 0;
                var currentStatus = checkPayed(reservationID);

                if (currentStatus == "1")
                {
                    payed = 0;
                }
                else
                {
                    payed = 1;
                }
                var query = "UPDATE RESERVERING r SET r.\"betaald\" = '" + payed + "' WHERE r.\"ID\" = '" +
                            reservationID + "'";
                connect.Execute(query);

                return "gebruiker heeft nu betaald";
            }
            catch
            {
                return "Error, update gefaald";
            }
        }

        public bool Update_Present(string userID)
        {
            try
            {
                var present = 0;
                var currentStatus = checkPresent(userID);

                if (currentStatus == "1")
                {
                    present = 0;
                }
                else
                {
                    present = 1;
                }
                var query = "UPDATE RESERVERING_POLSBANDJE r SET r.\"aanwezig\" = '" + present +
                            "' WHERE r.\"account_id\" = '" + userID + "'";
                connect.Execute(query);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     All the delete statements.
        /// </summary>
        public bool Delete_Reservation(string reservationID)
        {
            try
            {
                var query = "DELETE FROM RESERVERING WHERE id = '" + reservationID + "'";
                connect.Execute(query);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete_reservationPolsbandje(string reservationID)
        {
            try
            {
                var query = "DELETE FROM RESERVERING_POLSBANDJE WHERE \"reservering_id\" = '" + reservationID + "'";
                connect.Execute(query);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete_reservationRent(string reservationID)
        {
            try
            {
                var id = "";
                var sql =
                    "SELECT rp.id FROM VERHUUR v JOIN RESERVERING_POLSBANDJE rp ON v.\"res_pb_id\" = rp.id WHERE rp.\"reservering_id\" = " +
                    reservationID;

                var data = Connection.ExecuteQuery(sql);
                foreach (var row in data)
                {
                    id = Convert.ToString(row["ID"]);
                    var query = "DELETE FROM VERHUUR WHERE \"res_pb_id\" = '" + id + "'";
                    connect.Execute(query);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}