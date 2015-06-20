
using System;

namespace Mediasharing
{
    /// <summary>
    /// This class contains the methods relating to "berichten",
    /// and the information needed to create a "berichten".
    /// </summary>
    public class Bericht : Bijdrage
    {
        #region Properties
        public int MessageId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string DisplayValue { get { return "Username: " + Poster.Username + ", Title: " + Title + ", Content: " + Content;} }
        public new Account Poster { get; set; }
        #endregion

        #region Constructors
        public Bericht(int messageId, Account poster, string title, string content)
            : base(poster)
        {
            MessageId = messageId;
            Title = title;
            Content = content;
            Poster = poster;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method inserts a message into the database.
        /// </summary>
        /// <param name="title">title of the message</param>
        /// <param name="content">content of the message</param>
        /// <param name="categoryId">the corresponding category id</param>
        /// <param name="userId">the id of the user who posted the message</param>
        /// <returns></returns>
        public static bool InsertMessage(string title, string content, int categoryId, int userId)
        {
            Database database = Database.Instance;
            return database.InsertMessageCategory(title, content, categoryId, userId, DateTime.Now);
        }
        #endregion
    }
}