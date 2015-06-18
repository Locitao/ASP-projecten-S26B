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
            LoadItems();
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
            foreach (Material mat in materials)
            {
                lbProducts.Items.Add(mat.ToString());
            }
        }

        public List<Material> GetMaterialsInRented(List<string> barcodes)
        {
            List<Material> materials = new List<Material>();
            foreach (string barcode in barcodes)
            {
                string query = "select count(*) as VALUE from verhuur v, productexemplaar pe where \"productexemplaar_id\" = pe.id and \"barcode\" = '" + barcode + "'";
                List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));

                if ((int) (decimal)output[0]["VALUE"] == 1)
                {
                    query = "select pe.ID, \"merk\", \"serie\", p.\"prijs\", \"barcode\", \"datumIn\", \"datumUit\", \"betaald\" from product p, productexemplaar pe, verhuur v where p.ID = pe.\"product_id\" and v.\"productexemplaar_id\" = pe.ID and \"barcode\" = '" + barcode + "'";
                    List<Dictionary<string, object>> outputMaterials = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));
                    List<DateTime[]> datesList = new List<DateTime[]>();
                    DateTime[] dates = new DateTime[2];
                    dates[0] = (DateTime) outputMaterials[0]["datumIn"];
                    dates[1] = (DateTime) outputMaterials[0]["datumUit"];
                    datesList.Add(dates);
                    materials.Add(new Material((int)(long)outputMaterials[0]["ID"], (string)outputMaterials[0]["merk"], (string)outputMaterials[0]["serie"] , (int)(decimal)outputMaterials[0]["prijs"], (string)outputMaterials[0]["barcode"], datesList));
                }
                if ((int) (decimal)output[0]["VALUE"] > 1)
                {
                    query = "select pe.ID, \"merk\", \"serie\", p.\"prijs\", \"barcode\", \"datumIn\", \"datumUit\", \"betaald\" from product p, productexemplaar pe, verhuur v where p.ID = pe.\"product_id\" and v.\"productexemplaar_id\" = pe.ID and \"barcode\" = '" + barcode + "'";
                    List<Dictionary<string, object>> outputMaterials = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));
                    Material mat = null;
                    foreach (Dictionary<string, object> dic in outputMaterials)
                    {
                        if (mat == null) mat = new Material((int)(long)dic["ID"], (string)dic["merk"], (string)dic["serie"] , (int)(decimal)dic["prijs"], (string)dic["barcode"], new List<DateTime[]>());
                        mat.AddDates((DateTime)dic["datumIn"], (DateTime)dic["datumUit"]);
                    }
                    materials.Add(mat);
                }
                if ((int) (decimal)output[0]["VALUE"] == 0)
                {
                    query = "select pe.ID, \"merk\", \"serie\", p.\"prijs\", \"barcode\" from product p, productexemplaar pe where p.ID = pe.\"product_id\" and \"barcode\" = '" + barcode + "'";
                    List<Dictionary<string, object>> outputMaterial = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));
                    Dictionary<string, object> dic = outputMaterial[0];
                        materials.Add(new Material((int)(long)dic["ID"], (string)dic["merk"], (string)dic["serie"], (int)(decimal)dic["prijs"], (string)dic["barcode"], new List<DateTime[]>()));
                }
                
            }


            return materials;
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