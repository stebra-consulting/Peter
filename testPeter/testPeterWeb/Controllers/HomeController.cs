using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Web;
using System.Web.Mvc;
using testPeterWeb.Models;

namespace testPeterWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (ClientContext clientContext = new ClientContext("https://stebra.sharepoint.com/sites/sd1"))
            {
                if (clientContext != null)
                {
                    SecureString passWord = new SecureString();

                    foreach (char c in "CTvsTonDD2".ToCharArray()) passWord.AppendChar(c);

                    clientContext.Credentials = new SharePointOnlineCredentials("peter.ayvazian@stebra.se", passWord);

                    Web web = clientContext.Web;

                    clientContext.Load(web);

                    clientContext.ExecuteQuery();

                    ViewBag.webTitle = web.Title;
                }
            }

            return View();
        }

        public ActionResult About()
        {
            using (ClientContext clientContext = new ClientContext("https://stebra.sharepoint.com/sites/sd1"))
            {
                if (clientContext != null)
                {
                    SecureString passWord = new SecureString();
                    Microsoft.SharePoint.Client.List oList = clientContext.Web.Lists.GetByTitle("NyhetsLista");
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = @"
                                        <View>
                                            <Query>
                                                <Where>
                                                    <IsNotNull>
                                                        <FieldRef Name='Title' />
                                                    </IsNotNull>
                                                </Where>
                                            </Query>
                                        </View>";
                    ListItemCollection collListItem = oList.GetItems(camlQuery);

                    foreach (char c in "CTvsTonDD2".ToCharArray()) passWord.AppendChar(c);

                    clientContext.Credentials = new SharePointOnlineCredentials("peter.ayvazian@stebra.se", passWord);

                    Web web = clientContext.Web;

                    clientContext.Load(web);
                    clientContext.Load(collListItem);


                    clientContext.ExecuteQuery();

                    List<News> newsList = new List<News>();
                    foreach (var item in collListItem)
                    {
                        newsList.Add(new News() {
                            Title = item["Title"].ToString(),
                            Body = item["Body"].ToString(),
                            Article = item["Article"].ToString()
                        });
                    }
                    ViewData["MyNews"] = newsList;

                    NewsList.AllNews = newsList;

                    ViewBag.webTitle = web.Title;
                }
            }

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Item(string title)
        {

            string itemTitle = URLFriendly(title);
            //ListItemCollection allListItems = listObject.ListItemsGlobal;
            List<News> currentNews = NewsList.AllNews;

            string currentNewsEntry = "";

            foreach (var item in currentNews)
            {
                string currentTitle = item.Title;
                if (currentTitle == itemTitle)
                {
                    currentNewsEntry +=
                        "<h1>" + item.Title + "</h1>" +
                        "<h2>" + item.Body + "</h2>" +
                        "<p>" + item.Article + "</p>";   

                    break;
                }
            }


            ViewBag.NewsEntry = currentNewsEntry;
            return View();
        }

        public static string URLFriendly(string title)
        {
            if (title == null) return "";

            const int maxlen = 80;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

        private static object RemapInternationalCharToAscii(char c)
        {
            throw new NotImplementedException();
        }
    }
}
