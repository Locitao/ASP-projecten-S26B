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
        public Account Gebruiker { get; set; }
        public DateTime PlaatsingsDatum { get; set; }

        //Constructor
        protected Bijdrage(Account gebruiker, DateTime plaatsingsDatum)
        {
            Gebruiker = gebruiker;
            PlaatsingsDatum = plaatsingsDatum;
        }
    }
}