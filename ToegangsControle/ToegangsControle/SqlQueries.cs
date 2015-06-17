using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToegangsControle
{
    /// <summary>
    /// This class uses All te statements for SQL
    /// </summary>
    public class SqlQueries
    {       
        Connection connect = new Connection();

        /// <summary>
        /// All the select statements.
        /// </summary>       
        public List<Dictionary<string, object>> Select_Reserveringen()
        {
            var sql = "SELECT rp.\"reservering_id\",rp.\"account_id\", p.\"barcode\", a.\"gebruikersnaam\", r.\"betaald\", rp.\"aanwezig\" FROM RESERVERING_POLSBANDJE rp, POLSBANDJE p, ACCOUNT a, RESERVERING r WHERE p.\"ID\" = rp.\"polsbandje_id\" AND r.\"ID\" = rp.\"reservering_id\" AND a.\"ID\" = rp.\"account_id\"";

            var data = Connection.ExecuteQuery(sql);
            return data;
        }

        public List<Dictionary<string, object>> Select_ReserveringAccountID(string acountID)
        {
            var sql = "SELECT rp.\"reservering_id\",rp.\"account_id\", p.\"barcode\", a.\"gebruikersnaam\", r.\"betaald\", rp.\"aanwezig\" FROM RESERVERING_POLSBANDJE rp, POLSBANDJE p, ACCOUNT a, RESERVERING r WHERE p.\"ID\" = rp.\"polsbandje_id\" AND r.\"ID\" = rp.\"reservering_id\" AND a.\"ID\" = rp.\"account_id\" AND rp.\"account_id\" = '" + acountID + "'";

            var data = Connection.ExecuteQuery(sql);
            return data;
        }
        public List<Dictionary<string, object>> Select_alleAanwezigen()
        {
            var sql = "SELECT rp.\"reservering_id\", a.ID, a.\"gebruikersnaam\", rp.\"aanwezig\" FROM account a, reservering_polsbandje rp WHERE a.ID = rp.\"account_id\" AND rp.\"aanwezig\" = 1";

            var data = Connection.ExecuteQuery(sql);
            return data;
        }
        public string checkBetaald(string reserveringID)
        {
            string betaald = "2";
            var sql = "SELECT r.\"betaald\" FROM RESERVERING r WHERE r.\"ID\" = " + reserveringID;

            var data = Connection.ExecuteQuery(sql);
            foreach (Dictionary<string, object> row in data)
            {
                betaald = Convert.ToString(row["betaald"]);
            }

            return betaald;            
        }
        public string checkBetaaldOnAccountID(string accountID)
        {
            string betaald = "2";
            var sql = "SELECT r.\"betaald\" FROM RESERVERING r, RESERVERING_POLSBANDJE rp WHERE r.\"ID\" = rp.\"reservering_id\" AND rp.\"account_id\" = " + accountID;

            var data = Connection.ExecuteQuery(sql);
            foreach (Dictionary<string, object> row in data)
            {
                betaald = Convert.ToString(row["betaald"]);
            }

            return betaald;
        }
        public string checkAanwezig(string userID)
        {
            string aanwezig = "";
            var sql = "SELECT \"aanwezig\" FROM RESERVERING_POLSBANDJE WHERE \"account_id\" = " + userID;

            var data = Connection.ExecuteQuery(sql);
            foreach (Dictionary<string, object> row in data)
            {
                aanwezig = Convert.ToString(row["aanwezig"]);
            }
            return aanwezig;
        }

        public string HaalGebruikerIDVanBarcode(string barcode)
        {
            string userID = "";
            var sql = "SELECT rp.\"account_id\" FROM POLSBANDJE p JOIN RESERVERING_POLSBANDJE rp ON p.ID = rp.\"polsbandje_id\" WHERE p.\"barcode\" = " + barcode;

            var data = Connection.ExecuteQuery(sql);
            foreach (Dictionary<string, object> row in data)
            {
                userID = Convert.ToString(row["account_id"]);
            }
            return userID;
        }
        /// <summary>
        /// All the update statements.
        /// </summary>       
        public string Update_Betaald(string reserveringID)
        {
            try
            {
                int betaald = 0;
                string huidigeStatus = checkBetaald(reserveringID);

                if (huidigeStatus == "1")
                {
                    betaald = 0;
                }
                else
                {
                    betaald = 1;
                }
                string query = "UPDATE RESERVERING r SET r.\"betaald\" = '" + betaald + "' WHERE r.\"ID\" = '" + reserveringID + "'";
                connect.Execute(query);

                return "gebruiker heeft nu betaald";
            }
            catch
            {
                return "Error, update gefaald";
            }
        }
        public bool Update_Aanwezig(string userID)
        {
            try
            {
                int aanwezig = 0;
                string huidigeStatus = checkAanwezig(userID);

                if (huidigeStatus == "1")
                {
                    aanwezig = 0;
                }
                else
                {
                    aanwezig = 1;
                }
                string query = "UPDATE RESERVERING_POLSBANDJE r SET r.\"aanwezig\" = '" + aanwezig + "' WHERE r.\"account_id\" = '" + userID + "'";
                connect.Execute(query);

                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// All the delete statements.
        /// </summary>       
        public bool Delete_reservering(string reserveringID)
        {
            try
            {
                string query = "DELETE FROM RESERVERING WHERE id = '" + reserveringID + "'";
                connect.Execute(query);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete_reserveringPolsbandje(string reserveringID)
        {
            try
            {
                string query = "DELETE FROM RESERVERING_POLSBANDJE WHERE \"reservering_id\" = '" + reserveringID + "'";
                connect.Execute(query);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete_reserveringVerhuur(string reserveringID)
        {
            try
            {
                string query = "DELETE FROM VERHUUR WHERE \"res_pb_id\" = '" + reserveringID + "'";
                connect.Execute(query);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}