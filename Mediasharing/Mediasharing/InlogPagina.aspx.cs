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
        //Events
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnInloggen_Click(object sender, EventArgs e)
        {
            string id = tbId.Text;
            string wachtwoord = tbWachtwoord.Text;
            Database database = Database.Instance;
            DataSet dataset = new DataSet();

            /*dataset = administratie.GetData("SELECT ID, Gebruikersnaam FROM ACCOUNT WHERE ID = " + "'" + id + "'" + " AND WACHTWOORD = " +
                                  "'" + wachtwoord + "'");
             */
            dataset = database.GetData("SELECT \"ID\", \"gebruikersnaam\" FROM ACCOUNT WHERE ID =  " + "'" + id + "'");

            if (IsEmpty(dataset))
            {
                
                //Wrong id and pasword combination!
                lblGegevens.Text = "Foute gebruikersnaam en wachtwoord combinatie!";
            }
            else
            {
                //Log in
                int idInt = Convert.ToInt32(dataset.Tables[0].Rows[0]["ID"]);
                string gebruikersnaam = dataset.Tables[0].Rows[0]["gebruikersnaam"].ToString();
                Account account = new Account(idInt, gebruikersnaam);
                Session["account"] = account;
                Response.Redirect("Index/0", true);
            }
        }

        //Methods
        /// <summary>
        /// If the dataset is empty ->  returns true, else returns false. </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        bool IsEmpty(DataSet dataSet)
        {
            return dataSet.Tables.Cast<DataTable>().All(table => table.Rows.Count == 0);
        }
    }
}