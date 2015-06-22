using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediasharing
{
    public class Categorie : Bijdrage
    {
        public int CategorieId { get; set; }
        public string Naam { get; set; }

        public Categorie(Account gebruiker, DateTime plaatsingsDatum, string naam)
            : base(gebruiker, plaatsingsDatum)
        {
            Naam = naam;
        }
    }
}