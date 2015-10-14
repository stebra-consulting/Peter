using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
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
                        newsList.Add(new News() { Title = item["Title"].ToString(), Body = item["Body"].ToString(), Article = item["Article"].ToString() });
                    }
                    ViewData["MyNews"] = newsList;

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
    }
}
