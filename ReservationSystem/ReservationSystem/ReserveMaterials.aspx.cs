﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;


namespace ReservationSystem
{
    public partial class ReserveMaterials : Page
    {
        private readonly Database _db = new Database();
        private Person _p;
        private List<Product> _products = new List<Product>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserData"] == null)
            {
                var home = HttpContext.Current.Handler as Page;
                if (home != null)
                {
                    ScriptManager.RegisterStartupScript(home, home.GetType(), "err_msg",
                        "alert('You need to log in first.');window.location='NewAccount.aspx';", true);
                }
            }

            else
            {
                _p = (Person) Session["UserData"];
            }


            if (!IsPostBack)
            {
                Fill_List();
            }
        }

        /// <summary>
        /// Fills the listbox with materials that can still be reserved.
        /// </summary>
        protected void Fill_List()
        {
            _products = _db.Find_Products();
            Session["Products"] = _products;

            foreach (var x in _products)
            {
                lbMaterials.Items.Add(x.ToString());
            }
        }

        protected bool Insert_Mat_Res(int productId, int price, DateTime start, DateTime end, int paid)
        {
            return _db.Insert_Mat_Res(productId, start, end, price, paid);
        }

        /// <summary>
        ///     On button click, inserts the given ID's into VERHUUR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            int errorCounter = 0;
            errorCounter = Regex.Matches(tbStartDate.Text, @"[a-zA-Z]").Count;
            errorCounter = errorCounter + Regex.Matches(tbEndDate.Text, @"[a-zA-Z]").Count;

            var product = (List<Product>) Session["Products"];
            var paid = 0;

            if (cbPay.Checked)
            {
                paid = 1;
            }

            if (tbMatOne.Text != "" && errorCounter == 0)
            {
                var start = Convert.ToDateTime(tbStartDate.Text);
                var end = Convert.ToDateTime(tbEndDate.Text);

                int id = Convert.ToInt16(tbMatOne.Text);
                foreach (var x in product)
                {
                    if (x.Id == id)
                    {
                        Insert_Mat_Res(x.Id, x.Price, start, end, paid);
                    }
                }
            }

            else if (tbMatTwo.Text != "" && errorCounter == 0)
            {
                var start = Convert.ToDateTime(tbStartDate.Text);
                var end = Convert.ToDateTime(tbEndDate.Text);

                int id = Convert.ToInt16(tbMatTwo.Text);
                foreach (var x in product)
                {
                    if (x.Id == id)
                    {
                        Insert_Mat_Res(x.Id, x.Price, start, end, paid);
                    }
                }
            }

            else if (tbMatThree.Text != "" && errorCounter == 0)
            {
                var start = Convert.ToDateTime(tbStartDate.Text);
                var end = Convert.ToDateTime(tbEndDate.Text);

                int id = Convert.ToInt16(tbMatThree.Text);
                foreach (var x in product)
                {
                    if (x.Id == id)
                    {
                        Insert_Mat_Res(x.Id, x.Price, start, end, paid);
                    }
                }
            }

            else if (tbMatOne.Text != "" || tbMatTwo.Text != "" || tbMatThree.Text != "" && errorCounter > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage",
                    "alert('One of the given dates is wrong.')", true);
            }

            var home = HttpContext.Current.Handler as Page;
            if (home != null)
            {
                ScriptManager.RegisterStartupScript(home, home.GetType(), "err_msg",
                    "alert('Thanks for registering. Tell your friends to create an account as well, otherwise they cannot come to the event.');window.location='FrontPage.aspx';",
                    true);
            }
        }
    }
}