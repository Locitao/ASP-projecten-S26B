using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediasharing
{
    public abstract class Bijdrage
    {
        //Properties
        public int BijdrageId { get; set; }
        public Account Poster { get; set; }
        public DateTime Date { get; set; }

        //Constructor
        protected Bijdrage(Account poster)
        {
            Poster = poster;
        }

        protected Bijdrage(Account poster, DateTime date)
        {
            Poster = poster;
            Date = date;
        }

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

            if (liked > 0)
            {
                return true;
            }
            return false;
        }
    }
}