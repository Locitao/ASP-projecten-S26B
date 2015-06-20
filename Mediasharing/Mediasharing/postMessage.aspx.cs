using System;

namespace Mediasharing
{
    /// <summary>
    /// This page contains the information, event and methods that allow the user to post a message.
    /// </summary>
    public partial class PostMessage : System.Web.UI.Page
    {
        #region Fields
        private Account _user;
        private int _categoryId;
        #endregion

        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            _user = (Account)Session["user"];
            CheckIfLoggedIn();
            Rout();
        }

        public void Rout()
        {
            _categoryId = Convert.ToInt32(Page.RouteData.Values["id"]);
        }

        public void CheckIfLoggedIn()
        {
            if (Session["user"] == null)
            {
                Response.Redirect("InlogPagina.aspx", true);
            }
        }
        #endregion

        #region Events

        /// <summary>
        /// Inserts a message(bericht) into the database, shows an error message if something went wrong.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (Bericht.InsertMessage(tbTitle.Text, tbContent.Text, _categoryId, _user.Id))
            {
                Response.Redirect("/Index/" + _categoryId, true);
            }
            else
            {
                lblErrorMessage.Text = "Something went wrong";
                lblErrorMessage.CssClass = "error";
            }
        }

        /// <summary>
        /// Redirects you back to the page you came from.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Index/" + _categoryId, true);
        }
        #endregion
    }
}