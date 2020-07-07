using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Models;

namespace TF2ModsList.Services
{
    public class WebOperation : IWebOperation
    {
        private readonly string uriStart = @"https://www.transportfever.net/filebase/index.php?filebase/80-transport-fever-2/";
        private IDataOperation operationData;

        public WebOperation(IDataOperation operationData)
        {
            this.operationData = operationData;
        }

        public HtmlDocument ReadWeb(string uri = null)
        {
            return ReadPage(uri ?? uriStart);
        }

        private HtmlDocument ReadPage(string uri)
        {
            string codePage = string.Empty;
            var webRequest = PrepareHtmlRequest(uri);
            using (var response = (HttpWebResponse) webRequest.GetResponse())
            {
                codePage = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Singleton.Instance.HistoryUri.Add(response.ResponseUri.ToString());
                Singleton.Instance.Cookies = response.Cookies;
                operationData.LoadHtml(codePage);
            };
            if (operationData.CheckAcceptTerms())
                return ReadPage(Singleton.Instance.HistoryUri[Singleton.Instance.HistoryUri.Count-1]);
            else
                return operationData.Html;
        }

        private HttpWebRequest PrepareHtmlRequest(string uri)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.AllowAutoRedirect = false;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            WebHeaderCollection myWebHeaderCollection = webRequest.Headers;
            myWebHeaderCollection.Add("Accept-Language", "en;q=0.8");
            if (operationData.Html != null)
            {
                var data = operationData.PreparePostData();
                webRequest.ContentLength = data.Length;
                using (var stream = webRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            webRequest.CookieContainer = new CookieContainer();
            if (Singleton.Instance.Cookies.Count>0)
                webRequest.CookieContainer.Add(Singleton.Instance.Cookies);
            return webRequest;
        }
    }
}
