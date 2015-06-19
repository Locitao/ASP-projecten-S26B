using System;
using System.Web;
using System.Web.UI;

namespace ReservationSystem
{
    public partial class NewReservation : Page
    {
        /// <summary>
        /// Fields.
        /// </summary>
        private readonly Database _db = new Database();
        private Account _acc;
        private Person _p;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserData"] == null)
            {
                var home = HttpContext.Current.Handler as Page;
                if (home != null)
                {
                    ScriptManager.RegisterStartupScript(home, home.GetType(), "err_msg",
                        "alert('You need to log in first.');window.location='NewAccount.aspx';", true);
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

        /// <summary>
        /// Fills the listbox with possible locations which are still free on the camping.
        /// </summary>
        private void Fill_Listbox()
        {
            var locations = _db.Find_Locations();

            foreach (var x in locations)
            {
                lbLocations.Items.Add(x.ToString());
            }
        }

        /// <summary>
        /// Tries to create a reservation then sends the user to the page to reserve materials.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReserve_Click(object sender, EventArgs e)
        {
            if (Create_Reservation())
            {
                Response.Redirect("ReserveMaterials.aspx");
            }
        }

        /// <summary>
        ///     Gonna use this method to do everything in regards to inserting a reservation.
        /// </summary>
        /// <returns></returns>
        protected bool Create_Reservation()
        {
            if (tbPeople.Text == "") return false;
            if (tbLocation.Text == "") return false;

            var personId = _db.Person_Id(_p.Voornaam, _p.Achternaam, _p.Straat);
            var now = DateTime.Now;
            var end = new DateTime(2015, 7, 1);
            _db.Insert_Reservation(personId, now, end, 1);

            var accId = _db.Find_Acc(_acc.Username);
            var resId = _db.Max_Res();
            Session["resid"] = resId;
            _db.Insert_Res_Spot(resId, Convert.ToInt32(tbLocation.Text));
            return _db.Insert_Res_Band(accId);
        }
    }
}