using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediasharing
{
    public class Account
    {
        public int Id { get; set; }
        public string Gebruikersnaam { get; set; }

        public Account(int id, string gebruikersnaam)
        {
            Id = id;
            Gebruikersnaam = gebruikersnaam;
        }
    }
}