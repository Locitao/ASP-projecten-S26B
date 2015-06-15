using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Reservering;


public partial class MaakAccount : System.Web.UI.Page
{
    Database _db = new Database();
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected bool Insert_Person()
    {
        if (tbVoornaam.Text == "" || tbAchternaam.Text == "" || tbBankrekening.Text == "" || tbStraat.Text == "" || tbHuisnummer.Text == "" || tbWoonplaats.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", "alert('One of the required fields was not filled in.')", true);
            return false;
        }

        string surname = tbVoornaam.Text;
        string addition = tbTussenvoegsel.Text;
        string lastname = tbAchternaam.Text;
        int bank = Convert.ToInt32(tbBankrekening.Text);
        string street = tbStraat.Text;
        int housenr = Convert.ToInt32(tbHuisnummer.Text);
        string place = tbWoonplaats.Text;

        Person p = new Person(surname, addition, lastname, street, housenr, place, bank);

        Session["UserData"] = p;

        _db.Insert_Person(p);
        return true;
    }
    protected void btnSubmitReserve_Click(object sender, EventArgs e)
    {
        if (Insert_Person());
        {
            Response.Redirect("CreateReservation.aspx");
        }
        
    }
}