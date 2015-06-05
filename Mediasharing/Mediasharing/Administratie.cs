using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Mediasharing
{
    public class Administratie
    {
        public Account Gebruiker { get; set; }

        //Constructor
        public Administratie()
        {
        }

        public Administratie(Account gebruiker)
        {
            Gebruiker = gebruiker;
        }

        public DataSet GetData(string query)
        {
            Database database = new Database();
            DataSet output = new DataSet();
            return output = database.GetData(query);
        }
    }
}