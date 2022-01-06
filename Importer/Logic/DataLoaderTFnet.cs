using HtmlAgilityPack;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Importer.Interface;

namespace Importer.Logic
{
    public class DataLoaderTFnet :  DataLoader,IDataLoader
    {
        private HttpWebRequest _webRequest;
        private readonly Uri _uriStart = new Uri(@"https://www.transportfever.net/filebase/index.php?filebase/80-transport-fever-2/");
        private CookieContainer _cookies;
        public async Task<HtmlDocument> GetHtmlAsync(Uri uri = null)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            PrepareRequest(uri == null ? _uriStart : new Uri(uri.AbsoluteUri));

            using (HttpWebResponse response = (HttpWebResponse)await _webRequest.GetResponseAsync())
            {
                string codePage = new StreamReader(response.GetResponseStream()).ReadToEnd();
                htmlDocument.LoadHtml(codePage);
                ReadCookies(response);
            }

            return htmlDocument;
        }

        private void PrepareRequest(Uri uri)
        {
            _webRequest = (HttpWebRequest)WebRequest.Create(uri);
            _webRequest.AllowAutoRedirect = false;
            if (uri.AbsoluteUri != _uriStart.AbsoluteUri)
                _webRequest.Method = "Get";
            else
                _webRequest.Method = "POST";
            _webRequest.ContentType = "application/x-www-form-urlencoded";
            WebHeaderCollection myWebHeaderCollection = _webRequest.Headers;
            myWebHeaderCollection.Add("Accept-Language", "en;q=0.8");
            if (_cookies != null)
                _webRequest.CookieContainer = _cookies;
            else
                _webRequest.CookieContainer = null;
            _webRequest.ContentLength = 0;
        }

        private void ReadCookies(HttpWebResponse response)
        {
            if (response.Cookies.Count() > 0)
            {
                _cookies = new CookieContainer();
                foreach (Cookie item in response.Cookies.ToList())
                    _cookies.Add(item);
            }
            else
            {
                Regex regex = new Regex(@"__cfduid=(?<key>.*?);.*domain=(?<domain>.*?);");
                if (response.Headers.AllKeys.Contains("Set-Cookie"))
                {
                    Match m = regex.Match(response.Headers["Set-Cookie"]);
                    if (m.Success)
                    {
                        _cookies = new CookieContainer();
                        Cookie cookie = new Cookie
                        {
                            Name = "__cfduid",
                            Value = m.Groups["key"].Value,
                            Domain = m.Groups["domain"].Value
                        };
                        _cookies.Add(cookie);
                    }

                }

            }
        }
    }
}
