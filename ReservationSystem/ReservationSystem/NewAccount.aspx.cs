using System;
using System.Web.UI;
using System.Text.RegularExpressions;

namespace ReservationSystem
{
    public partial class NewAccount : Page
    {
        private readonly Database _db = new Database();
        private ActiveDirectory _ad = new ActiveDirectory();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Method for inserting a person/account into the database/AD
        /// </summary>
        /// <returns></returns>
        protected bool Insert_Person()
        {

            if (tbName.Text == "" || tbSurname.Text == "" || tbBankAccount.Text == "" || tbStreet.Text == "" ||
                tbHouseNumber.Text == "" || tbCity.Text == "" || tbUsername.Text == "" || tbEmail.Text == "" ||
                tbPassword.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage",
                    "alert('One of the required fields was not filled in.')", true);
                return false;
            }

            var name = tbName.Text;
            var addition = tbAddition.Text;
            var lastname = tbSurname.Text;
            var bank = tbBankAccount.Text;
            var street = tbStreet.Text;
            var housenr = Convert.ToInt32(tbHouseNumber.Text);
            var place = tbCity.Text;
            var uname = tbUsername.Text;
            var password = tbPassword.Text;
            var email = tbEmail.Text;

            if (_db.Check_Username(uname))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage",
                    "alert('That username already exists.')", true);
                return false;
            }
            //New user in AD, commented away because no AD on this laptop.
            //UserPrincipal user = _ad.CreateNewUser(uname, password, name + " " + addition, lastname);

            var p = new Person(name, addition, lastname, street, housenr, place, bank);
            var acc = new Account(uname, email);

            Session["UserData"] = p;
            Session["Acc"] = acc;
            //Session["ADacc"] = user;

            _db.Insert_Person(p);
            _db.Insert_Account(uname, email);
            return true;
        }

        /// <summary>
        /// Inserts user, sends user to NewReservation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmitReserve_Click(object sender, EventArgs e)
        {
            if (Insert_Person())
            {
                Response.Redirect("NewReservation.aspx");
            }
        }
    }
}