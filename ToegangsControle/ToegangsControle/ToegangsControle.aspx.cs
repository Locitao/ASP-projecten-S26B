using System;
using System.Web.UI;

namespace ToegangsControle
{
    /// <summary>
    ///     This form will be used to manage Accesscontrol.
    /// </summary>
    public partial class ToegangsControle : Page
    {
        private readonly SqlQueries sqlQueries = new SqlQueries();
        private bool refresh = true;

        /// <summary>
        ///     Loads the page with all reservations into the listbox.
        ///     Gets called when page reloads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (refresh)
            {
                lbGegevens.Items.Clear();
                try
                {
                    var reservation = sqlQueries.Select_Reservation();

                    foreach (var row in reservation)
                    {
                        var present = Convert.ToString(row["aanwezig"]);
                        var payed = Convert.ToString(row["betaald"]);
                        var barcode = Convert.ToString(row["barcode"]);

                        if (present != "0")
                        {
                            present = "ja";
                        }
                        else
                        {
                            present = "nee";
                        }
                        if (payed != "0")
                        {
                            payed = "ja";
                        }
                        else
                        {
                            payed = "nee";
                        }
                        lbGegevens.Items.Add("reservering ID: " + row["reservering_id"] + ". Account ID: " +
                                             row["account_id"] + ". Barcode: " + barcode + ". Name: " +
                                             row["gebruikersnaam"] + ". payed?: " + payed + ". Present?: " + present);
                    }
                    refresh = false;
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "Scripts",
                        "<script>alert(" + ex.Message + ");</script>");
                }
            }
        }

        /// <summary>
        ///     Button refresh, refreshes the page which is followed by page_preRender.
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
        ///     Button shows all users which are on the terrain in the listbox.
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

                var attendees = sqlQueries.Select_allAttendees();

                foreach (var row in attendees)
                {
                    var present = Convert.ToString(row["aanwezig"]);

                    if (present != "0")
                    {
                        present = "ja";
                    }
                    else
                    {
                        present = "nee";
                    }

                    lbGegevens.Items.Add("Reservering_ID: " + row["reservering_id"] + ". Account_ID: " + row["ID"] +
                                         ". Name: " + row["gebruikersnaam"] + ". Present?: " + present);
                    refresh = false;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Scripts",
                    "<script>alert(" + ex.Message + ");</script>");
            }
        }

        /// <summary>
        ///     Button deletes the reservation of the selected reservation in the listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bttnAnuleren_Click(object sender, EventArgs e)
        {
            var reserveringsID = lbGegevens.SelectedItem.ToString().Substring(16, 1);
            sqlQueries.Delete_reserveringVerhuur(reserveringsID);
            sqlQueries.Delete_reserveringPolsbandje(reserveringsID);
            sqlQueries.Delete_reservering(reserveringsID);
        }

        /// <summary>
        ///     Button changes paid to 1 or 0 of the selected reservation in the listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bttnBetaald_Click(object sender, EventArgs e)
        {
            tbBarcode.Text = "Scan Code";
            var reservationID = lbGegevens.SelectedItem.ToString().Substring(16, 1);
            var userID = lbGegevens.SelectedItem.ToString().Substring(31, 1);
            string payed = sqlQueries.Update_Betaald(reservationID);
            refreshOnUserID(userID);

            refresh = false;
        }

        /// <summary>
        ///     When the page gets reloaded and the textbox is changed, the text gets checked for text length and changes present
        ///     to 1 or 0.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbBarcode_TextChanged(object sender, EventArgs e)
        {
            if (tbBarcode.Text.Length == 13)
            {
                var barcode = tbBarcode.Text;
                var numericOnly = true;

                foreach (var c in barcode)
                {
                    if (!Char.IsDigit(c))
                        numericOnly = false;
                }

                if (numericOnly)
                {
                    string userID = sqlQueries.HaalGebruikerIDVanBarcode(barcode);
                    string hasPayed = sqlQueries.checkBetaaldOnUserID(userID);
                    if (hasPayed != "0")
                    {
                        sqlQueries.Update_Aanwezig(userID);
                        tbBarcode.Text = "";
                    }
                    else
                    {
                        tbBarcode.Text = "User has not payed yet";
                    }
                    refreshOnUserID(userID);
                }
                else
                {
                    tbBarcode.Text = "Unvalid input";
                }
            }
        }

        /// <summary>
        ///     Method which loads the reservation of the given "gebruikerID" into the listbox.
        /// </summary>
        /// <param name="userID"></param>
        public void refreshOnUserID(string userID)
        {
            try
            {
                lbGegevens.Items.Clear();
                var reservering = sqlQueries.Select_reservationUserID(userID);

                foreach (var row in reservering)
                {
                    var aanwezig = Convert.ToString(row["aanwezig"]);
                    var betaald = Convert.ToString(row["betaald"]);
                    var barcode = Convert.ToString(row["barcode"]);

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
                    lbGegevens.Items.Add("reservering ID: " + row["reservering_id"] + ". Account ID: " +
                                         row["account_id"] + ". Barcode: " + barcode + ". Name: " +
                                         row["gebruikersnaam"] + ". Betaald?: " + betaald + ". Aanwezig?: " + aanwezig);
                }
                refresh = false;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Scripts",
                    "<script>alert(" + ex.Message + ");</script>");
            }
        }
    }
}