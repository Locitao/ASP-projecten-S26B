using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
                pnlPopUpAddProduct.Visible = false;
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
            string query = "select distinct \"barcode\" from productexemplaar order by \"barcode\" asc";
            List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(new OracleCommand(query));
            foreach (Dictionary<string, object> dic in output)
            {
                barcodes.Add((string) dic["barcode"]);
            }

            _materialList = GetMaterialsFromDatabase(barcodes);
        }

        /// <summary>
        ///     this method loads all materials from the database
        /// </summary>
        /// <param name="barcodes">a list with all barcodes for the materials that should be loaded</param>
        /// <returns>the list with all materials that excist in the database</returns>
        public List<Material> GetMaterialsFromDatabase(List<string> barcodes)
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
        /// <param name="dateReturn">the date when the item will be retured</param>
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
                if (output.Count == 0) return false;
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
        /// reserve material to an user
        /// </summary>
        /// <param name="mat">the Material that will be reserved</param>
        /// <param name="barCode">the barcode which represents the user who wants this product</param>
        /// <param name="dateStart">the starting date from when the product will be reserved</param>
        /// <param name="dateReturn">the date this product will be returned</param>
        /// <returns>wether the input was correct</returns>
        public bool ReserveItem(Material mat, string barCode, DateTime dateStart, DateTime dateReturn)
        {
            if (barCode.Length == 12 && dateReturn > dateStart)
            {
                string query =
                    "select rp.ID from polsbandje p, reservering_polsbandje rp where p.id = rp.\"polsbandje_id\" and \"barcode\" = :barcode and \"actief\" = 1";
                OracleCommand oc = new OracleCommand(query);
                oc.Parameters.Add("barcode", barCode);
                List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(oc);
                if (output.Count == 0) return false;
                int id = (int)(long)output[0]["ID"];


                query =
                    "insert into verhuur values(verhuur_fcseq.nextval, :productID, :rpID, :dateIn, :dateOut, :price, '0')";
                OracleCommand ocInsert = new OracleCommand(query);
                ocInsert.Parameters.Add("productID", mat.Id);
                ocInsert.Parameters.Add("rpID", id);
                ocInsert.Parameters.Add("dateIn", dateStart);
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

        /// <summary>
        /// Loads all product species into the drop down list
        /// </summary>
        private void LoadProductsIntoComboBox()
        {
            if (!IsPostBack)
            {
                string query = "select id, \"merk\", \"serie\", \"typenummer\" from product";
                OracleCommand oc = new OracleCommand(query);
                List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(oc);
                foreach (Dictionary<string, object> dic in output)
                {
                    string productSpecies = Convert.ToString((long)dic["ID"]) + ": " + (string)dic["merk"] + ", " + (string)dic["serie"] + ", " + (string)dic["typenummer"];
                    ddlProducts.Items.Add(productSpecies);
                }
            }
        }

        /// <summary>
        /// Add another instance off the selected product to the database
        /// </summary>
        private void SaveAddedProduct()
        {
            try
            {
                string selectedProduct = ddlProducts.SelectedItem.ToString();
                string productId = selectedProduct.Substring(0, selectedProduct.IndexOf(':'));
                string query = "select max(\"volgnummer\") as value from productexemplaar where \"product_id\" = :productId";
                OracleCommand oc = new OracleCommand(query);
                oc.Parameters.Add("productId", productId);
                List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(oc);
                string followingNumber = Convert.ToString((decimal)output[0]["VALUE"] + 1);
                string followingNumberPadded = followingNumber.PadLeft(3, '0');
                string typeNumber = selectedProduct.Substring(selectedProduct.Length - 4, 4);
                string barcode = typeNumber + "." + followingNumberPadded;
                query = "insert into productexemplaar values (productexemplaar_fcseq.nextval, :id, :followingNumber, :barcode)";
                OracleCommand cmd = new OracleCommand(query);
                cmd.Parameters.Add("id", productId);
                cmd.Parameters.Add("followingNumber", followingNumber);
                cmd.Parameters.Add("barcode", barcode);
                DbConnection.Instance.Execute(cmd);
            }
            catch
            {
                Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"An error has occured, the item has not been added. Please try again\")</SCRIPT>");
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
                if (LendItem((Material) Session["selectedMaterial"], tbLendBarcode.Text,
                    DateTime.ParseExact(tbLendReturnDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                {
                    Server.Transfer("MaterialRenting.aspx");
                }
                else
                {
                    Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"Input was not correct, please put in correct information\")</SCRIPT>");
                }
            }
        }

        

        protected void btnReserveProduct_OnClick(object sender, EventArgs e)
        {
            ReserveMaterial();
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
            if (CheckReserveMaterialStatus())
            {
                if (ReserveItem((Material)Session["selectedMaterial"], tbReserveBarcode.Text, DateTime.ParseExact(tbReserveLendDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact(tbReserveReturnDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                {
                    Server.Transfer("MaterialRenting.aspx");
                }
                else
                {
                    Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"Input was not correct, please put in correct information\")</SCRIPT>");
                }
            }
        }

        protected void btnReserveCancel_OnClick(object sender, EventArgs e)
        {
            Server.Transfer("MaterialRenting.aspx");
        }

        protected void btnAddProduct_OnClick(object sender, EventArgs e)
        {
            SaveAddedProduct();
            Server.Transfer("MaterialRenting.aspx");
        }

        protected void ddlProducts_OnLoad(object sender, EventArgs e)
        {
            LoadProductsIntoComboBox();
        }

        protected void btnNewItem_OnClick(object sender, EventArgs e)
        {
            pnlMain.Visible = false;
            pnlPopUpAddProduct.Visible = true;
        }
    }
}