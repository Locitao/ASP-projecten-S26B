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
        {
            string query = "select pe.ID, \"merk\", \"serie\", p.\"prijs\", \"barcode\", \"datumIn\", \"datumUit\", \"betaald\" from product p, productexemplaar pe, verhuur v where p.ID = pe.\"product_id\" and v.\"productexemplaar_id\" = pe.ID order by \"barcode\"";
            List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));
            List<Material> matList = new List<Material>();
            List<DateTime[]> dates = new List<DateTime[]>();
            string barcode = "";
            Material material = null;
            foreach (Dictionary<string, object> dic in output)
            {
                if (barcode == "")
                {
                    // first time this for loop will be called
                    barcode = (string)dic["barcode"];
                    dates = new List<DateTime[]>();
                    material = new Material((int)(long)dic["ID"], (string)dic["merk"], (string)dic["serie"], (int)(decimal)dic["prijs"], (string)dic["barcode"], null);
                }
                else if (barcode != (string) dic["barcode"])
                {
                    // if the barcode doesnt match the previous one the material will be saved.
                    material.RentingTimes = dates;
                    matList.Add(material);
                    barcode = (string)dic["barcode"];
                    dates = new List<DateTime[]>();
                    material = new Material((int)(long)dic["ID"], (string)dic["merk"], (string)dic["serie"], (int)(decimal)dic["prijs"], (string)dic["barcode"], null);
                }
                else
                {
                    // if we already have an instance with this barcode the dates will be added to that list.
                    DateTime[] tempDateTimes = new DateTime[2];
                    tempDateTimes[0] = (DateTime) dic["datumIn"];
                    tempDateTimes[1] = (DateTime) dic["datumUit"];
                    dates.Add(tempDateTimes);
                }

            }
            foreach (Material mat in matList)
            {
                lbProducts.Items.Add(mat.ToString());
            }
            
            
        }


        public void LoadItems()
        {
            List<string> barcodes = new List<string>();
            string query = "select distinct \"barcode\" from productexemplaar";
            List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));
            foreach (Dictionary<string, object> dic in output)
            {
                    barcodes.Add((string)dic["barcode"]);
            }

            List<Material> materials = GetMaterialsInRented(barcodes);
        }

        public List<Material> GetMaterialsInRented(List<string> barcodes)
        {
            foreach (string barcode in barcodes)
            {
                string query = "select count(*) as value from verhuur v, productexemplaar pe where \"productexemplaar_id\" = pe.id and \"barcode\" = '" + barcode + "'";
                List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));

                if ((int) output[0]["value"] == 1)
                {
                    //TODO: product uit database halen via onderstaande query en meteen in new Material() stoppen
                    //query = "select pe.ID, \"merk\", \"serie\", p.\"prijs\", \"barcode\", \"datumIn\", \"datumUit\", \"betaald\" from product p, productexemplaar pe, verhuur v where p.ID = pe.\"product_id\" and v.\"productexemplaar_id\" = pe.ID where \"barcode\" = '" + barcode + "'";
                }
                if ((int) output[0]["value"] > 1)
                {
                    //TODO: product uit database halen en vervolgens voor iedere tijd een tijd toevoegen
                }
                if ((int) output[0]["value"] == 0)
                {
                    //TODO: product uit tabbelen product en productexemplaar halen, dit product staat NIET in verhuur;
                }
                
            }


            return null;
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