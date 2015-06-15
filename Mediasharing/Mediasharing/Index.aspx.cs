using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;

namespace Mediasharing
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private int categorieId;
        protected void Page_Load(object sender, EventArgs e)
        {
            Rout();
            CheckIfLoggedIn();
            LoadPage();
        }

        public void Rout()
        {
            categorieId = Convert.ToInt32(Page.RouteData.Values["id"]);
        }

        public void LoadPage()
        {
            LoadCategories();
            LoadSubCategories();
        }

        public void CheckIfLoggedIn()
        {
            if (Session["account"] == null)
            {
                Response.Redirect("InlogPagina.aspx", false);
            }
        }


        public void LoadCategories()
        {
            DataSet output = new DataSet();

            try
            {
                if (categorieId == 0)
                {
                    Database database = Database.Instance;
                    output = database.GetData("SELECT c.\"naam\" AS NAAM, b.\"ID\" AS ID " +
                                                   "FROM BIJDRAGE b " +
                                                   "JOIN CATEGORIE c ON b.\"ID\" = c.\"bijdrage_id\" "+
                                                   "WHERE \"bijdrage_id\" IS NULL");
                }
                else
                {
                    Database database = Database.Instance;
                    output = database.GetData("SELECT c.\"naam\", b.\"ID\" " +
                                            "FROM BIJDRAGE b " +
                                            "JOIN CATEGORIE c ON b.\"ID\" = c.\"bijdrage_id\" " +
                                            "WHERE \"bijdrage_id\" = " + categorieId);
                }
                RepeaterCategorie.DataSource = output;
                RepeaterCategorie.DataBind();
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " + " + ex.ErrorCode);
            }

        }

        public void LoadSubCategories()
        {
            DataSet output = new DataSet();

            try
            {
                if (categorieId == 0)
                {
                    Database database = Database.Instance;
                    output = database.GetData("SELECT c.\"naam\" AS NAAM, b.\"ID\" AS ID " +
                                                   "FROM BIJDRAGE b " +
                                                   "JOIN CATEGORIE c ON b.\"ID\" = c.\"bijdrage_id\" "+
                                                   "WHERE \"categorie_id\" IS NULL");
                }
                else
                {
                    Database database = Database.Instance;
                    output = database.GetData("SELECT c.\"naam\", b.\"ID\" " +
                                            "FROM BIJDRAGE b " +
                                            "JOIN CATEGORIE c ON b.\"ID\" = c.\"bijdrage_id\" " +
                                            "WHERE \"categorie_id\" = " + categorieId);
                }
                RepeaterSubCategorie.DataSource = output;
                RepeaterSubCategorie.DataBind();
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " + " + ex.ErrorCode);
            }
        }
    }
}