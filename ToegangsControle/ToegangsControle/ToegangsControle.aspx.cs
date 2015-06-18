using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace ToegangsControle
{
    /// <summary>
    /// This form will be used to manage Accesscontrol.
    /// </summary>
    public partial class ToegangsControle : System.Web.UI.Page
    {
        readonly SqlQueries sqlQueries = new SqlQueries(); 
        bool refresh = true;
        /// <summary>
        /// Loads the page with all reservations into the listbox.
        /// Gets called when page reloads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Button refresh, refreshes the page which is followed by page_preRender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bttnRefresh_Click(object sender, EventArgs e)
        {
            bttnAanwezig.Enabled = true;
            bttnAnuleren.Enabled = true;
            bttnBetaald.Enabled = true;
            tbBarcode.Text = "Scan Code";
            refresh = true;
        }

        /// <summary>
        /// Button shows all users which are on the terrain in the listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Button deletes the reservation of the selected reservation in the listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bttnAnuleren_Click(object sender, EventArgs e)
        {
            string reserveringsID = lbGegevens.SelectedItem.ToString().Substring(16, 1);
            sqlQueries.Delete_reserveringVerhuur(reserveringsID);
            sqlQueries.Delete_reserveringPolsbandje(reserveringsID);            
            sqlQueries.Delete_reservering(reserveringsID);
        }

        /// <summary>
        /// Button changes paid to 1 or 0 of the selected reservation in the listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bttnBetaald_Click(object sender, EventArgs e)
        {
            tbBarcode.Text = "Scan Code";
            string reserveringsID = lbGegevens.SelectedItem.ToString().Substring(16,1);
            string gebruikersID = lbGegevens.SelectedItem.ToString().Substring(31, 1);
            string betaald = sqlQueries.Update_Betaald(reserveringsID);
            verversOpGebruiker(gebruikersID);
            
            refresh = false;
        }

        /// <summary>
        /// When the page gets reloaded and the textbox is changed, the text gets checked for text length and changes present to 1 or 0.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbBarcode_TextChanged(object sender, System.EventArgs e)
        {
            if (tbBarcode.Text.Length == 13)
            {                
                string barcode = tbBarcode.Text;                
                bool numericOnly = true;

                foreach (char c in barcode)
                {
                    if (!Char.IsDigit(c))
                        numericOnly = false;
                }

                if (numericOnly != false)
                {
                    string accountID = sqlQueries.HaalGebruikerIDVanBarcode(barcode);
                    string heeftBetaald = sqlQueries.checkBetaaldOnAccountID(accountID);                    
                    if (heeftBetaald != "0")
                    {
                        sqlQueries.Update_Aanwezig(accountID);
                        tbBarcode.Text = "";
                    }
                    else
                    {
                        tbBarcode.Text = "Gebruiker heeft nog niet betaald";
                    }
                    verversOpGebruiker(accountID);                    
                }
                else
                {
                    tbBarcode.Text = "Ongeldige input";
                }
            }
        }

        /// <summary>
        /// Method which loads the reservation of the given "gebruikerID" into the listbox.
        /// </summary>
        /// <param name="gebruikerID"></param>        
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