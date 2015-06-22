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
                lbContent.Items.Clear();
                try
                {
                    var reservation = sqlQueries.Select_Reservation();

                    foreach (var row in reservation)
                    {
                        var present = Convert.ToString(row["aanwezig"]);
                        var paid = Convert.ToString(row["betaald"]);
                        var barcode = Convert.ToString(row["barcode"]);

                        if (present != "0")
                        {
                            present = "yes";
                        }
                        else
                        {
                            present = "no";
                        }
                        if (paid != "0")
                        {
                            paid = "yes";
                        }
                        else
                        {
                            paid = "no";
                        }
                        lbContent.Items.Add("reservering ID: " + row["reservering_id"] + ". Account ID: " +
                                             row["account_id"] + ". Barcode: " + barcode + ". Name: " +
                                             row["gebruikersnaam"] + ". paid?: " + paid + ". Present?: " + present);
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
            bttnPresent.Enabled = true;
            bttnCancel.Enabled = true;
            bttnPaid.Enabled = true;
            tbBarcode.Text = "Scan Code";
            refresh = true;
        }

        /// <summary>
        ///     Button shows all users which are on the terrain in the listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bttnPresent_Click(object sender, EventArgs e)
        {
            try
            {
                lbContent.Items.Clear();
                tbBarcode.Text = "Scan Code";
                bttnPresent.Enabled = false;
                bttnCancel.Enabled = false;
                bttnPaid.Enabled = false;

                var attendees = sqlQueries.Select_allAttendees();

                foreach (var row in attendees)
                {
                    var present = Convert.ToString(row["aanwezig"]);

                    if (present != "0")
                    {
                        present = "yes";
                    }
                    else
                    {
                        present = "no";
                    }

                    lbContent.Items.Add("Reservering_ID: " + row["reservering_id"] + ". Account_ID: " + row["ID"] +
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
        protected void bttnCancel_Click(object sender, EventArgs e)
        {
            var reserveringsID = "";
            try
            {
                reserveringsID = lbContent.SelectedItem.ToString().Substring(16, 1);
            }
            catch
            {
                tbBarcode.Text = "No item selected";
            }

            sqlQueries.Delete_reservationRent(reserveringsID);
            sqlQueries.Delete_reservationPolsbandje(reserveringsID);
            sqlQueries.Delete_Reservation(reserveringsID);
        }

        /// <summary>
        ///     Button changes paid to 1 or 0 of the selected reservation in the listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bttnPaid_Click(object sender, EventArgs e)
        {
            try
            {
                tbBarcode.Text = "Scan Code";
                var reservationID = lbContent.SelectedItem.ToString().Substring(16, 1);
                var userID = lbContent.SelectedItem.ToString().Substring(31, 1);
                string payed = sqlQueries.Update_Payed(reservationID);
                refreshOnUserID(userID);
            }
            catch
            {
                tbBarcode.Text = "No item selected";
            }

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
                    string userID = sqlQueries.getUserIDFromBarcode(barcode);
                    string hasPayed = sqlQueries.checkPayedOnUserID(userID);
                    if (hasPayed != "0")
                    {
                        sqlQueries.Update_Present(userID);
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
                    tbBarcode.Text = "Invalid input";
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
                lbContent.Items.Clear();
                var reservering = sqlQueries.Select_reservationUserID(userID);

                foreach (var row in reservering)
                {
                    var present = Convert.ToString(row["aanwezig"]);
                    var paid = Convert.ToString(row["betaald"]);
                    var barcode = Convert.ToString(row["barcode"]);

                    if (present != "0")
                    {
                        present = "yes";
                    }
                    else
                    {
                        present = "no";
                    }
                    if (paid != "0")
                    {
                        paid = "yes";
                    }
                    else
                    {
                        paid = "no";
                    }
                    lbContent.Items.Add("reservering ID: " + row["reservering_id"] + ". Account ID: " +
                                         row["account_id"] + ". Barcode: " + barcode + ". Name: " +
                                         row["gebruikersnaam"] + ". Paid?: " + paid + ". Present?: " + present);
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