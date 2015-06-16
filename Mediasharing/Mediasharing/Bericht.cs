using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediasharing
{
    public class Bericht : Bijdrage
    {
         //Properties
        public int MessageId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string DisplayValue { get { return "Username:" + Poster.Username + ", Title:" + Title + ", Content: " + Content;} }
        public Account Poster { get; set; }

        //Constructor
        public Bericht(int messageId, Account poster, string title, string content)
            : base(poster)
        {
            MessageId = messageId;
            Title = title;
            Content = content;
            Poster = poster;
        }

        public Bericht(Account poster, DateTime date, string title, string content)
            : base(poster, date)
        {
            Title = title;
            Content = content;
        }
    }
}