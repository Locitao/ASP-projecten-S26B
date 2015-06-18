using System;
using System.Web.UI;
using Reservering;


public partial class MaakAccount : System.Web.UI.Page
{
    readonly Database _db = new Database();
    ActiveDirectory _ad = new ActiveDirectory();
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Method for inserting a person/account into the database/AD
    /// </summary>
    /// <returns></returns>
    protected bool Insert_Person()
    {
        if (tbName.Text == "" || tbSurname.Text == "" || tbBankAccount.Text == "" || tbStreet.Text == "" || tbHouseNumber.Text == "" || tbCity.Text == "" || tbUsername.Text == "" || tbEmail.Text == "" || tbPassword.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", "alert('One of the required fields was not filled in.')", true);
            return false;
        }

        string name = tbName.Text;
        string addition = tbAddition.Text;
        string lastname = tbSurname.Text;
        int bank = Convert.ToInt32(tbBankAccount.Text);
        string street = tbStreet.Text;
        int housenr = Convert.ToInt32(tbHouseNumber.Text);
        string place = tbCity.Text;
        string uname = tbUsername.Text;
        string password = tbPassword.Text;
        string email = tbEmail.Text;

        if (_db.Check_Username(uname))
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", "alert('That username already exists.')", true);
            return false;
        }
        //New user in AD, commented away because no AD on this laptop.
        //UserPrincipal user = _ad.CreateNewUser(uname, password, name + " " + addition, lastname);

        Person p = new Person(name, addition, lastname, street, housenr, place, bank);
        Account acc = new Account(uname, email);

        Session["UserData"] = p;
        Session["Acc"] = acc;
        //Session["ADacc"] = user;

        _db.Insert_Person(p);
        _db.Insert_Account(uname, email);
        return true;
    }
    protected void btnSubmitReserve_Click(object sender, EventArgs e)
    {
        if (Insert_Person())
        {
            Response.Redirect("CreateReservation.aspx");
        }
        
    }
}