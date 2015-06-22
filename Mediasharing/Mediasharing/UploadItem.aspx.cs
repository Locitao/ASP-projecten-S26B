using System;

namespace Mediasharing
{
    /// <summary>
    /// This page contains the information, event and methods that allow the user to post a message and file.
    /// </summary>
    public partial class UploadItem : System.Web.UI.Page
    {
        #region Fields
        private Account _user;
        private int _categoryId;
        #endregion

        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            _user = (Account)Session["user"];
            CheckIfLoggedIn();
            Rout();
        }

        /// <summary>
        /// Routes the user to the correct page.
        /// </summary>
        public void Rout()
        {
            _categoryId = Convert.ToInt32(Page.RouteData.Values["id"]);
        }

        /// <summary>
        /// Checks if the user is logged in, if not, the user is redirected to the login page.
        /// </summary>
        public void CheckIfLoggedIn()
        {
            if (Session["user"] == null)
            {
                Response.Redirect("InlogPagina.aspx", true);
            }
        }

        /// <summary>
        /// This method saves the image uploaded by the user onto the server and stores the link into the database.
        /// The path gets returned, if the method fails a link to an errordisplay picture is returned.
        /// </summary>
        /// <returns>string imagepath</returns>
        public bool UploadFile()
        {
            //Code from the internet, edited to suit this specific program.
            //Checks if the FileUpload has a file.
            if (fuUpload.HasFile)
            {
                try
                {
                    //Checks if file format is correct.
                    if (fuUpload.PostedFile.ContentType == "image/jpeg" || fuUpload.PostedFile.ContentType == "image/png" || fuUpload.PostedFile.ContentType == "image/bmp")
                    {
                        //Checks if the size is less than one megabyte.
                        if (fuUpload.PostedFile.ContentLength < 1024000)
                        {
                            //Gets the maxId from the database so we know we won't have an duplicate imagename.
                            //Combine this id with the filename.
                            Database database = Database.Instance;
                            int maxId = database.GetMaxBijdrageId();
                            string imagename = "Id_" + maxId + "_" + fuUpload.FileName;

                            string savepath = "~/Uploads/" + imagename;

                            //Saves the file.
                            fuUpload.SaveAs(Server.MapPath(savepath));

                            //Inserts the imagedata into the database.
                            database.InsertItem(_user.Id, _categoryId, tbTitle.Text, tbContent.Text, savepath,
                            fuUpload.PostedFile.ContentLength, OracleDate.GetOracleDate());

                            return true;
                        }
                        else
                            lblErrorMessage.Text = "The picturesize is too large, please upload a file under 1MB.";
                    }
                    else
                        lblErrorMessage.Text = "Only .png, .bmp or .jpeg files are supported.";
                }
                catch (Exception ex)
                {
                    lblErrorMessage.Text = "Upload status: error: " + ex.Message;
                }
            }
            return false;
        }
        #endregion

        #region Events
        /// <summary>
        /// Calls the UploadFile method which uploads the file, when everything went as expected,
        /// the event redirects you to the category which you came from.
        /// If not, an error message will appear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (tbContent.Text != "" && tbTitle.Text != "")
            {
                if (UploadFile())
                {
                    //OK, redirect to the page the user came from.
                    Response.Redirect("/Index/" + _categoryId, true);
                }
                else
                {
                    //Something went wrong, display an error message.
                    lblErrorMessage.Text = "Something went wrong";
                    lblErrorMessage.CssClass = "error";
                }
            }
            lblErrorMessage.Text = "Please enter a message and a title.";
            lblErrorMessage.CssClass = "error";
        }

        /// <summary>
        /// Redirects the user to the page which he came from.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Index/0", true);
        }
        #endregion
    }
}