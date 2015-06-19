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
        Database _db = new Database();
        protected void Page_Load(object sender, EventArgs e)
        {

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