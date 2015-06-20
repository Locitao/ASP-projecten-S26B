using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReservationSystem
{
    public partial class OnlyAcc : System.Web.UI.Page
    {
        readonly Database _db = new Database();
        ActiveDirectory _ad = new ActiveDirectory();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmitReserve_Click(object sender, EventArgs e)
        {
            if (Insert_Person())
            {
                Page home = HttpContext.Current.Handler as Page;
                if (home != null)
                {
                    ScriptManager.RegisterStartupScript(home, home.GetType(), "err_msg", "alert('Thanks for registering!');window.location='FrontPage.aspx';", true);
                }
            }
        }

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
            string bank = tbBankAccount.Text;
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
    }
}