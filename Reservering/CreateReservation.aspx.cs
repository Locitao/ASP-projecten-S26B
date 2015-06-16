using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MaakReservering : System.Web.UI.Page
{
    private Person _p;
    private Account _acc;
    protected void Page_Load(object sender, EventArgs e)
    {
        /*
        if (Session["UserData"] == null)
        {
            Page home = HttpContext.Current.Handler as Page;
            if (home != null)
            {
                ScriptManager.RegisterStartupScript(home, home.GetType(), "err_msg", "alert('You need to log in first.');window.location='Signup.aspx';", true);
            }
        }

        else
        {
            _p = (Person) Session["UserData"];
            _acc = (Account) Session["Acc"];
        }
          */
    }
}