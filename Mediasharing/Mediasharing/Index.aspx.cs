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
        private int categorieId;
        private Account user;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = (Account)Session["user"];
            Rout();
            CheckIfLoggedIn();
            LoadPage();
        }

        public void Rout()
        {
            categorieId = Convert.ToInt32(Page.RouteData.Values["id"]);
        }

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

        public void CheckIfLoggedIn()
        {
            if (Session["user"] == null)
            {
                Response.Redirect("InlogPagina.aspx", true);
            }
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

        public void UpdateLikeButton(int id, string type)
        {
            //Checks which button has to be updated
            switch (type)
            {
                case "message":
                    //If true, change the like button to unlike button.
                    btnLikeMessage.Text = Bijdrage.IsLiked(id, user.Id) ? "Unlike" : "Like";
                    break;
                case "reaction":
                    btnLikeReaction.Text = Bijdrage.IsLiked(id, user.Id) ? "Unlike" : "Like";
                    break;
            }
        }

        public void UpdateReportButton(int id, string type)
        {
            //Checks which button has to be updated
            switch (type)
            {
                case "message":
                    if (Bijdrage.IsReported(id, user.Id))
                    {
                        btnReportMessage.Enabled = false;
                        btnReportMessage.CssClass = "buttondisabled";
                    }
                    break;
                case "reaction":
                    if (Bijdrage.IsReported(id, user.Id))
                    {
                        btnReportReaction.Enabled = false;
                        btnReportReaction.CssClass = "buttondisabled";
                    }
                    break;
            }
        }

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
            UpdateLikeButton(messageId, "message");
            UpdateReportButton(messageId, "message");
        }

        #region Report and Like Messages
        protected void btnLikeMessage_Click(object sender, EventArgs e)
        {
            int messageId = Convert.ToInt32(lbMessages.SelectedValue);

            if (btnLikeMessage.Text == "Like")
            {
                Bijdrage.Like(messageId, user.Id);
                UpdateLikeButton(messageId, "message");
                UpdateLikes(messageId, "message");
            }
            else if (btnLikeMessage.Text == "Unlike")
            {
                Bijdrage.Unlike(messageId, user.Id);
                UpdateLikeButton(messageId, "message");
                UpdateLikes(messageId, "message");
            }
        }

        protected void btnReportMessage_Click(object sender, EventArgs e)
        {
            int messageId = Convert.ToInt32(lbMessages.SelectedValue);
            Bijdrage.Report(messageId, user.Id);
            UpdateReportButton(messageId, "message");
        }

        #endregion

        #region Report and Like Reactions
        protected void btnLikeReaction_Click(object sender, EventArgs e)
        {
            int reactionId = Convert.ToInt32(lbReactions.SelectedValue);

            if (btnLikeMessage.Text == "Like")
            {
                Bijdrage.Like(reactionId, user.Id);
                UpdateLikeButton(reactionId, "reaction");
                UpdateLikes(reactionId, "reaction");
            }
            else if (btnLikeMessage.Text == "Unlike")
            {
                Bijdrage.Unlike(reactionId, user.Id);
                UpdateLikeButton(reactionId, "reaction");
                UpdateLikes(reactionId, "reaction");
            }
        }

        protected void btnReportReaction_Click(object sender, EventArgs e)
        {
            int reactionId = Convert.ToInt32(lbReactions.SelectedValue);
            Bijdrage.Report(reactionId, user.Id);
            UpdateReportButton(reactionId, "reaction");
        }
        #endregion

        protected void lbReactions_SelectedIndexChanged(object sender, EventArgs e)
        {
            int reactionId = Convert.ToInt32(lbReactions.SelectedValue);
            UpdateReportButton(reactionId, "reaction");
            UpdateLikeButton(reactionId, "reaction");
            UpdateLikes(reactionId, "reaction");
        }
    }
}