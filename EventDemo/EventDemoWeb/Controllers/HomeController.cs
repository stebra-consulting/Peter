using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventDemoWeb.Models;

namespace EventDemoWeb.Controllers
{
    public class HomeController : Controller
    {
        [SharePointContextFilter]
        public ActionResult Index()
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    var spWeb = clientContext.Web;
                    var hostListColl = spWeb.Lists;
                    clientContext.Load(spWeb, w => w.Id);
                    //spWeb  == w. load laddar spWeb och sen får den ID.. 
                    //den returnerar objectet du laddar och de properties den får av laddningen. i det här fallet ID
                    //som jag förstår det
                    clientContext.Load(hostListColl);
                    clientContext.ExecuteQuery();
                    ViewBag.HostLists = hostListColl.Select(l => new SelectListItem() { Text = l.Title, Value = l.Title });
                }
            }
            return View();
        }

        [SharePointContextFilter]
        [HttpPost]
        public ActionResult Subscribe(string listTitle)
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (!string.IsNullOrEmpty(listTitle))
                {
                    RERUtility.AddListItemRemoteEventReceiver(
                      clientContext,
                      listTitle,
                      EventReceiverType.ItemAdded,
                      EventReceiverSynchronization.Asynchronous,
                      "Receiver1",
                      "https://serviceberra.servicebus.windows.net/2815169314/5482624/obj/cc8d6c2a-2776-45ea-967a-210c5ca3daf0/Services/Receiver1.svc",
                          10);
                }
            }
            return RedirectToAction("Index", new { SPHostUrl = spContext.SPHostUrl.ToString() });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
