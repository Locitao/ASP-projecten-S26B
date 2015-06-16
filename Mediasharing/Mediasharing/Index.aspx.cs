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
            LoadBerichten();
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
                System.Diagnostics.Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
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
                System.Diagnostics.Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
        }

        public void LoadMediaItems()
        {
            DataSet output = new DataSet();

            try
            {
                if (categorieId == 0)
                {
                    Database database = Database.Instance;
                    output = database.GetData("SELECT be.\"naam\" AS NAAM, b.\"ID\" AS ID " +
                                              "FROM BIJDRAGE b " +
                                              "JOIN BESTAND be ON b.\"ID\" = c.\"bijdrage_id\" " +
                                              "WHERE \"categorie_id\" IS NULL");
                }
                RepeaterSubCategorie.DataSource = output;
                RepeaterSubCategorie.DataBind();
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
        }

        public void LoadBerichten()
        {
            try
            {
                //Retrieves all the messages that aren't replies from the database.
                Database database = Database.Instance;
                List<Dictionary<string, object>> output = database.GetBerichten();
                List<Bericht> messages = new List<Bericht>();

                //Creates the messages.
                foreach (Dictionary<string, object> dic in output)
                {
                    int id = Convert.ToInt32(dic["ID"]);
                    string title = Convert.ToString(dic["TITEL"]);
                    string content = Convert.ToString(dic["INHOUD"]);
                    string username = Convert.ToString(dic["GEBRUIKERSNAAM"]);

                    Account account = new Account(username);
                    Bericht message = new Bericht(id, account, title, content);
                    messages.Add(message);
                }

                //Binds the messages to the listbox.
                lbMessages.DataSource = messages;
                lbMessages.DataTextField = "DisplayValue";
                lbMessages.DataValueField = "MessageId";
                lbMessages.DataBind();
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
        }
    }
}