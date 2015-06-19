using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mediasharing
{
    public partial class InlogPagina : System.Web.UI.Page
    {
        #region Methods
        /// <summary>
        /// The page load method, get's called everytime the page is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Events
        /// <summary>
        /// This event handles the logging in of the user.
        /// It checks wheter the input parameters match with the records in the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInloggen_Click(object sender, EventArgs e)
        {
            //Gets data from the textboxes.
            int id = Convert.ToInt32(tbId.Text);
            string wachtwoord = tbWachtwoord.Text;

            //Try to log in, if a wrong id is given the method returns null.
            Account user = Account.Login(id);

            //If the user is null, the wrong id was given.
            if (user == null)
            {
                //Wrong id and pasword combination!
                lblGegevens.Text = "Wrong id!";
            }
            else
            {
                //Log in
                Session["user"] = user;
                Response.Redirect("Index/0", true);
            }
        }
        #endregion
    }
}