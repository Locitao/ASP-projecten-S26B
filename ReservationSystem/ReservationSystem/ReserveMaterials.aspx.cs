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
        private Account _acc;
        private Person _p;
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
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
                _acc = (Account)Session["Acc"];
            }
            */

            if (!IsPostBack)
            {
                Fill_List();
            }
        }

        protected void Fill_List()
        {
            List<Product> products = _db.Find_Products();

            foreach (var x in products)
            {
                lbMaterials.Items.Add(x.ToString());
            }
        }
    }
}