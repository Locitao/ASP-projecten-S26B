using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Reservering;

public partial class MaakReservering : System.Web.UI.Page
{
    readonly Database _db = new Database();
    private Person _p;
    private Account _acc;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Session["UserData"] == null)
        {
            Page home = HttpContext.Current.Handler as Page;
            if (home != null)
            {
                ScriptManager.RegisterStartupScript(home, home.GetType(), "err_msg", "alert('You need to log in first.');window.location='CreateAccount.aspx';", true);
            }
        }

        else
        {
            _p = (Person) Session["UserData"];
            _acc = (Account) Session["Acc"];
        }
          

        if (!IsPostBack)
        {
            Fill_Listbox();
        }
    }

    private void Fill_Listbox()
    {
        List<Location> locations = _db.Find_Locations();

        foreach (var x in locations)
        {
            lbLocations.Items.Add(x.ToString());
        }
    }
    protected void btnReserve_Click(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Gonna use this method to do everything in regards to inserting a reservation.
    /// </summary>
    /// <returns></returns>
    protected bool Create_Reservation()
    {
        int personId = _db.Person_Id(_p.Voornaam, _p.Achternaam, _p.Straat);
        DateTime now = DateTime.Now;
        DateTime end = new DateTime(2015, 7, 1);
        _db.Insert_Reservation(personId, now, end, 1);
        if (!_db.New_Wristband())
        {
            return false;
        }

        int accId = _db.Find_Acc(_acc.Username);
        if (!_db.Insert_Res_Band(accId))
        {
            return false;
        }

        return true;

    }
}