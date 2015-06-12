using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Management;
using Reservering;

namespace Reservering
{
    /// <summary>
    /// This class will be used for all the insert statements.
    /// </summary>
    public class Insert
    {
        readonly Connection conn = new Connection();

        /// <summary>
        /// Method to be used for inserting a reservation.
        /// </summary>
        /// <returns>True or false, wether or not it succeeded.</returns>
        public string Insert_Reservation(int persoonId, DateTime startDateTime, DateTime endDateTime, int betaald)
        {
            try
            {
                var query = "EXECUTE Insert_Reservering('" + persoonId + "', '" + Convert.ToString(startDateTime) +
                            "', '" + Convert.ToString(endDateTime) + "', '" + betaald + "')";
                Connection.ExecuteQuery(query);
                const string result = "Reservering aangemaakt!";
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Method to be used to insert a new account.
        /// </summary>
        /// <returns>True or false, wether or not it succeeded.</returns>
        public string Insert_Account(string gebruikersnaam, string email)
        {
            try
            {
                var query = "EXECUTE Insert_Account('" + gebruikersnaam + "', '" + email + "')";
                conn.Execute(query);
                const string result = "Account gemaakt!";
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Insert_Acc_Res()
        {
            return true;
        }

        public string Insert_Persoon(string voornaam, string tussenvoegsel, string achternaam, string straat, string huisnummer, string woonplaats, string banknr)
        {
            try
            {
                var query = "EXECUTE Insert_Persoon('" + voornaam + "', '" + tussenvoegsel + "', '" + achternaam +
                               "', '" + straat + "', '" + huisnummer + "', '" + woonplaats + "', '" + banknr + "')";
                conn.Execute(query);
                const string result = "Persoon aangemaakt!";
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

}
