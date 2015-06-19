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
    public partial class WebForm1 : System.Web.UI.Page
    {
        #region Fields
        private int categorieId;
        private Account user;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            user = (Account)Session["user"];
            Rout();
            CheckIfLoggedIn();
            LoadPage();
        }
        #endregion

        #region Misc Methods
        public void Rout()
        {
            categorieId = Convert.ToInt32(Page.RouteData.Values["id"]);
        }

        public void CheckIfLoggedIn()
        {
            if (Session["user"] == null)
            {
                Response.Redirect("InlogPagina.aspx", true);
            }
        }
        #endregion

        #region Load Methods
        public void LoadPage()
        {
            //Only reloads the categories when the page isn't a postback.
            if (!IsPostBack)
            {
                List<Bericht> messages = LoadMessages();
                lbMessages.DataSource = messages;
                lbMessages.DataTextField = "DisplayValue";
                lbMessages.DataValueField = "MessageId";
                lbMessages.DataBind();
            }
            LoadCategories();
            LoadSubCategories();
            LoadMediaItems();
        }

        public void LoadCategories()
        {
            DataSet output = new DataSet();

            try
            {
                if (categorieId == 0)
                {
                    Database database = Database.Instance;
                    output = database.GetData("SELECT c.\"naam\" AS NAAM, b.\"ID\" AS ID " +
                                              "FROM BIJDRAGE b " +
                                              "JOIN CATEGORIE c ON b.\"ID\" = c.\"bijdrage_id\" "+
                                              "WHERE \"bijdrage_id\" IS NULL");
                }
                else
                {
                    Database database = Database.Instance;
                    output = database.GetData("SELECT c.\"naam\" AS NAAM, b.\"ID\" AS ID " +
                                            "FROM BIJDRAGE b " +
                                            "JOIN CATEGORIE c ON b.\"ID\" = c.\"bijdrage_id\" " +
                                            "WHERE \"bijdrage_id\" = " + categorieId);
                }
                RepeaterCategories.DataSource = output;
                RepeaterCategories.DataBind();
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }

        }

        public void LoadSubCategories()
        {
            DataSet output = new DataSet();
            Database database = Database.Instance;

            try
            {
                if (categorieId == 0)
                {
                    output = database.GetData("SELECT c.\"naam\" AS NAAM, b.\"ID\" AS ID " +
                                              "FROM BIJDRAGE b " +
                                              "JOIN CATEGORIE c ON b.\"ID\" = c.\"bijdrage_id\" "+
                                              "WHERE \"categorie_id\" IS NULL");
                }
                else
                {
                    output = database.GetData("SELECT c.\"naam\" AS NAAM, b.\"ID\" AS ID " +
                                              "FROM BIJDRAGE b " +
                                              "JOIN CATEGORIE c ON b.\"ID\" = c.\"bijdrage_id\" " +
                                              "WHERE \"categorie_id\" = " + categorieId);
                }
                RepeaterSubCategories.DataSource = output;
                RepeaterSubCategories.DataBind();
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
        }

        public void LoadMediaItems()
        {
            DataSet output = new DataSet();
            Database database = Database.Instance;

            try
            {
                if (categorieId == 0)
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
                                              "AND bes.\"categorie_id\" = " + categorieId);
                }
                RepeaterMediaItems.DataSource = output;
                RepeaterMediaItems.DataBind();
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
        }

        public List<Bericht> LoadMessages()
        {
            try
            {
                //Retrieves all the messages that aren't replies from the database.
                Database database = Database.Instance;
                List<Dictionary<string, object>> output = database.GetMessages();
                List<Bericht> messages = new List<Bericht>();

                //Creates the messages.
                foreach (Dictionary<string, object> dic in output)
                {
                    int id = Convert.ToInt32(dic["ID"]);
                    string title = Convert.ToString(dic["TITEL"]);
                    string content = Convert.ToString(dic["INHOUD"]);
                    string username = Convert.ToString(dic["GEBRUIKERSNAAM"]);

                    Account account = new Account(username);
                    Bericht message = new Bericht(id, account, title, content);
                    messages.Add(message);
                }

                //Binds the messages to the listbox.
                return messages;
            }
            catch (OracleException ex)
            {
                System.Diagnostics.Debug.WriteLine("error message: " + ex.Message + "\n" + "error code:" + ex.ErrorCode);
            }
                return null;
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

        #endregion

        #region Update Methods
        public void UpdateLikes(int messageId, string type)
        {
            int likes = Bijdrage.GetLikes(messageId);
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

        public void UpdateButtons(int id, string type)
        {
            //The type is a message.
            if (type == "message")
            {
                //Let's checked if the user reported the message
                if (Bijdrage.IsReported(id, user.Id))
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
                    if (Bijdrage.IsLiked(id, user.Id))
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
                if (Bijdrage.IsReported(id, user.Id))
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
                    if (Bijdrage.IsLiked(id, user.Id))
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
        /// Loads the reactions in a listbox for the selected message in the other listbox.
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

        protected void btnLikeMessage_Click(object sender, EventArgs e)
        {
            int messageId = Convert.ToInt32(lbMessages.SelectedValue);

            if (btnLikeMessage.Text == "Like")
            {
                Bijdrage.Like(messageId, user.Id);
                UpdateButtons(messageId, "message");
                UpdateLikes(messageId, "message");
            }
            else if (btnLikeMessage.Text == "Unlike")
            {
                Bijdrage.Unlike(messageId, user.Id);
                UpdateButtons(messageId, "message");
                UpdateLikes(messageId, "message");
            }
        }

        protected void btnReportMessage_Click(object sender, EventArgs e)
        {
            int messageId = Convert.ToInt32(lbMessages.SelectedValue);
            Bijdrage.Report(messageId, user.Id);
            UpdateButtons(messageId, "message");
        }

        protected void btnLikeReaction_Click(object sender, EventArgs e)
        {
            int reactionId = Convert.ToInt32(lbReactions.SelectedValue);

            if (btnLikeMessage.Text == "Like")
            {
                Bijdrage.Like(reactionId, user.Id);
                UpdateButtons(reactionId, "reaction");
                UpdateLikes(reactionId, "reaction");
            }
            else if (btnLikeMessage.Text == "Unlike")
            {
                Bijdrage.Unlike(reactionId, user.Id);
                UpdateButtons(reactionId, "reaction");
                UpdateLikes(reactionId, "reaction");
            }
        }

        protected void btnReportReaction_Click(object sender, EventArgs e)
        {
            int reactionId = Convert.ToInt32(lbReactions.SelectedValue);
            Bijdrage.Report(reactionId, user.Id);
            UpdateButtons(reactionId, "reaction");
        }

        protected void lbReactions_SelectedIndexChanged(object sender, EventArgs e)
        {
            int reactionId = Convert.ToInt32(lbReactions.SelectedValue);
            UpdateButtons(reactionId, "reaction");
            UpdateButtons(reactionId, "reaction");
            UpdateLikes(reactionId, "reaction");
        }
        #endregion
    }
}