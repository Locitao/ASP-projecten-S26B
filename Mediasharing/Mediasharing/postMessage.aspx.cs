using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mediasharing
{
    public partial class PostMessage : System.Web.UI.Page
    {
        private Account user;
        private int categoryId;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = (Account)Session["user"];
            CheckIfLoggedIn();
            Rout();
        }

        public void Rout()
        {
            categoryId = Convert.ToInt32(Page.RouteData.Values["id"]);
        }

        public void CheckIfLoggedIn()
        {
            if (Session["user"] == null)
            {
                Response.Redirect("InlogPagina.aspx", true);
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (Bericht.InsertMessage(tbTitle.Text, tbContent.Text, categoryId, user.Id))
            {
                Response.Redirect("/Index/" + categoryId, true);
            }
            else
            {
                lblErrorMessage.Text = "Something went wrong";
                lblErrorMessage.CssClass = "error";
            }
        }
    }
}