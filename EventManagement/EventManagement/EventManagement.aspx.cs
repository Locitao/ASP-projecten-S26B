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
                if (dic["plaats"] != DBNull.Value)
                {
                    city = (string)dic["plaats"];
                }
                else
                {
                    city = "City";
                }

                lbCampings.Items.Add(Convert.ToString((long)dic["ID"]) + ": " + (string)dic["naam"] + ", " + postcode + ", " + city);
                ddlEvents.Items.Add(Convert.ToString((long)dic["ID"]) + ": " + (string)dic["naam"] + ", " + postcode + ", " + city);
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
                DateTime dateStart = (DateTime) dic["datumstart"];
                DateTime dateEnd = (DateTime) dic["datumEinde"];
                lbEvents.Items.Add(Convert.ToString((long)dic["ID"]) + ": " + (string)dic["naam"] + ", " + dateStart.ToString("dd-MM-yyyy") + ", " + dateEnd.ToString("dd-MM-yyyy") + ", Cap:" + capacity);
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
                string query = "insert into locatie values (null, :name, :street, :numberr, :postcode, :city)";
                OracleCommand oc = new OracleCommand(query);
                oc.Parameters.Add("name", name);
                oc.Parameters.Add("street", street);
                oc.Parameters.Add("numberr", number);
                oc.Parameters.Add("postcode", postcode);
                oc.Parameters.Add("city", city);
                DbConnection.Instance.Execute(oc);
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

            if (tbStreetNumber.Text != ""
                && AddCamping(tbCampingName.Text, tbStreet.Text, Convert.ToInt32(tbStreetNumber.Text), tbPostcode.Text, tbCity.Text))
            {
                RefreshCampingsListBox();
            }
            else
            {
                Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"Input was incorrect, please correct\")</SCRIPT>");
            }
            
           
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