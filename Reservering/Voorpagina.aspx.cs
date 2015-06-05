using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Reservering;

public partial class Voorpagina : System.Web.UI.Page
{
    Connection conn = new Connection();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void TestButton_Click(object sender, EventArgs e)
    {
        
    }
    protected void btnReserve_Click(object sender, EventArgs e)
    {
        Response.Redirect("Reserveer.aspx");
    }
}