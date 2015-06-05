using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediasharing
{
    public class Bestand : Bijdrage
    {
        //Properties
        public int BestandId { get; set; }
        public string Bestandslocatie { get; set; }
        public int Grootte { get; set; }

        //Constructor
        public Bestand(Account gebruiker, DateTime plaatsingsDatum, string bestandslocatie, int grootte)
            : base(gebruiker, plaatsingsDatum)
        {
            Bestandslocatie = bestandslocatie;
            Grootte = grootte;
        }
    }
}