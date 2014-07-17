using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using rssSpider.Models;
using System.Xml;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace rssSpider.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext dbcontext = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            return View();
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

        public ActionResult Partial1(string make, string year_from, string year_to, string price_from, string price_to)
        {

            int prt, prf, yrt, yrf;
            bool result = int.TryParse(price_to, out prt);
            
            bool result1 = int.TryParse(price_from, out prf);

            bool result2 = int.TryParse(year_to, out yrt);

            bool result3 = int.TryParse(year_from, out yrf);

            XmlDocument rssXmlDoc = new XmlDocument();
            SpiderRss rssinput = new SpiderRss();
            // Load the RSS file from the RSS URL
            rssXmlDoc.Load("http://avtobazar.ua/feed/rss/avto/?price_to="+prt+"&model1=&year_from="+yrf+"&region1=&year_to="+yrt+"&price_from="+prf+"&make1=");

            // Parse the Items in the RSS file
            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

            StringBuilder rssContent = new StringBuilder();
            //int delId = dbcontext.SpiderRsses.Count();
            //dbcontext.SpiderRsses.Remove(delId);

            foreach (SpiderRss item in dbcontext.SpiderRsses)
            {
                for (int i = 0; i <= 10000; i++)
                    if (item.id == i)
                    {
                        dbcontext.SpiderRsses.Remove(item);
                        
                    }
                dbcontext.SaveChanges();
                //break;
            }

            string pattern = "\\s+";
            string replacement = " ";
            string res;
            bool b;
            
            string s = make;
            // Iterate through the items in the RSS file
            foreach (XmlNode rssNode in rssNodes)
            {
                XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                string title = rssSubNode != null ? rssSubNode.InnerText : "";
                if (b = title.Contains(s))
                {
                    rssinput.titel = title;
                    rssSubNode = rssNode.SelectSingleNode("link");
                    string link = rssSubNode != null ? rssSubNode.InnerText : "";
                    rssinput.link = link;

                    rssSubNode = rssNode.SelectSingleNode("description");
                    string description = rssSubNode != null ? rssSubNode.InnerText : "";
                    res = Regex.Replace(description, pattern, replacement);
                    rssinput.description = res;
                    dbcontext.SpiderRsses.Add(rssinput);
                }



                ViewData["model"] = dbcontext.SpiderRsses;
                dbcontext.SaveChanges();
            }
            return View();
        }
        
        public ActionResult Part()
        {         
            return View(dbcontext.SpiderRsses);
        }
        public ActionResult Delete(int id)
        {
            foreach (SpiderRss item in dbcontext.SpiderRsses )
            {
                if (item.id == id)
                {
                    dbcontext.SpiderRsses.Remove(item);
                    break;
                }
            }

            dbcontext.SaveChanges();
            return View("Part", dbcontext.SpiderRsses);

        }
    }
}