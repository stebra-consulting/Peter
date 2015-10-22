using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventDemoWeb.Models
{
    public class News
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Article { get; set; }
        public News(string title, string body, string article)
        {
            Title = title;
            Body = body;
            Article = article;
        }
    }
}