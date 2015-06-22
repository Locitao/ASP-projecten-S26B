using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialRenting;
using Oracle.DataAccess.Client;

namespace EventManagement
{
    public partial class EventManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefreshCampingsListBox();
                RefreshCampingSpotsListBox();
                RefreshEventsListBox();
            }
        }



        private void RefreshCampingsListBox()
        {
            //TODO: load campings from database and put them in the listbox
            lbCampings.Items.Clear();
            string query = "select * from locatie";
            OracleCommand oc = new OracleCommand(query);
            List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(oc);
            foreach (Dictionary<string, object> dic in output)
            {
                string postcode;
                if (dic["postcode"] != DBNull.Value)
                {
                    postcode = (string) dic["postcode"];
                }
                else
                {
                    postcode = "Postcode";
                }
                string city;
                if (dic["postcode"] != DBNull.Value)
                {
                    city = (string)dic["postcode"];
                }
                else
                {
                    city = "City";
                }

                lbCampings.Items.Add(Convert.ToString((long)dic["ID"]) + ": " + (string)dic["naam"] + ", " + postcode + ", " + city);
            }
        }

        private void RefreshCampingSpotsListBox()
        {
            //TODO: load campingspots from database and put them in the listbox
        }

        private void RefreshEventsListBox()
        {
            //TODO: load events from database and put them in the listbox
        }

        private void AddCamping()
        {
            //TODO: add camping to database
        }

        private void AddCampingSpot()
        {
            //TODO: add campingspots to database
        }

        private void AddEvent()
        {
            //TODO: add event to database
        }

        protected void btnAddCamping_OnClick(object sender, EventArgs e)
        {
            AddCamping();
        }

        protected void btnAddCampingSpot_OnClick(object sender, EventArgs e)
        {
            AddCampingSpot();
        }

        protected void btnAddEvent_OnClick(object sender, EventArgs e)
        {
            AddEvent();
        }
    }
}