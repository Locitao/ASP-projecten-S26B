using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;

namespace Mediasharing
{
    /// <summary>
    /// This page shows the information on the selected item,
    /// such as the attached message and the file itself.
    /// </summary>
    public partial class Item : System.Web.UI.Page
    {
        #region Fields
        private int _itemId;
        private int _itemMessageId; //An item can be posted with a desciption message, the itemMessageId stores the id of this message.
        private Account _user;
        private string _imagePath;
        #endregion

        #region Update Methods

        /// <summary>
        /// This method updates the labels that display the number of likes.
        /// </summary>
        /// <param name="id"> the id of the selected message </param>
        /// <param name="type"> the type, is the selected id from a message or a reaction?</param>
        public void UpdateLikes(int id, string type)
        {
            var likes = Bijdrage.GetLikes(id);
            string likeString = "";

            switch (likes)
            {
                case 0:
                    likeString = "Be the first to like this!";
                    break;
                case 1:
                    likeString = likes + " Like";
                    break;
                default:
                    likeString = likes + " Likes";
                    break;
            }

            switch (type)
            {
                case "message":
                    lblMessageLikes.Text = likeString;
                    break;
                case "reaction":
                    lblReactionLikes.Text = likeString;
                    break;
            }
        }

        /// <summary>
        /// This method updates the like and report buttons for the message and reaction listboxes.
        /// When a user liked a message or reaction, he or she can't report it and vice versa.
        /// </summary>
        /// <param name="id"> the id of the user</param>
        /// <param name="type"> either a "message" or a "reaction"</param>
        public void UpdateButtons(int id, string type)
        {
            //The type is a message.
            if (type == "message")
            {
                //Let's checked if the user reported the message
                if (Bijdrage.IsReported(id, _user.Id))
                {
                    //The selected message is reported, so we can't like it!
                    btnLikeMessage.Text = "Reported";
                    btnLikeMessage.CssClass = "buttondisabled";
                    btnLikeMessage.Enabled = false;

                    //Let's disable the report button.
                    btnReportMessage.Text = "Reported";
                    btnReportMessage.CssClass = "buttondisabled";
                    btnLikeMessage.Enabled = false;
                }
                else
                {
                    //The selected message isn't reported, we can like or unlike it!
                    //Let's check if the user already liked the reaction, and edit the button to Like or Unlike.
                    if (Bijdrage.IsLiked(id, _user.Id))
                    {
                        //It's already liked!
                        btnLikeMessage.Text = "Unlike";
                        btnLikeMessage.CssClass = "button";
                        btnLikeMessage.Enabled = true;

                        //Let's disable the report button, we can't report liked files after all.
                        btnReportMessage.Text = "Liked";
                        btnReportMessage.CssClass = "buttondisabled";
                        btnReportMessage.Enabled = false;
                    }
                    else
                    {
                        //It's not liked yet!
                        btnLikeMessage.Text = "Like";
                        btnLikeMessage.CssClass = "button";
                        btnLikeMessage.Enabled = true;

                        //This means we can report the message.
                        btnReportMessage.Text = "Report";
                        btnReportMessage.CssClass = "button";
                        btnReportMessage.Enabled = true;
                    }
                }
            }
            //The type is a reaction, but let's check to be sure.
            else if (type == "reaction")
            {
                //Let's checked if the user reported the reaction.
                if (Bijdrage.IsReported(id, _user.Id))
                {
                    //The selected reaction is reported, so we can't like it!
                    btnLikeReaction.Text = "Reported";
                    btnLikeReaction.CssClass = "buttondisabled";
                    btnLikeReaction.Enabled = false;

                    //Let's disable the report button.
                    btnReportReaction.Text = "Reported";
                    btnReportReaction.CssClass = "buttondisabled";
                    btnLikeReaction.Enabled = false;
                }
                else
                {
                    //The selected reaction isn't reported, we can like or unlike it!
                    //Let's check if the user already liked the reaction, and edit the button to Like or Unlike.
                    if (Bijdrage.IsLiked(id, _user.Id))
                    {
                        //It's already liked!
                        btnLikeReaction.Text = "Unlike";
                        btnLikeReaction.CssClass = "button";
                        btnLikeReaction.Enabled = true;

                        //Let's disable the report button, we can't report liked reactions after all.
                        btnReportReaction.Text = "Liked";
                        btnReportReaction.CssClass = "buttondisabled";
                        btnReportReaction.Enabled = false;
                    }
                    else
                    {
                        //It's not liked yet!
                        btnLikeReaction.Text = "Like";
                        btnLikeReaction.CssClass = "button";
                        btnLikeReaction.Enabled = true;

                        //This means we can report the message.
                        btnReportReaction.Text = "Report";
                        btnReportReaction.CssClass = "button";
                        btnReportReaction.Enabled = true;
                    }
                }
            }
        }
        #endregion

        #region LoadMethods

        /// <summary>
        /// The page load method, called when the page is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _user = (Account)Session["user"];
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
        /// Gets the imagelocation from the database and sets the uploadedImage Image's path.
        /// </summary>
        public void LoadImage()
        {
            Database database = Database.Instance;
            List<Dictionary<string, object>> output = database.GetItem(_itemId);
            _imagePath = Convert.ToString(output[0]["BESTANDSLOCATIE"]);
            uploadedImage.ImageUrl = _imagePath;
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
                lbMessages.DataSource = itemMessages;
                lbMessages.DataTextField = "DisplayValue";
                lbMessages.DataValueField = "MessageId";
                lbMessages.DataBind();
                
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

        #region Misc Methods
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
        #endregion

        #region Events

        /// <summary>
        ///     Inserts a report of a reaction into the database, and changes the corresponding buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReportReaction_Click(object sender, EventArgs e)
        {
            int reactionId = Convert.ToInt32(lbReactions.SelectedValue);
            Bijdrage.Report(reactionId, _user.Id);
            UpdateButtons(reactionId, "reaction");
        }

        /// <summary>
        ///     When the index of the listbox containing reactions changes, the corresponding buttons change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReactions_SelectedIndexChanged(object sender, EventArgs e)
        {
            int reactionId = Convert.ToInt32(lbReactions.SelectedValue);
            UpdateButtons(reactionId, "reaction");
            UpdateButtons(reactionId, "reaction");
            UpdateLikes(reactionId, "reaction");
        }

        protected void btnReaction_Click(object sender, EventArgs e)
        {
            //Checks wheter data is filled in or not.
            if (tbTitle.Text == "" && tbContent.Text == "")
            {
                //What's the point of a message if it's empty? Show error.
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "Please enter a message.";
            }
            else
            {
                //Insert reaction into the database.
                Database database = Database.Instance;
                database.InsertReaction(_user.Id, tbTitle.Text, tbContent.Text, OracleDate.GetOracleDate());
            }
        }

        /// <summary>
        ///     Loads the reactions in a listbox for the selected message in the other listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            int messageId = Convert.ToInt32(lbMessages.SelectedValue);
            List<Bericht> reactions = LoadReactions(messageId);

            //Bind to reactions lbReactions
            lbReactions.DataSource = reactions;
            lbReactions.DataTextField = "DisplayValue";
            lbReactions.DataValueField = "MessageId";
            lbReactions.DataBind();

            //Updates the buttons
            UpdateLikes(messageId, "message");
            UpdateButtons(messageId, "message");
            UpdateButtons(messageId, "message");
        }

        /// <summary>
        ///     Inserts a like of a message into the database, and changes the corresponding buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLikeMessage_Click(object sender, EventArgs e)
        {
            int messageId = Convert.ToInt32(lbMessages.SelectedValue);

            if (btnLikeMessage.Text == "Like")
            {
                Bijdrage.Like(messageId, _user.Id);
                UpdateButtons(messageId, "message");
                UpdateLikes(messageId, "message");
            }
            else if (btnLikeMessage.Text == "Unlike")
            {
                Bijdrage.Unlike(messageId, _user.Id);
                UpdateButtons(messageId, "message");
                UpdateLikes(messageId, "message");
            }
        }

        /// <summary>
        ///     Inserts a report of a message into the database, and changes the corresponding buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReportMessage_Click(object sender, EventArgs e)
        {
            int messageId = Convert.ToInt32(lbMessages.SelectedValue);
            Bijdrage.Report(messageId, _user.Id);
            UpdateButtons(messageId, "message");
        }

        /// <summary>
        ///     Inserts a like of a reaction into the database, and changes the corresponding buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLikeReaction_Click(object sender, EventArgs e)
        {
            int reactionId = Convert.ToInt32(lbReactions.SelectedValue);

            if (btnLikeReaction.Text == "Like")
            {
                Bijdrage.Like(reactionId, _user.Id);
                UpdateButtons(reactionId, "reaction");
                UpdateLikes(reactionId, "reaction");
            }
            else if (btnLikeReaction.Text == "Unlike")
            {
                Bijdrage.Unlike(reactionId, _user.Id);
                UpdateButtons(reactionId, "reaction");
                UpdateLikes(reactionId, "reaction");
            }
        }

        /// <summary>
        /// Redirects the user to the page he or she came from.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Database database = Database.Instance;
            int categoryId = database.GetCategoryIdWithItemId(_itemId);
            Response.Redirect("/Index/" + categoryId);
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Download/" + _imagePath);
        }
    #endregion
    }
}