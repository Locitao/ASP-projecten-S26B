using System;
using System.Web.UI;

namespace ReservationSystem
{
    public partial class Voorpagina : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Sends the user to the create an account page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReserve_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewAccount.aspx");
        }

        /// <summary>
        /// Sends the user to the page to only create an account. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAcc_Click(object sender, EventArgs e)
        {
            const int x = 1;
            Session["onlycreate"] = x;
            Response.Redirect("OnlyAcc.aspx");
        }
    }
}