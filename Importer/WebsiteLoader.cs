using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Importer
{
    public class WebsiteLoader : IWebsiteLoader
    {
        private HttpWebRequest webRequest;
        private readonly string uriStart = @"https://www.transportfever.net/filebase/index.php?filebase/80-transport-fever-2/";
        private readonly string defaultPath = "html/body/div[@id='pageContainer']/section[@id='main']/div/div";
        private CookieContainer cookie;
        protected Dictionary<string, string> Pairs = new Dictionary<string, string>();
        public WebsiteLoader()
        {         
            //SetCookies();
        }
        public HtmlDocument GetHtml(string uri=null)
        {
            PrepareRequest(uri == null ? uriStart : uri);
            HtmlDocument htmlDocument = new HtmlDocument();
            using (var response = (HttpWebResponse)webRequest.GetResponse())
            {
                string codePage = new StreamReader(response.GetResponseStream()).ReadToEnd();
                htmlDocument.LoadHtml(codePage);
            }
            return htmlDocument;
        }
        public HtmlDocument GetHtml(string uri, bool flagOverride)
        {
            if (flagOverride)
            {

            }
            else
            {

            }
            return null;
        }

        private void PrepareRequest(string uri)
        {
            webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.AllowAutoRedirect = false;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            WebHeaderCollection myWebHeaderCollection = webRequest.Headers;
            myWebHeaderCollection.Add("Accept-Language", "en;q=0.8");
            webRequest.CookieContainer = cookie != null ? cookie : null;
            webRequest.ContentLength = 0;                  
        }             
    }
}
