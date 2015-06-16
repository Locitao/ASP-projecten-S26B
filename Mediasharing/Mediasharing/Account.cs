using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediasharing
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public Account(string username)
        {
            Username = username;
        }

        public Account(int id, string username)
        {
            Id = id;
            Username = username;
        }

        //Nog implementeren
        public static bool Login(int id)
        {
            return true;
        }
    }
}