using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialRenting;
using Microsoft.Win32.SafeHandles;
using Oracle.DataAccess.Client;

namespace MaterialRenting
{
    public partial class MateriaalBeheer : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write("<SCRIPT LANGUAGE='JavaScript'>alert('Hello this is an Alert')</SCRIPT>");
            pnlPopUpLeenItem.Visible = false;
            pnlPopUpReserveerItem.Visible = false;
            LoadAllItems();
        }

        public void LoadAllItems()
        {/*
            string query = "select pe.ID, \"merk\", \"serie\", p.\"prijs\", \"barcode\", \"datumIn\", \"datumUit\", \"betaald\" from product p, productexemplaar pe, verhuur v where p.ID = pe.\"product_id\" and v.\"productexemplaar_id\" = pe.ID";
            List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));
            List<Material> matList = new List<Material>();
            foreach (Dictionary<string, object> dic in output)
            {
                matList.Add(new Material((int)dic["ID"], (string)dic["merk"], ));

            }*/
        }

        public void RefreshAllItems()
        {
            if (tbDateFrom.Text.Length == 0
                || tbDateTo.Text.Length == 0)
            {
                // laat alle aanwezige items zien
                

            }
            else
            {
                
            }


        }

        public void btnLeenUit_Click(object sender, EventArgs e)
        {
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
        }
    }
}