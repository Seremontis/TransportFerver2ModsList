using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Models;
using TF2ModsList.Services.Interface;

namespace TF2ModsList.Services
{
    public class WebOperation : IWebOperation
    {
        private readonly string uriStart = @"https://www.transportfever.net/filebase/index.php?filebase/80-transport-fever-2/";
        private IDataOperation operationData;
        private EnumWebsite _enumWebsite;
        public WebOperation(IDataOperation operationData)
        {
            this.operationData = operationData;
        }

        public async Task<HtmlDocument> ReadWeb(string uri = null,EnumWebsite enumWebsite=EnumWebsite.TF2Community)
        {
            this._enumWebsite = enumWebsite;
            return await Task.Run(()=> ReadPage(uri ?? uriStart));
        }

        public async Task<HtmlDocument> ReadPage(string uri)
        {
            string codePage = string.Empty;
            var webRequest = PrepareHtmlRequest(uri);
            using (var response = (HttpWebResponse) await webRequest.GetResponseAsync())
            {
                codePage = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Singleton.Instance.HistoryUri.Add(response.ResponseUri.ToString());
                Singleton.Instance.Cookies = response.Cookies;
                operationData.LoadHtml(codePage);
            };
            if (_enumWebsite == 0)
            {
                 if (operationData.CheckAcceptTerms())
                     return await ReadWeb(Singleton.Instance.HistoryUri[Singleton.Instance.HistoryUri.Count - 1]);
                 else
                    return operationData.Html;
            }
            else
                return operationData.Html;
        }

        private HttpWebRequest PrepareHtmlRequest(string uri)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            if (_enumWebsite == 0)
            {
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
                if (Singleton.Instance.Cookies.Count > 0)
                    webRequest.CookieContainer.Add(Singleton.Instance.Cookies);
            }
            else
                webRequest.ContentLength = 0;
            return webRequest;
        }
    }
}
