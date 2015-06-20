using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using Oracle.DataAccess.Client;

namespace MaterialRenting
{
    public partial class MaterialRenting : Page
    {
        private List<Material> _materialList;

        /// <summary>
        ///     this gets fired when the page gets loaded.
        ///     when its not a postback everything will be loaded from the database
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlPopUpLendItem.Visible = false;
                pnlPopUpReserveItem.Visible = false;
                LoadItems();
                RefreshAllItems();
                Session["materialList"] = _materialList;
            }
            else
            {
                _materialList = (List<Material>) Session["materialList"];
            }
        }

        /// <summary>
        ///     Load all items and put them in materialList
        /// </summary>
        public void LoadItems()
        {
            List<string> barcodes = new List<string>();
            string query = "select distinct \"barcode\" from productexemplaar";
            List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));
            foreach (Dictionary<string, object> dic in output)
            {
                barcodes.Add((string) dic["barcode"]);
            }

            _materialList = GetMaterialsInRented(barcodes);
        }

        /// <summary>
        ///     this method loads all materials from the database
        /// </summary>
        /// <param name="barcodes">a list with all barcodes for the materials that should be loaded</param>
        /// <returns>the list with all materials that excist in the database</returns>
        public List<Material> GetMaterialsInRented(List<string> barcodes)
        {
            List<Material> materials = new List<Material>();
            foreach (string barcode in barcodes)
            {
                string query =
                    "select count(*) as VALUE from verhuur v, productexemplaar pe where \"productexemplaar_id\" = pe.id and \"barcode\" = '" +
                    barcode + "'";
                List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));

                if ((int) (decimal) output[0]["VALUE"] == 1)
                {
                    // products that have only 1 record in verhuur
                    query =
                        "select pe.ID, \"merk\", \"serie\", p.\"prijs\", \"barcode\", \"datumIn\", \"datumUit\", \"betaald\" from product p, productexemplaar pe, verhuur v where p.ID = pe.\"product_id\" and v.\"productexemplaar_id\" = pe.ID and \"barcode\" = '" +
                        barcode + "'";
                    List<Dictionary<string, object>> outputMaterials =
                        DbConnection.Instance.ExecuteQuery(new OracleCommand(query));
                    List<DateTime[]> datesList = new List<DateTime[]>();
                    DateTime[] dates = new DateTime[2];
                    dates[0] = (DateTime) outputMaterials[0]["datumIn"];
                    dates[1] = (DateTime) outputMaterials[0]["datumUit"];
                    datesList.Add(dates);
                    materials.Add(new Material((int) (long) outputMaterials[0]["ID"],
                        (string) outputMaterials[0]["merk"], (string) outputMaterials[0]["serie"],
                        (int) (decimal) outputMaterials[0]["prijs"], (string) outputMaterials[0]["barcode"], datesList));
                }
                if ((int) (decimal) output[0]["VALUE"] > 1)
                {
                    // products that have multiple records in verhuur
                    query =
                        "select pe.ID, \"merk\", \"serie\", p.\"prijs\", \"barcode\", \"datumIn\", \"datumUit\", \"betaald\" from product p, productexemplaar pe, verhuur v where p.ID = pe.\"product_id\" and v.\"productexemplaar_id\" = pe.ID and \"barcode\" = '" +
                        barcode + "'";
                    List<Dictionary<string, object>> outputMaterials =
                        DbConnection.Instance.ExecuteQuery(new OracleCommand(query));
                    Material mat = null;
                    foreach (Dictionary<string, object> dic in outputMaterials)
                    {
                        if (mat == null)
                            mat = new Material((int) (long) dic["ID"], (string) dic["merk"], (string) dic["serie"],
                                (int) (decimal) dic["prijs"], (string) dic["barcode"], new List<DateTime[]>());
                        mat.AddDates((DateTime) dic["datumIn"], (DateTime) dic["datumUit"]);
                    }
                    materials.Add(mat);
                }
                if ((int) (decimal) output[0]["VALUE"] == 0)
                {
                    // products that have no record in verhuur
                    query =
                        "select pe.ID, \"merk\", \"serie\", p.\"prijs\", \"barcode\" from product p, productexemplaar pe where p.ID = pe.\"product_id\" and \"barcode\" = '" +
                        barcode + "'";
                    List<Dictionary<string, object>> outputMaterial =
                        DbConnection.Instance.ExecuteQuery(new OracleCommand(query));
                    Dictionary<string, object> dic = outputMaterial[0];
                    materials.Add(new Material((int) (long) dic["ID"], (string) dic["merk"], (string) dic["serie"],
                        (int) (decimal) dic["prijs"], (string) dic["barcode"], new List<DateTime[]>()));
                }
            }


            return materials;
        }

        /// <summary>
        ///     this method refreshes all materials that are shown on the screen
        /// </summary>
        public void RefreshAllItems()
        {
            lbProducts.Items.Clear();
            if (tbStartDate.Text.Length == 0
                || tbEndDate.Text.Length == 0)
            {
                foreach (Material mat in _materialList)
                {
                    mat.Status = Status.Undefined;
                    lbProducts.Items.Add(mat.ToString());
                }
            }
            else
            {
                foreach (Material mat in _materialList)
                {
                    try
                    {
                        DateTime dateFrom = DateTime.ParseExact(tbStartDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime dateTo = DateTime.ParseExact(tbEndDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        mat.CheckStatus(dateFrom, dateTo);
                        lbProducts.Items.Add(mat.ToString());
                    }
                    catch
                    {
                        Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"An error has occured, please try again later\")</SCRIPT>");
                    }
                }
            }
        }

        /// <summary>
        ///     this method checks if a material is available for the selected period
        /// </summary>
        /// <returns>wether the check has gone good or if the input was wrong</returns>
        private bool CheckLendMaterialStatus()
        {
            try
            {
                DateTime dateToCheck = DateTime.ParseExact(tbLendReturnDate.Text, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture);
                Material materialToCheck = (Material) Session["selectedMaterial"];
                materialToCheck.CheckStatus(DateTime.Now, dateToCheck);
                lblLendItem.Text = "Name: " + materialToCheck.Brand + ", " + materialToCheck.Serie + "<br />price: " +
                                   materialToCheck.Price + "<br/>status: " +
                                   materialToCheck.Status;
                Session["selectedMaterial"] = materialToCheck;
                return true;
            }
            catch
            {
                Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"An error has occured, please try again later\")</SCRIPT>");
                return false;
            }
        }

        /// <summary>
        ///     this method checks if a material is available for the selected period
        /// </summary>
        /// <returns>wether the check has gone good or if the input was wrong</returns>
        private bool CheckReserveMaterialStatus()
        {
            try
            {
                DateTime dateFrom = DateTime.ParseExact(tbReserveLendDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime dateTo = DateTime.ParseExact(tbReserveReturnDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Material materialToCheck = (Material)Session["selectedMaterial"];
                materialToCheck.CheckStatus(dateFrom, dateTo);
                lblPopUpReserveItem.Text = "Name: " + materialToCheck.Brand + ", " + materialToCheck.Serie + "<br />price: " +
                                   materialToCheck.Price + "<br/>status: " +
                                   materialToCheck.Status;
                Session["selectedMaterial"] = materialToCheck;
                return true;
            }
            catch
            {
                Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"An error has occured, please try again later\")</SCRIPT>");
                return false;
            }
        }

        /// <summary>
        ///     lend the inputted material to the user with the inputted barcode until the inputted return date
        /// </summary>
        /// <param name="mat">the Material that will be lend</param>
        /// <param name="barCode">the barcode to which user this material will be lend</param>
        /// <param name="dateReturn">the date when the item will be lend</param>
        /// <returns>wether the input was correct</returns>
        public bool LendItem(Material mat, string barCode, DateTime dateReturn)
        {
            if (barCode.Length == 12 && dateReturn > DateTime.Now)
            {
                string query =
                    "select rp.ID from polsbandje p, reservering_polsbandje rp where p.id = rp.\"polsbandje_id\" and \"barcode\" = :barcode and \"actief\" = 1";
                OracleCommand oc = new OracleCommand(query);
                oc.Parameters.Add("barcode", barCode);
                List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(oc);
                int id = (int) (long) output[0]["ID"];


                query =
                    "insert into verhuur values(verhuur_fcseq.nextval, :productID, :rpID, :dateIn, :dateOut, :price, '1')";
                OracleCommand ocInsert = new OracleCommand(query);
                ocInsert.Parameters.Add("productID", mat.Id);
                ocInsert.Parameters.Add("rpID", id);
                ocInsert.Parameters.Add("dateIn", DateTime.Now);
                ocInsert.Parameters.Add("dateOut", dateReturn);
                ocInsert.Parameters.Add("price", mat.Price);
                DbConnection.Instance.Execute(ocInsert);

                return true;
            }
            else
            {
                // input is wrong
                return false;
            }
        }

        /// <summary>
        ///     this method gathers all information about the material that should be lend
        /// </summary>
        private void LendMaterial()
        {
            Material selectedProduct = null;
            foreach (Material mat in _materialList)
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
                lblLendItem.Text = "Name: " + selectedProduct.Brand + ", " + selectedProduct.Serie + "<br />price: " +
                                   selectedProduct.Price + "<br/>status: " +
                                   selectedProduct.Status;
                tbLendReturnDate.Text = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy");
                pnlMain.Visible = false;
                pnlPopUpLendItem.Visible = true;
            }
            catch
            {
                Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"An error has occured, please try again later\")</SCRIPT>");
            }
        }

        /// <summary>
        ///     this method gathers all information about the material that should be reserved
        /// </summary>
        private void ReserveMaterial()
        {
            Material selectedProduct = null;
            foreach (Material mat in _materialList)
            {
                if (lbProducts.SelectedItem != null && mat.ToString() == lbProducts.SelectedItem.Text)
                {
                    Session["selectedMaterial"] = mat;
                    selectedProduct = mat;
                    break;
                }
            }
            try
            {
                selectedProduct.CheckStatus(DateTime.Now, DateTime.Now.AddDays(1));
                lblPopUpReserveItem.Text = "Name: " + selectedProduct.Brand + ", " + selectedProduct.Serie + "<br />price: " +
                                   selectedProduct.Price + "<br/>status: " +
                                   selectedProduct.Status;
                tbReserveLendDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                tbReserveReturnDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                pnlMain.Visible = false;
                pnlPopUpReserveItem.Visible = true;
            }
            catch
            {
                Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"An error has occured, please try again later\")</SCRIPT>");
            }
        }

        public void btnLendProduct_Click(object sender, EventArgs e)
        {
            LendMaterial();
        }

        public void btnLendPopUpSave_OnClick(object sender, EventArgs e)
        {
            if (CheckLendMaterialStatus())
            {
                LendItem((Material) Session["selectedMaterial"], tbLendBarcode.Text, DateTime.Now.AddDays(1));
                Server.Transfer("MaterialRenting.aspx");
            }
        }

        

        protected void btnReserveProduct_OnClick(object sender, EventArgs e)
        {
            ReserveMaterial();
        }

        protected void btnReturnProduct_OnClick(object sender, EventArgs e)
        {
        }

        protected void btnRefresh_OnClick(object sender, EventArgs e)
        {
            RefreshAllItems();
        }

        protected void btnCheckStatus_OnClick(object sender, EventArgs e)
        {
            CheckLendMaterialStatus();
        }

        protected void btnLendCancel_OnClick(object sender, EventArgs e)
        {
            Server.Transfer("MaterialRenting.aspx");
        }

        protected void btnReserveCheckStatus_OnClick(object sender, EventArgs e)
        {
            CheckReserveMaterialStatus();
        }

        protected void btnReserveSave_OnClick(object sender, EventArgs e)
        {
            //TODO: 
        }

        protected void btnReserveCancel_OnClick(object sender, EventArgs e)
        {
            Server.Transfer("MaterialRenting.aspx");
        }
    }
}