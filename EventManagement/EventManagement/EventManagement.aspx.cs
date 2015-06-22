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
                RefreshCampingSpotsListBox(1);
                RefreshEventsListBox(1);
            }
        }



        private void RefreshCampingsListBox()
        {
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
            if (lbCampings.Items.Count > 0)
            {
                lbCampings.SelectedIndex = 0;
            }
        }

        private void RefreshCampingSpotsListBox(int campingId)
        {
            lbCampingSpots.Items.Clear();
            string query = "select * from plek where \"locatie_id\" = :locId";
            OracleCommand oc = new OracleCommand(query);
            oc.Parameters.Add("locId", campingId);
            List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(oc);
            foreach (Dictionary<string, object> dic in output)
            {
                string number;
                if (dic["nummer"] != DBNull.Value)
                {
                    number = (string)dic["nummer"];
                }
                else
                {
                    number = "-";
                }
                string capacity;
                if (dic["capaciteit"] != DBNull.Value)
                {
                    capacity = Convert.ToString((long)dic["capaciteit"]);
                }
                else
                {
                    capacity = "-";
                }
                lbCampingSpots.Items.Add(Convert.ToString((long)dic["ID"]) + ": number: " + number + ", capacity:" + capacity);
            }
            if (lbCampingSpots.Items.Count > 0)
            {
                lbCampingSpots.SelectedIndex = 0;
            }
        }

        private void RefreshEventsListBox(int campingId)
        {
            lbEvents.Items.Clear();
            string query = "select * from event where \"locatie_id\" = :locId";
            OracleCommand oc = new OracleCommand(query);
            oc.Parameters.Add("locId", campingId);
            List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(oc);
            foreach (Dictionary<string, object> dic in output)
            {
                string capacity;
                if (dic["maxBezoekers"] != DBNull.Value)
                {
                    capacity = Convert.ToString((long)dic["maxBezoekers"]);
                }
                else
                {
                    capacity = "-";
                }
                lbEvents.Items.Add(Convert.ToString((long)dic["ID"]) + ": " + (string)dic["naam"] + ", " + Convert.ToString((DateTime)dic["datumstart"]) + ", " + Convert.ToString((DateTime)dic["datumEinde"]) + ", Capacity:" + capacity);
            }
            if (lbEvents.Items.Count > 0)
            {
                lbEvents.SelectedIndex = 0;
            }
        }

        private bool AddCamping(string name, string street, int number, string postcode, string city)
        {
            try
            {
                //string query = "insert into locatie values (null, :name, :street, :number, :postcode, :city)";
                string query = "insert into locatie values (10, 'naae', 'street', '3', '5571AB', 'Boergaaik')";
                OracleCommand oc = new OracleCommand(query);
                //oc.Parameters.Add("name", name);
                //oc.Parameters.Add("street", street);
               // oc.Parameters.Add("number", number);
                //oc.Parameters.Add("postcode", postcode);
                //oc.Parameters.Add("city", city);
                DbConnection.Instance.Execute(oc);
                RefreshCampingsListBox();
                return true;
            }
            catch
            {
                return false;
            }
            
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
            AddCamping("naam", "street", 3, "5571AB", "Boergaaik");
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