using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace ToegangsControle
{
    public partial class ToegangsControle : System.Web.UI.Page
    {
        readonly SqlQueries sqlQueries = new SqlQueries(); 
        bool refresh = true;        
        protected void Page_PreRender(object sender, EventArgs e)
        {           
            if (refresh != false)
            {
                lbGegevens.Items.Clear();
                try
                {
                    var reservering = sqlQueries.Select_Reserveringen();
                    
                    foreach (Dictionary<string, object> row in reservering)
                    {
                        string aanwezig = Convert.ToString(row["aanwezig"]);
                        string betaald = Convert.ToString(row["betaald"]);
                        string barcode = Convert.ToString(row["barcode"]);                        

                        if (aanwezig != "0")
                        {
                            aanwezig = "ja";
                        }
                        else
                        {
                            aanwezig = "nee";
                        }
                        if (betaald != "0")
                        {
                            betaald = "ja";
                        }
                        else
                        {
                            betaald = "nee";
                        }                        
                        lbGegevens.Items.Add("reservering ID: " + row["reservering_id"] + ". Account ID: " + row["account_id"] + ". Barcode: " + barcode + ". Name: " + row["gebruikersnaam"] + ". Betaald?: " + betaald + ". Aanwezig?: " + aanwezig);

                    }
                    refresh = false;
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert(" + ex.Message + ");</script>");
                } 
            }            
        }

        protected void bttnRefresh_Click(object sender, EventArgs e)
        {
            bttnAanwezig.Enabled = true;
            bttnAnuleren.Enabled = true;
            bttnBetaald.Enabled = true;
            tbBarcode.Text = "Scan Code";
            refresh = true;
        }

        protected void bttnAanwezig_Click(object sender, EventArgs e)
        {            
            try
            {
                lbGegevens.Items.Clear();
                tbBarcode.Text = "Scan Code";
                bttnAanwezig.Enabled = false;
                bttnAnuleren.Enabled = false;
                bttnBetaald.Enabled = false;

                var aanwezigen = sqlQueries.Select_alleAanwezigen();

                foreach (Dictionary<string, object> row in aanwezigen)
                {
                    string aanwezig = Convert.ToString(row["aanwezig"]);

                    if (aanwezig != "0")
                    {
                        aanwezig = "ja";
                    }
                    else
                    {
                        aanwezig = "nee";
                    }

                    lbGegevens.Items.Add("Reservering_ID: " + row["reservering_id"] + ". Account_ID: " + row["ID"] + ". Name: " + row["gebruikersnaam"] + ". Aanwezig?: " + aanwezig);
                    refresh = false;
                }                
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert(" + ex.Message + ");</script>");
            }
        }

        protected void bttnAnuleren_Click(object sender, EventArgs e)
        {
            string reserveringsID = lbGegevens.SelectedItem.ToString().Substring(16, 1);
            sqlQueries.Delete_reserveringVerhuur(reserveringsID);
            sqlQueries.Delete_reserveringPolsbandje(reserveringsID);            
            sqlQueries.Delete_reservering(reserveringsID);
        }

        protected void bttnBetaald_Click(object sender, EventArgs e)
        {
            tbBarcode.Text = "Scan Code";
            string reserveringsID = lbGegevens.SelectedItem.ToString().Substring(16,1);
            string gebruikersID = lbGegevens.SelectedItem.ToString().Substring(31, 1);
            string betaald = sqlQueries.Update_Betaald(reserveringsID);
            verversOpGebruiker(gebruikersID);
            
            refresh = false;
        }
        protected void tbBarcode_TextChanged(object sender, System.EventArgs e)
        {
            if (tbBarcode.Text.Length == 13)
            {                
                string barcode = tbBarcode.Text;
                string userID = sqlQueries.HaalGebruikerIDVanBarcode(barcode);
                sqlQueries.Update_Aanwezig(userID);
                verversOpGebruiker(userID);
            }
        }
        public void verversOpGebruiker(string gebruikerID)
        {
            try
            {
                lbGegevens.Items.Clear();
                var reservering = sqlQueries.Select_ReserveringAccountID(gebruikerID);

                foreach (Dictionary<string, object> row in reservering)
                {
                    string aanwezig = Convert.ToString(row["aanwezig"]);
                    string betaald = Convert.ToString(row["betaald"]);
                    string barcode = Convert.ToString(row["barcode"]);

                    if (aanwezig != "0")
                    {
                        aanwezig = "ja";
                    }
                    else
                    {
                        aanwezig = "nee";
                    }
                    if (betaald != "0")
                    {
                        betaald = "ja";
                    }
                    else
                    {
                        betaald = "nee";
                    }
                    lbGegevens.Items.Add("reservering ID: " + row["reservering_id"] + ". Account ID: " + row["account_id"] + ". Barcode: " + barcode + ". Name: " + row["gebruikersnaam"] + ". Betaald?: " + betaald + ". Aanwezig?: " + aanwezig);

                }
                refresh = false;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert(" + ex.Message + ");</script>");
            }
        }

    }
}