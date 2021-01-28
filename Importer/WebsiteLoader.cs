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

            
            /*using (var response = (HttpWebResponse)webRequest.GetResponse())
            {
                cookie = new CookieContainer();
                cookie.Add(response.Cookies);
                ////The secure on the site has been changed
                //string codePage = new StreamReader(response.GetResponseStream()).ReadToEnd();
                /*AcceptTerms(codePage);
                using (var stream = webRequest.GetRequestStream())
                {
                    var data = PreparePostData();
                    stream.Write(data, 0, data.Length);
                    webRequest.ContentLength = data.Length;
                }
            }*/
        }

        
        /*
        private async Task<HttpWebRequest> PrepareHtmlRequest()
        {
            webRequest = (HttpWebRequest)WebRequest.Create(uriStart);
            // webRequest.ContentLength = data.Length;
            //using (var stream = webRequest.GetRequestStream())
            //    stream.Write(data, 0, data.Length);
            webRequest.CookieContainer = cookie;
            return webRequest;
        }
       
        private void AcceptTerms(string html)
        {
            HtmlDocument document = new HtmlDocument();
            if (document == null)
                throw new Exception("Empty HtmlDocument");
            else
            {
                HtmlNode selectedNode = document.DocumentNode.SelectSingleNode(defaultPath + "/form/div[@class='formSubmit']");
                if (selectedNode != null)
                    PrepareAccessResource(selectedNode);
            }
        }
        private byte[] PreparePostData()
        {
            string postData = string.Empty;
            foreach (var item in Pairs)
            {
                if (!string.IsNullOrEmpty(postData))
                    postData += "&";
                postData += item.Key + "=" + item.Value;
            };
            return Encoding.ASCII.GetBytes(postData);
        }
        private void PrepareAccessResource(HtmlNode selectedNode)
        {
            foreach (var item in selectedNode.ChildNodes)
            {
                if (item.Name == "input")
                {
                    var shortpath = item.Attributes;
                    if (shortpath[0].Name == "type" && shortpath[0].Value == "hidden")
                    {
                        if (Pairs.ContainsKey(shortpath[1].Value))
                            Pairs[shortpath[1].Value] = shortpath[2].Value;
                        else
                            Pairs.Add(shortpath[1].Value, shortpath[2].Value);
                    }
                }
            }
        }
    */
    }
}
