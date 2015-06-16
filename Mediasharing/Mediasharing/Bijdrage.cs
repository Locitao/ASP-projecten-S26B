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
    }
}