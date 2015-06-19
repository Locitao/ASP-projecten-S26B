using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;

namespace Mediasharing
{
    public partial class Item : System.Web.UI.Page
    {
        #region Fields
        private int _itemId;
        private int _itemMessageId; //An item can be posted with a desciption message, the itemMessageId stores the id of this message.
        #endregion

        #region Methods
        /// <summary>
        /// The page load method, get's called everytime the page is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckIfLoggedIn();
            Rout();
            LoadItemMessage();  
            if (!IsPostBack)
            {
                LoadImage();
                LoadItemMessages();
            }
        }

        /// <summary>
        /// Routes the user to the correct page.
        /// </summary>
        public void Rout()
        {
            _itemId = Convert.ToInt32(Page.RouteData.Values["id"]);
        }

        /// <summary>
        /// Checks if the user is currently logged in, if not, the user gets redirected to the login page.
        /// </summary>
        public void CheckIfLoggedIn()
        {
            if (Session["user"] == null)
            {
                Response.Redirect("InlogPagina.aspx", true);
            }
        }

        /// <summary>
        /// Gets the imagelocation from the database and sets the uploadedImage Image's path.
        /// </summary>
        public void LoadImage()
        {
            Database database = Database.Instance;
            List<Dictionary<string, object>> output = database.GetItem(_itemId);
            string imagePath = Convert.ToString(output[0]["BESTANDSLOCATIE"]);
            uploadedImage.ImageUrl = imagePath;
        }

        /// <summary>
        /// Loads the message that belongs to the corresponding item.
        /// </summary>
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
                                              "AND bb.\"bijdrage_id\" = " + _itemId);

                //Sets the item messageId of the corresponding Item.
                _itemMessageId = Convert.ToInt32(output.Tables[0].Rows[0]["ID"]);

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

        /// <summary>
        /// Loads the messages that are replies to the message that belongs to the corresponding item.
        /// </summary>
        public void LoadItemMessages()
        {
            try
            {
                //Retrieves all the messages that are replies to the message tied to the item.
                Database database = Database.Instance;
                List<Dictionary<string, object>> output = database.GetItemMessages(_itemMessageId);
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

        /// <summary>
        /// Loads the reactions that are reactions to the messages posted by other people.
        /// </summary>
        /// <param name="messageId"> the id of the selected message</param>
        /// <returns></returns>
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

        #endregion

        #region Events
        protected void lbItemMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            int messageId = Convert.ToInt32(lbMessages.SelectedValue);
            List<Bericht> reactions = LoadReactions(messageId);

            //Bind to reactions lbReactions
            lbReactions.DataSource = reactions;
            lbReactions.DataTextField = "DisplayValue";
            lbReactions.DataValueField = "MessageId";
            lbReactions.DataBind();
        }

        protected void btnReaction_Click(object sender, EventArgs e)
        {

        }

        protected void btnLikeMessage_Click(object sender, EventArgs e)
        {

        }

        protected void btnReportMessage_Click(object sender, EventArgs e)
        {

        }

        protected void btnLikeReaction_Click(object sender, EventArgs e)
        {

        }

        protected void btnReportReaction_Click(object sender, EventArgs e)
        {

        }

        protected void lbMessages_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void lbReactions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    #endregion
    }
}