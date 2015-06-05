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
            Administratie administratie = new Administratie();
            DataSet dataset = new DataSet();

            dataset = administratie.GetData("SELECT COUNT(*) FROM ACCOUNT WHERE ID = " + "'" + id + "'" + "AND WACHTWOORD = " +
                                  "'" + wachtwoord + "'");

            if (IsEmpty(dataset))
            {
                //Wrong id and pasword combination!
            }
            else
            {
                //Log in
                Account account = new Account();

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