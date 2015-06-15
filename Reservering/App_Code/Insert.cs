using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Management;
using Oracle.DataAccess.Client;
using Reservering;

namespace Reservering
{
    /// <summary>
    /// This class will be used for all the insert statements.
    /// </summary>
    /// 
        
    public class Insert
    {
        Connection conn = new Connection();
        readonly OracleConnection conn2 = new OracleConnection();
        private const string user = "system";
        private const string pw = "wachtwoord";
        private const string test = "xe";


        public bool NewConnection()
        {
            conn2.ConnectionString = "User Id=" + user + ";Password=" + pw + ";Data Source=" +
                                    "//localhost:1521/" + test + ";";

            try
            {
                conn2.Open();
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
            conn2.Close();
        }
        /// <summary>
        /// Method to be used for inserting a reservation.
        /// </summary>
        /// <returns>True or false, wether or not it succeeded.</returns>
        public string Insert_Reservering(int persoonId, DateTime startDateTime, DateTime endDateTime, int betaald)
        {
            /*if (!NewConnection()) return;

            OracleCommand cmd = new OracleCommand("Insert_Persoon", conn2);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("VOORNAAM", OracleDbType.NVarchar2).Value = p.Voornaam;
            cmd.Parameters.Add("TUSSENVOEGSEL", OracleDbType.NVarchar2).Value = p.Tussenvoegsel; */

            try
            {
                const string x = "Connection failed";
                if (!NewConnection()) return x;
                OracleCommand cmd = new OracleCommand("Insert_Reservering", conn2);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("PERSOON_ID", OracleDbType.Int32).Value = persoonId;
                cmd.Parameters.Add("DATUMSTART", OracleDbType.Date).Value = startDateTime;
                cmd.Parameters.Add("DATUMEINDE", OracleDbType.Date).Value = endDateTime;
                cmd.Parameters.Add("BETAALD", OracleDbType.Int32).Value = betaald;

                conn.Execute(cmd);

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
        /*public string Insert_Account(string gebruikersnaam, string email)
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
            
            
        }*/
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Insert_Acc_Res()
        {
            return true;
        }

        public void Insert_Persoon(Person p)
        {
            /*
            try
            {
                var query = "Insert_Persoon('" + p.Voornaam + "', '" + p.Tussenvoegsel + "', '" + p.Achternaam +
                               "', '" + p.Straat + "', '" + p.Huisnr + "', '" + p.Woonplaats + "', '" + p.Banknr + "')";

                
                conn.Execute_Procedure(query);
                const string result = "Persoon aangemaakt!";
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            } */

            if (!NewConnection()) return;

            OracleCommand cmd = new OracleCommand("Insert_Persoon", conn2);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("VOORNAAM", OracleDbType.NVarchar2).Value = p.Voornaam;
            cmd.Parameters.Add("TUSSENVOEGSEL", OracleDbType.NVarchar2).Value = p.Tussenvoegsel;
            cmd.Parameters.Add("ACHTERNAAM", OracleDbType.NVarchar2).Value = p.Achternaam;
            cmd.Parameters.Add("STRAAT", OracleDbType.NVarchar2).Value = p.Straat;
            cmd.Parameters.Add("HUISNR", OracleDbType.NVarchar2).Value = p.Huisnr;
            cmd.Parameters.Add("WOONPLAATS", OracleDbType.NVarchar2).Value = p.Woonplaats;
            cmd.Parameters.Add("BANKNR", OracleDbType.NVarchar2).Value = p.Banknr;

            conn.Execute(cmd);
        }

        /*
        public void test
        {
            OracleCommand oc =
                new OracleCommand(
                    "INSERT INTO Advertentie (Prijs, AdvertentieID, Foto, GroepID, PersoonID, Leveren, Ophalen, Afmeting, Gewicht, Zendprijs, Titel, Contactnaam, Contactpostcode, Contacttelefoon, Aantalbezocht, Aantalfavoriet, Plaatsingsdatum, Conditie, Merk, Beschrijving, Website, Vasteprijs, Biedprijs)" +
                    "VALUES (:prijs, NULL, :foto, :categorieId, :persoonId, 1, 1, :gewicht, :afmetingen, 0, :titel, :naam, :postcode, :telnr, 0, 0, :datum, :conditie, :merk, :beschrijving, :website, 0, 1");

            oc.Parameters.Add("prijs", prijs);
            oc.Parameters.Add("foto", foto);
            oc.Parameters.Add("categorieId", categorieId);
            oc.Parameters.Add("persoonId", persoonId;
            oc.Parameters.Add("gewicht", gewicht);
            oc.Parameters.Add("afmetingen", afmetingen);
            oc.Parameters.Add("titel", titel);
            oc.Parameters.Add("naam", naam);
            oc.Parameters.Add("postcode", postcode);
            oc.Parameters.Add("telnr", telnr);
            oc.Parameters.Add("datum", datum);
            oc.Parameters.Add("conditie", conditie);
            oc.Parameters.Add(":merk", merk);
            oc.Parameters.Add("beschrijving", beschrijving);
            oc.Parameters.Add("website", website);
        }*/

        private bool Insert_Account(string username, string email)
        {
            try
            {
                if (!NewConnection()) return false;
                OracleCommand cmd = new OracleCommand("INSERT INTO ACCOUNT (\"ID\", \"GEBRUIKERSNAAM\", \"EMAIL\", \"ACTIVATIEHASH\", \"GEACTIVEERD\") VALUES(null, :gebruikersnaam, :email, :activatiehash, :geactiveerd");
                cmd.Parameters.Add(":gebruikersnaam", conn2).Value = username;
                cmd.Parameters.Add(":email", conn2).Value = email;
                cmd.Parameters.Add(":activatiehash", conn2).Value = Get_ActivationHash();
                cmd.Parameters.Add(":geactiveerd", conn2).Value = 1;

                conn.Execute(cmd);
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
    }

    

}
