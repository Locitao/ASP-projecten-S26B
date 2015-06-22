using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
                RefreshCampingSpotSpecifications();
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

        private void RefreshCampingSpotSpecifications()
        {
            string query = "select * from specificatie";
            OracleCommand oc = new OracleCommand(query);
            List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(oc);
            foreach (Dictionary<string, object> dic in output)
            {
                ddlSpecification1.Items.Add(Convert.ToString((long)dic["ID"]) + ": " + (string)dic["naam"]);
                ddlSpecification2.Items.Add(Convert.ToString((long)dic["ID"]) + ": " + (string)dic["naam"]);
                ddlSpecification3.Items.Add(Convert.ToString((long)dic["ID"]) + ": " + (string)dic["naam"]);
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

        private bool AddCampingSpot(int locId, int number, int capacity, List<int> specIdList, List<string> specValueList)
        {
            try
            {
                string query = "insert into plek values (null, :locId, :numberr, :capacity)";
                OracleCommand oc = new OracleCommand(query);
                oc.Parameters.Add("locId", locId);
                oc.Parameters.Add("numberr", number);
                oc.Parameters.Add("capacity", capacity);
                DbConnection.Instance.Execute(oc);

                query = "select max(ID) as VALUE from plek";
                oc = new OracleCommand(query);
                List<Dictionary<string, object>> output = DbConnection.Instance.ExecuteQuery(oc);
                int spotId = (int)(decimal)output[0]["VALUE"];
                foreach (int id in specIdList)
                {
                    int index = specIdList.IndexOf(id);
                    string specValue = specValueList[index];
                    query = "insert into plek_specificatie values (null, :specId, :spotId, :specValue)";
                    oc = new OracleCommand(query);
                    oc.Parameters.Add("specId", id);
                    oc.Parameters.Add("spotId", spotId);
                    oc.Parameters.Add("specValue", specValue);
                    DbConnection.Instance.Execute(oc);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool AddEvent(string name, int locId, DateTime dateStart, DateTime dateEnd, int maxVisitors)
        {
            try
            {
                string query = "insert into event values(null, :locId, :name, :dateStart, :dateEnd, :maxVisitors)";
                OracleCommand oc = new OracleCommand(query);
                oc.Parameters.Add("locId", locId);
                oc.Parameters.Add("name", name);
                oc.Parameters.Add("dateStart", dateStart);
                oc.Parameters.Add("dateEnd", dateEnd);
                oc.Parameters.Add("maxVisitors", maxVisitors);
                DbConnection.Instance.Execute(oc);
                return true;
            }
            catch
            {
                return false;
            }

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
                Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"Input was incorrect, please fill in correct input\")</SCRIPT>");
            }
            
           
        }

        protected void btnAddCampingSpot_OnClick(object sender, EventArgs e)
        {
            List<int> specIdList = new List<int>();
            List<string> specValueList = new List<string>();
            int indexOfColon;
            if (tbSpecificationValue1.Text != "")
            {
                indexOfColon = ddlSpecification1.SelectedItem.ToString().IndexOf(':');
                int specId = Convert.ToInt32(ddlSpecification1.SelectedItem.ToString().Substring(0, indexOfColon));
                specIdList.Add(specId);
                specValueList.Add(tbSpecificationValue1.Text);
            }
            if (tbSpecificationValue2.Text != "")
            {
                indexOfColon = ddlSpecification2.SelectedItem.ToString().IndexOf(':');
                int specId = Convert.ToInt32(ddlSpecification2.SelectedItem.ToString().Substring(0, indexOfColon));
                specIdList.Add(specId);
                specValueList.Add(tbSpecificationValue2.Text);
            }
            if (tbSpecificationValue3.Text != "")
            {
                indexOfColon = ddlSpecification3.SelectedItem.ToString().IndexOf(':');
                int specId = Convert.ToInt32(ddlSpecification3.SelectedItem.ToString().Substring(0, indexOfColon));
                specIdList.Add(specId);
                specValueList.Add(tbSpecificationValue3.Text);
            }

            indexOfColon = ddlEvents.SelectedItem.ToString().IndexOf(':');
            int eventId = Convert.ToInt32(ddlEvents.SelectedItem.ToString().Substring(0, indexOfColon));
            if (AddCampingSpot(eventId, Convert.ToInt32(tbSpotNumber.Text), Convert.ToInt32(tbCapacity.Text), specIdList, specValueList))
            {
                RefreshCampingSpotsListBox(1);
            }
            else
            {
                Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"Input was incorrect, please fill in correct input\")</SCRIPT>");
            }
        }

        protected void btnAddEvent_OnClick(object sender, EventArgs e)
        {
            int indexOfColon = ddlEvents.SelectedItem.ToString().IndexOf(':');
            int eventId = Convert.ToInt32(ddlEvents.SelectedItem.ToString().Substring(0, indexOfColon));
            if (AddEvent(tbEventName.Text, eventId,
                DateTime.ParseExact(tbDateStart.Text, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture),
                DateTime.ParseExact(tbDateEnd.Text, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture), Convert.ToInt32(tbEventCapacity.Text)))
            {
                RefreshEventsListBox(1);
            }
            else
            {
                Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(\"Input was incorrect, please fill in correct input\")</SCRIPT>");
            }
        }

        protected void lbCampings_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int indexOfColon = lbCampings.SelectedItem.ToString().IndexOf(':');
            int campingId = Convert.ToInt32(lbCampings.SelectedItem.ToString().Substring(0, indexOfColon));
            RefreshCampingSpotsListBox(campingId);
            RefreshEventsListBox(campingId);
        }
    }
}