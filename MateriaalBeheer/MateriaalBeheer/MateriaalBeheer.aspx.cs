using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialRenting;

namespace MateriaalBeheer
{
    public partial class MateriaalBeheer : System.Web.UI.Page
    {

        private DatabaseConnection dbConnection = new DatabaseConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write("<SCRIPT LANGUAGE='JavaScript'>alert('Hello this is an Alert')</SCRIPT>");
            pnlPopUpLeenItem.Visible = false;
            pnlPopUpReserveerItem.Visible = false;
            listBox.Items.Clear();
            List<List<string>> output;
            string query =
                "select \"barcode\",\"serie\" from productexemplaar pe, product p where p.id=pe.\"product_id\"";
            if (dbConnection.SQLQueryWithOutput(query, out output))
            {
                foreach (List<string> list in output)
                {
                    listBox.Items.Add(list[0] + " " + list[1]);
                }
            }
        }

        public void RefreshAllItems()
        {
            
        }

        public void btnLeenUit_Click(object sender, EventArgs e)
        {
            //Response.Write("<SCRIPT LANGUAGE='JavaScript'>alert('Hello this is an Alert')</SCRIPT>");
            pnlMain.Visible = false;
            pnlPopUpLeenItem.Visible = true;
        }

        public void btnLeenUitPopUp_Click(object sender, EventArgs e)
        {
            pnlMain.Visible = true;
            pnlPopUpLeenItem.Visible = false;
        }

        protected void btnReserveerPopUp_OnClick(object sender, EventArgs e)
        {
            pnlMain.Visible = true;
            pnlPopUpReserveerItem.Visible = false;
        }

        protected void BtnReserveer_OnClick(object sender, EventArgs e)
        {
            pnlMain.Visible = false;
            pnlPopUpReserveerItem.Visible = true;
        }

        protected void BtRetourneer_OnClick(object sender, EventArgs e)
        {
            DbConnection conn = DbConnection.Instance;

            List<Dictionary<string, object>> List = conn.GetMaterials();

            lbl.Text = (string)List[0]["merk"];
        }
    }
}