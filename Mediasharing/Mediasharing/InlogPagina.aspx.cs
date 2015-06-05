using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mediasharing
{
    public partial class InlogPagina : System.Web.UI.Page
    {
        //Events
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            VeranderInlogData();
        }

        //Methods
        public void VeranderInlogData()
        {
            if (rbGebruiker.SelectedValue == "Gebruiker")
            {
                tbWachtwoord.Visible = false;
                lblWachtwoord.Visible = false;
                lblGebruikersnaamOrRfid.Text = "RFIDCode:";
            }
            else if (rbGebruiker.SelectedValue == "Administrator")
            {
                tbWachtwoord.Visible = true;
                lblWachtwoord.Visible = true;
                lblGebruikersnaamOrRfid.Text = "Gebruikersnaam:";
            }

        }
    }
}