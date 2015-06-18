using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;

namespace Mediasharing
{
    public partial class Item : System.Web.UI.Page
    {
        private int itemId;
        //An item can be posted with a desciption message, the itemMessageId stores the id of this message.
        private int itemMessageId;

        protected void Page_Load(object sender, EventArgs e)
        {
            Rout();
            LoadItemMessage();
            if (!IsPostBack)
            {
                LoadItemMessages();
            }
        }

        public void Rout()
        {
           itemId = Convert.ToInt32(Page.RouteData.Values["id"]);
        }

        public void LoadItemMessage()
        {
            //Initialize
            DataSet output = new DataSet();
            Database database = Database.Instance;

            try
            {
                    output = database.GetData("SELECT bij.\"ID\" AS ID, acc.\"gebruikersnaam\" AS GEBRUIKERSNAAM, ber.\"titel\" AS TITEL, ber.\"inhoud\" AS INHOUD " +
                                              "FROM BIJDRAGE bij, BERICHT ber, ACCOUNT acc, BIJDRAGE_BERICHT bb " +
                                              "WHERE bij.\"account_id\" = acc.\"ID\" " +
                                              "AND ber.\"bijdrage_id\" = bij.\"ID\" " +
                                              "AND bij.\"ID\" = bb.\"bericht_id\" " +
                                              "AND bb.\"bijdrage_id\" = " + itemId);

                //Sets the item messageId of the corresponding Item.
                itemMessageId = Convert.ToInt32(output.Tables[0].Rows[0]["ID"]);

                //Binds the data to the repeater.
                RepeaterItemView.DataSource = output;
                RepeaterItemView.DataBind();
            }
            catch (OracleException ex)
            {
                //Prints the errormessages.
                System.Diagnostics.Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
        }

        public void LoadItemMessages()
        {
            try
            {
                //Retrieves all the messages that are replies to the message tied to the item.
                Database database = Database.Instance;
                List<Dictionary<string, object>> output = database.GetItemMessages(itemMessageId);
                List<Bericht> itemMessages = new List<Bericht>();

                //Creates the messages.
                foreach (Dictionary<string, object> dic in output)
                {
                    int id = Convert.ToInt32(dic["ID"]);
                    string title = Convert.ToString(dic["TITEL"]);
                    string content = Convert.ToString(dic["INHOUD"]);
                    string username = Convert.ToString(dic["GEBRUIKERSNAAM"]);

                    Account account = new Account(username);
                    Bericht message = new Bericht(id, account, title, content);
                    itemMessages.Add(message);
                }

                //Binds the messages to the listbox.
                lbItemMessages.DataSource = itemMessages;
                lbItemMessages.DataTextField = "DisplayValue";
                lbItemMessages.DataValueField = "MessageId";
                lbItemMessages.DataBind();
                
            }
            catch (OracleException ex)
            {
                //Prints the errormessages.
                System.Diagnostics.Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
        }

        public List<Bericht> LoadReactions(int messageId)
        {
            try
            {
                //Retrieves all the messages that aren't replies from the database.
                Database database = Database.Instance;
                List<Dictionary<string, object>> output = database.GetReactions(messageId);
                List<Bericht> reactions = new List<Bericht>();

                //Creates the messages.
                foreach (Dictionary<string, object> dic in output)
                {
                    int id = Convert.ToInt32(dic["ID"]);
                    string title = Convert.ToString(dic["TITEL"]);
                    string content = Convert.ToString(dic["INHOUD"]);
                    string username = Convert.ToString(dic["GEBRUIKERSNAAM"]);

                    Account account = new Account(username);
                    Bericht message = new Bericht(id, account, title, content);
                    reactions.Add(message);
                }

                //Binds the messages to the listbox.
                return reactions;
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
            return null;
        }
        protected void lbItemMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            int messageId = Convert.ToInt32(lbItemMessages.SelectedValue);
            List<Bericht> reactions = LoadReactions(messageId);

            //Bind to reactions lbReactions
            lbReactions.DataSource = reactions;
            lbReactions.DataTextField = "DisplayValue";
            lbReactions.DataValueField = "MessageId";
            lbReactions.DataBind();
        }
    }
}