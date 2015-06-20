using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReservationSystem
{
    public partial class ReserveMaterials : System.Web.UI.Page
    {
        readonly Database _db = new Database();
        private Person _p;
        private List<Product> products = new List<Product>(); 
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["UserData"] == null)
            {
                Page home = HttpContext.Current.Handler as Page;
                if (home != null)
                {
                    ScriptManager.RegisterStartupScript(home, home.GetType(), "err_msg", "alert('You need to log in first.');window.location='NewAccount.aspx';", true);
                }
            }

            else
            {
                _p = (Person)Session["UserData"];
            }
            
            
            if (!IsPostBack)
            {
                Fill_List();
            }
        }

        protected void Fill_List()
        {
            products = _db.Find_Products();

            foreach (var x in products)
            {
                lbMaterials.Items.Add(x.ToString());
            }
        }

        protected bool Insert_Mat_Res(int productId, int price, DateTime start, DateTime end)
        {
            return _db.Insert_Mat_Res(productId, start, end, price);
        }
        
        /// <summary>
        /// On button click, inserts the given ID's into VERHUUR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            DateTime start = Convert.ToDateTime(tbStartDate);
            DateTime end = Convert.ToDateTime(tbEndDate);

            if (tbMatOne.Text != "")
            {
                int id = Convert.ToInt16(tbMatOne.Text);
                foreach (var x in products)
                {
                    if (x.Id == id)
                    {
                        Insert_Mat_Res(id, x.Price, start, end);
                    }
                }
            }

            if (tbMatTwo.Text != "")
            {
                int id = Convert.ToInt16(tbMatTwo.Text);
                foreach (var x in products)
                {
                    if (x.Id == id)
                    {
                        Insert_Mat_Res(id, x.Price, start, end);
                    }
                }
            }

            if (tbMatThree.Text != "")
            {
                int id = Convert.ToInt16(tbMatThree.Text);
                foreach (var x in products)
                {
                    if (x.Id == id)
                    {
                        Insert_Mat_Res(id, x.Price, start, end);
                    }
                }
            }

            Page home = HttpContext.Current.Handler as Page;
            if (home != null)
            {
                ScriptManager.RegisterStartupScript(home, home.GetType(), "err_msg", "alert('Thanks for registering. Tell your friends to create an account as well, otherwise they can't come to the event.');window.location='FrontPage.aspx';", true);
            }

        }
    }
}