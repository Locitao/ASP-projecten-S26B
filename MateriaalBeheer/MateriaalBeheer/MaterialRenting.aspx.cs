﻿using System;
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
    public partial class MateriaalBeheer : Page
    {
        List<Material> materialList; 

        /// <summary>
        /// this gets fired when the page gets loaded.
        /// when its not a postback everything will be loaded from the database
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlPopUpLeenItem.Visible = false;
                pnlPopUpReserveerItem.Visible = false;
                LoadItems();
                RefreshAllItems();
                Session["materialList"] = materialList;
            }
            else
            {
                materialList = (List<Material>) Session["materialList"];
            }
        }

       
        /// <summary>
        /// Load all items and put them in materialList
        /// </summary>
        public void LoadItems()
        {
            List<string> barcodes = new List<string>();
            string query = "select distinct \"barcode\" from productexemplaar";
            List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));
            foreach (Dictionary<string, object> dic in output)
            {
                    barcodes.Add((string)dic["barcode"]);
            }

            materialList = GetMaterialsInRented(barcodes);
        }

        /// <summary>
        /// this method loads all materials from the database
        /// </summary>
        /// <param name="barcodes">a list with all barcodes for the materials that should be loaded</param>
        /// <returns>the list with all materials that excist in the database</returns>
        public List<Material> GetMaterialsInRented(List<string> barcodes)
        {
            List<Material> materials = new List<Material>();
            foreach (string barcode in barcodes)
            {
                string query = "select count(*) as VALUE from verhuur v, productexemplaar pe where \"productexemplaar_id\" = pe.id and \"barcode\" = '" + barcode + "'";
                List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));

                if ((int) (decimal)output[0]["VALUE"] == 1)
                {
                    // products that have only 1 record in verhuur
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
                    // products that have multiple records in verhuur
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
                    // products that have no record in verhuur
                    query = "select pe.ID, \"merk\", \"serie\", p.\"prijs\", \"barcode\" from product p, productexemplaar pe where p.ID = pe.\"product_id\" and \"barcode\" = '" + barcode + "'";
                    List<Dictionary<string, object>> outputMaterial = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));
                    Dictionary<string, object> dic = outputMaterial[0];
                        materials.Add(new Material((int)(long)dic["ID"], (string)dic["merk"], (string)dic["serie"], (int)(decimal)dic["prijs"], (string)dic["barcode"], new List<DateTime[]>()));
                }
                
            }


            return materials;
        }

        /// <summary>
        /// this method refreshes all materials that are shown on the screen
        /// </summary>
        public void RefreshAllItems()
        {
            lbProducts.Items.Clear();
            if (tbDateFrom.Text.Length == 0
                || tbDateTo.Text.Length == 0)
            {
                foreach (Material mat in materialList)
                {
                    mat.Status = Status.Undefined;
                    lbProducts.Items.Add(mat.ToString());
                }
            }
            else
            {
                foreach (Material mat in materialList)
                {
                    DateTime dateFrom = DateTime.ParseExact(tbDateFrom.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dateTo = DateTime.ParseExact(tbDateTo.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    mat.CheckStatus(dateFrom, dateTo);
                    lbProducts.Items.Add(mat.ToString());
                }
            }

            
        }

        /// <summary>
        /// this method checks if a material is available for the selected period
        /// </summary>
        /// <returns>wether the check has gone good or if the input was wrong</returns>
        private bool CheckMaterialStatus()
        {
            try
            {
                DateTime dateToCheck = DateTime.ParseExact(tbLeenTerugbrengDatum.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Material materialToCheck = (Material)Session["selectedMaterial"];
                materialToCheck.CheckStatus(DateTime.Now, dateToCheck);
                lblLeenItem.Text = "Name: " + materialToCheck.Brand + ", " + materialToCheck.Serie + "<br />price: " +
                                   materialToCheck.Price.ToString() + "<br/>status: " +
                                   materialToCheck.Status.ToString();
                Session["selectedMaterial"] = materialToCheck;
                return true;
            }
            catch
            {
                Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"An error has occured\")</SCRIPT>");
                return false;
            }
        }

        /// <summary>
        /// lend the inputted material to the user with the inputted barcode until the inputted return date
        /// </summary>
        /// <param name="mat">the Material that will be lend</param>
        /// <param name="barCode">the barcode to which user this material will be lend</param>
        /// <param name="dateReturn">the date when the item will be lend</param>
        /// <returns>wether the input was correct</returns>
        public bool LendItem(Material mat, string barCode, DateTime dateReturn)
        {
            if (barCode.Length == 12 && dateReturn > DateTime.Now)
            {
                string query = "select rp.ID from polsbandje p, reservering_polsbandje rp where p.id = rp.\"polsbandje_id\" and \"barcode\" = :barcode and \"actief\" = 1";
                OracleCommand oc = new OracleCommand(query);
                oc.Parameters.Add("barcode", barCode);
                List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(oc);
                int id = (int) (long) output[0]["ID"];

                return true;
            }
            else
            {
                // input is wrong
                return false;
            }
        }

        /// <summary>
        /// this method gathers all information about the material that should be lend
        /// then the lendItem() gets fired with the needed information
        /// </summary>
        private void LendMaterial()
        {
            Material selectedProduct = null;
            foreach (Material mat in materialList)
            {
                if (mat.ToString() == lbProducts.SelectedItem.Text)
                {
                    Session["selectedMaterial"] = mat;
                    selectedProduct = mat;
                    break;
                }
            }
            try
            {
                selectedProduct.CheckStatus(DateTime.Now, DateTime.Now.AddDays(1));
                lblLeenItem.Text = "Name: " + selectedProduct.Brand + ", " + selectedProduct.Serie + "<br />price: " +
                                   selectedProduct.Price.ToString() + "<br/>status: " +
                                   selectedProduct.Status.ToString();
                tbLeenTerugbrengDatum.Text = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy");
                pnlMain.Visible = false;
                pnlPopUpLeenItem.Visible = true;
            }
            catch
            {

            }
        }


        public void btnLeenUit_Click(object sender, EventArgs e)
        {
            LendMaterial();
        }

        public void btnLeenUitPopUp_Click(object sender, EventArgs e)
        {
            if (CheckMaterialStatus())
            {
                LendItem((Material)Session["selectedMaterial"], tbLeenRFID.Text, DateTime.Now.AddDays(1));   
                pnlMain.Visible = true;
                pnlPopUpLeenItem.Visible = false;
            }
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

        protected void btnRefresh_OnClick(object sender, EventArgs e)
        {
            RefreshAllItems();
        }

        protected void btnCheckStatus_OnClick(object sender, EventArgs e)
        {
            CheckMaterialStatus();
        }
    }
}