using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediasharing
{
    public abstract class Bijdrage
    {
        #region Properties
        public int BijdrageId { get; set; }
        public Account Poster { get; set; }
        public DateTime Date { get; set; }
        #endregion

        #region Constructors
        protected Bijdrage(Account poster)
        {
            Poster = poster;
        }

        protected Bijdrage(Account poster, DateTime date)
        {
            Poster = poster;
            Date = date;
        }
        #endregion

        #region Static Methods
        public static int GetLikes(int id)
        {
            Database database = Database.Instance;
            List<Dictionary<string, object>> output = database.GetLikes(id);
            int likes = Convert.ToInt32(output[0]["LIKES"]);
            return likes;
        }

        public static bool IsLiked(int id, int userId)
        {
            Database database = Database.Instance;
            List<Dictionary<string, object>> output = database.GetLikedByUser(id, userId);
            int liked = Convert.ToInt32(output[0]["LIKED"]);

            return liked > 0;
        }

        public static bool IsReported(int id, int userId)
        {
            Database database = Database.Instance;
            List<Dictionary<string, object>> output = database.GetReportedByUser(id, userId);
            int reported = Convert.ToInt32(output[0]["REPORTED"]);

            return reported > 0;
        }

        public static bool Like(int id, int userId)
        {
            Database database = Database.Instance;
            return database.InsertLike(id, userId);
        }

        public static bool Unlike(int id, int userId)
        {
            Database database = Database.Instance;
            return database.DeleteLike(id, userId);
        }

        public static bool Report(int id, int userId)
        {
            Database database = Database.Instance;
            return database.InsertReport(id, userId);
        }
        #endregion
    }
}