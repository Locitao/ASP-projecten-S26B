using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Reservering;


public partial class Voorpagina : System.Web.UI.Page
{
    Database _db = new Database();
        
    protected void Page_Load(object sender, EventArgs e)
    {

    }
 
    protected void btnReserve_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateAccount.aspx");
        //testlabel.Text = Convert.ToString(_db.Max_Polsbandje());
        /*
        var data = select.Select_Test_Personen();

        foreach (Dictionary<string, object> s in data)
        {
            testLabel.Text = testLabel.Text + " " + Convert.ToString(s["locatie_id"]);
        }
         */
    }
}