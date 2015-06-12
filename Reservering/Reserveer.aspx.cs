using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Reservering;

public partial class Reserveer : System.Web.UI.Page
{
    Insert insert = new Insert();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Insert_Person()
    {
        if (tbVoornaam.Text == "" || tbAchternaam.Text == "" || tbBankrekening.Text == "" || tbStraat.Text == "" || tbHuisnummer.Text == "" || tbWoonplaats.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", "alert('One of the required fields was not filled in.')", true);
        }

        else
        {
            string surname = tbVoornaam.Text;
            string addition = tbTussenvoegsel.Text;
            string lastname = tbAchternaam.Text;
            int bank = Convert.ToInt32(tbBankrekening.Text);
            string street = tbStraat.Text;
            int housenr = Convert.ToInt32(tbHuisnummer.Text);
            string place = tbWoonplaats.Text;

            Person p = new Person(surname, addition, lastname, street, housenr, place, bank);

            Session["UserData"] = p;

            insert.Insert_Persoon(p);

        }
    }
    protected void btnSubmitReserve_Click(object sender, EventArgs e)
    {
        Insert_Person();
    }
}