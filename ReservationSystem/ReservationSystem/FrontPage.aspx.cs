using System;

namespace ReservationSystem
{
public partial class Voorpagina : System.Web.UI.Page
{
    Database _db = new Database();
        
    protected void Page_Load(object sender, EventArgs e)
    {

    }
 
    protected void btnReserve_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateAccount.aspx");
    }
}
}