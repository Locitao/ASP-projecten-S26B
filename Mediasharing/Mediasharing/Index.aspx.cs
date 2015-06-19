using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using Oracle.DataAccess.Client;

namespace Mediasharing
{
    public partial class WebForm1 : Page
    {
        #region Page Load

        /// <summary>
        ///     The page load method, get's called everytime the page is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _user = (Account) Session["user"];
            Rout();
            CheckIfLoggedIn();
            LoadPage();
        }

        #endregion

        /// <summary>
        ///     Searches the database for either categories, or media items that match the search term inputted by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Initialize.
            var database = Database.Instance;
            var searchTerm = tbSearch.Text;

            //We're searching for a category.
            if (ddlSearch.SelectedValue == "Category")
            {
                //Fill DataSet with data.
                var foundCategories = new DataSet();
                foundCategories = database.GetData("SELECT c.\"naam\" AS NAAM, b.\"ID\" AS ID " +
                                                   "FROM BIJDRAGE b " +
                                                   "JOIN CATEGORIE c ON b.\"ID\" = c.\"bijdrage_id\" " +
                                                   "WHERE c.\"naam\" LIKE " + "'%" + searchTerm + "%'");

                //Bind data to repeater.
                RepeaterSearchCategories.DataSource = foundCategories;
                RepeaterSearchCategories.DataBind();

                //Clear the other search repeaters data.
                RepeaterSearchMediaItems.DataSource = null;
                RepeaterSearchMediaItems.DataBind();
            }

            //We're searching for a media item.
            else if (ddlSearch.SelectedValue == "Media Item")
            {
                //Fill DataSet with data.
                var foundMediaItems = new DataSet();
                foundMediaItems =
                    database.GetData("SELECT bij.\"ID\" AS ID, bes.\"bestandslocatie\" AS BESTANDSLOCATIE " +
                                     "FROM BIJDRAGE bij, BESTAND bes " +
                                     "WHERE bij.\"ID\" = bes.\"bijdrage_id\" " +
                                     "AND bij.\"soort\" = 'bestand' " +
                                     "AND bes.\"bestandslocatie\" LIKE " + "'%" + searchTerm + "%'");

                //Bind data to repeater.
                RepeaterSearchMediaItems.DataSource = foundMediaItems;
                RepeaterSearchMediaItems.DataBind();

                //Clear the other search repeaters data.
                RepeaterSearchCategories.DataSource = null;
                RepeaterSearchCategories.DataBind();
            }
        }

        /// <summary>
        ///     Redirects you to the post message page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddPost_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PostMessage/" + _categoryId, true);
        }

        #region Fields

        private int _categoryId;
        private Account _user;

        #endregion

        #region Misc Methods

        /// <summary>
        ///     Routes the user to the correct page.
        /// </summary>
        public void Rout()
        {
            _categoryId = Convert.ToInt32(Page.RouteData.Values["id"]);
        }

        /// <summary>
        ///     Checks if the user is currently logged in, if not, the user gets redirected to the login page.
        /// </summary>
        public void CheckIfLoggedIn()
        {
            if (Session["user"] == null)
            {
                Response.Redirect("InlogPagina.aspx", true);
            }
        }

        #endregion

        #region Load Methods

        /// <summary>
        ///     This method loads the page by calling most of the load methods defined below.
        /// </summary>
        public void LoadPage()
        {
            //Only reloads the categories when the page isn't a postback.
            if (!IsPostBack)
            {
                var messages = LoadMessages();
                lbMessages.DataSource = messages;
                lbMessages.DataTextField = "DisplayValue";
                lbMessages.DataValueField = "MessageId";
                lbMessages.DataBind();
            }
            LoadCategories();
            LoadSubCategories();
            LoadMediaItems();
        }

        /// <summary>
        ///     This method shows the parentcategory of the current category, or parentcategories when in the rootfolder.
        /// </summary>
        public void LoadCategories()
        {
            var output = new DataSet();

            try
            {
                if (_categoryId == 0)
                {
                    var database = Database.Instance;
                    output = database.GetData("SELECT c.\"naam\" AS NAAM, b.\"ID\" AS ID " +
                                              "FROM BIJDRAGE b " +
                                              "JOIN CATEGORIE c ON b.\"ID\" = c.\"bijdrage_id\" " +
                                              "WHERE \"bijdrage_id\" IS NULL");
                }
                else
                {
                    var database = Database.Instance;
                    output = database.GetData("SELECT c.\"naam\" AS NAAM, b.\"ID\" AS ID " +
                                              "FROM BIJDRAGE b " +
                                              "JOIN CATEGORIE c ON b.\"ID\" = c.\"bijdrage_id\" " +
                                              "WHERE \"bijdrage_id\" = " + _categoryId);
                }
                RepeaterCategories.DataSource = output;
                RepeaterCategories.DataBind();
            }
            catch (OracleException ex)
            {
                Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
        }

        /// <summary>
        ///     This method loads all the sub categories of the current category.
        /// </summary>
        public void LoadSubCategories()
        {
            var output = new DataSet();
            var database = Database.Instance;

            try
            {
                if (_categoryId == 0)
                {
                    output = database.GetData("SELECT c.\"naam\" AS NAAM, b.\"ID\" AS ID " +
                                              "FROM BIJDRAGE b " +
                                              "JOIN CATEGORIE c ON b.\"ID\" = c.\"bijdrage_id\" " +
                                              "WHERE \"categorie_id\" IS NULL");
                }
                else
                {
                    output = database.GetData("SELECT c.\"naam\" AS NAAM, b.\"ID\" AS ID " +
                                              "FROM BIJDRAGE b " +
                                              "JOIN CATEGORIE c ON b.\"ID\" = c.\"bijdrage_id\" " +
                                              "WHERE \"categorie_id\" = " + _categoryId);
                }
                RepeaterSubCategories.DataSource = output;
                RepeaterSubCategories.DataBind();
            }
            catch (OracleException ex)
            {
                Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
        }

        /// <summary>
        ///     This method loads all the media item of the  current category.
        /// </summary>
        public void LoadMediaItems()
        {
            var output = new DataSet();
            var database = Database.Instance;

            try
            {
                if (_categoryId == 0)
                {
                    output = database.GetData("SELECT bij.\"ID\" AS ID, bes.\"bestandslocatie\" AS BESTANDSLOCATIE " +
                                              "FROM BIJDRAGE bij, BESTAND bes " +
                                              "WHERE bij.\"ID\" = bes.\"bijdrage_id\" " +
                                              "AND bij.\"soort\" = 'bestand' " +
                                              "AND bes.\"categorie_id\" IS NULL");
                }
                else
                {
                    output = database.GetData("SELECT bij.\"ID\" AS ID, bes.\"bestandslocatie\" AS BESTANDSLOCATIE " +
                                              "FROM BIJDRAGE bij, BESTAND bes " +
                                              "WHERE bij.\"ID\" = bes.\"bijdrage_id\" " +
                                              "AND bij.\"soort\" = 'bestand' " +
                                              "AND bes.\"categorie_id\" = " + _categoryId);
                }
                RepeaterMediaItems.DataSource = output;
                RepeaterMediaItems.DataBind();
            }
            catch (OracleException ex)
            {
                Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
        }

        /// <summary>
        ///     This method loads all the messages that aren't replies.
        /// </summary>
        /// <returns></returns>
        public List<Bericht> LoadMessages()
        {
            try
            {
                //Retrieves all the messages that aren't replies from the database.
                var database = Database.Instance;
                var output = database.GetMessages();
                var messages = new List<Bericht>();

                //Creates the messages.
                foreach (var dic in output)
                {
                    var id = Convert.ToInt32(dic["ID"]);
                    var title = Convert.ToString(dic["TITEL"]);
                    var content = Convert.ToString(dic["INHOUD"]);
                    var username = Convert.ToString(dic["GEBRUIKERSNAAM"]);

                    var account = new Account(username);
                    var message = new Bericht(id, account, title, content);
                    messages.Add(message);
                }

                //Binds the messages to the listbox.
                return messages;
            }
            catch (OracleException ex)
            {
                Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
            return null;
        }

        /// <summary>
        ///     This method loads the reactions of the corresponding selected message.
        /// </summary>
        /// <param name="messageId"> the selected messageId from lbMessages</param>
        /// <returns></returns>
        public List<Bericht> LoadReactions(int messageId)
        {
            try
            {
                //Retrieves all the messages that aren't replies from the database.
                var database = Database.Instance;
                var output = database.GetReactions(messageId);
                var reactions = new List<Bericht>();

                //Creates the messages.
                foreach (var dic in output)
                {
                    var id = Convert.ToInt32(dic["ID"]);
                    var title = Convert.ToString(dic["TITEL"]);
                    var content = Convert.ToString(dic["INHOUD"]);
                    var username = Convert.ToString(dic["GEBRUIKERSNAAM"]);

                    var account = new Account(username);
                    var message = new Bericht(id, account, title, content);
                    reactions.Add(message);
                }
                //Binds the messages to the listbox.
                return reactions;
            }
            catch (OracleException ex)
            {
                Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
            return null;
        }

        #endregion

        #region Update Methods

        public void UpdateLikes(int messageId, string type)
        {
            var likes = Bijdrage.GetLikes(messageId);
            var likeString = "";

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
        ///     This method updates the like and report buttons for the message and reaction listboxes.
        ///     When a user liked a message or reaction, he or she can't report it and vice versa.
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
                    }
                }
            }
            //The type isn't a message so it's a reaction, but let's check to be sure.
            else if (type == "reaction")
            {
                //Let's checked if the user reported the reaction.
                if (Bijdrage.IsReported(id, _user.Id))
                {
                    //The selected reaction is reported, so we can't like it!
                    btnLikeReaction.Text = "Reported";
                    btnLikeReaction.CssClass = "buttondisabled";
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
                    }
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        ///     Loads the reactions in a listbox for the selected message in the other listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            var messageId = Convert.ToInt32(lbMessages.SelectedValue);
            var reactions = LoadReactions(messageId);

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
            var messageId = Convert.ToInt32(lbMessages.SelectedValue);

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
            var messageId = Convert.ToInt32(lbMessages.SelectedValue);
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
            var reactionId = Convert.ToInt32(lbReactions.SelectedValue);

            if (btnLikeMessage.Text == "Like")
            {
                Bijdrage.Like(reactionId, _user.Id);
                UpdateButtons(reactionId, "reaction");
                UpdateLikes(reactionId, "reaction");
            }
            else if (btnLikeMessage.Text == "Unlike")
            {
                Bijdrage.Unlike(reactionId, _user.Id);
                UpdateButtons(reactionId, "reaction");
                UpdateLikes(reactionId, "reaction");
            }
        }

        /// <summary>
        ///     Inserts a report of a reaction into the database, and changes the corresponding buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReportReaction_Click(object sender, EventArgs e)
        {
            var reactionId = Convert.ToInt32(lbReactions.SelectedValue);
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
            var reactionId = Convert.ToInt32(lbReactions.SelectedValue);
            UpdateButtons(reactionId, "reaction");
            UpdateButtons(reactionId, "reaction");
            UpdateLikes(reactionId, "reaction");
        }

        #endregion
    }
}