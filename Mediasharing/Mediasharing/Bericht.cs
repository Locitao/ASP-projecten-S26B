using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediasharing
{
    public class Bericht : Bijdrage
    {
         //Properties
        public int BerichtId { get; set; }
        public string Titel { get; set; }
        public string Inhoud { get; set; }

        //Constructor
        public Bericht(Account gebruiker, DateTime plaatsingsDatum, string titel, string inhoud)
            : base(gebruiker, plaatsingsDatum)
        {
            Titel = titel;
            Inhoud = inhoud;
        }
    }
}