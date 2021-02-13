using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Importer
{
    public class WebsiteLoader : IWebsiteLoader
    {
        private HttpWebRequest webRequest;
        private readonly Uri uriStart = new Uri(@"https://www.transportfever.net/filebase/index.php?filebase/80-transport-fever-2/");
        private CookieContainer cookies;
        private readonly string englishUri = "&l=2";
        public async Task<HtmlDocument> GetHtml(Uri uri = null)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            PrepareRequest(uri == null ? uriStart : new Uri(uri.AbsoluteUri));

            using (HttpWebResponse response = (HttpWebResponse)await webRequest.GetResponseAsync())
            {
                string codePage = new StreamReader(response.GetResponseStream()).ReadToEnd();
                htmlDocument.LoadHtml(codePage);
                ReadCookies(response);
            }

            return htmlDocument;
        }
        public HtmlDocument GetHtml(Uri uri, bool flagOverride)
        {
            if (flagOverride)
            {

            }
            else
            {

            }
            return null;
        }

        private void PrepareRequest(Uri uri)
        {
            webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.AllowAutoRedirect = false;
            if (uri.AbsoluteUri != uriStart.AbsoluteUri)
                webRequest.Method = "Get";
            else
                webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            WebHeaderCollection myWebHeaderCollection = webRequest.Headers;
            myWebHeaderCollection.Add("Accept-Language", "en;q=0.8");
            if (cookies != null)
                webRequest.CookieContainer = cookies;
            else
                webRequest.CookieContainer = null;
            webRequest.ContentLength = 0;
        }

        private void ReadCookies(HttpWebResponse response)
        {
            if (response.Cookies.Count() > 0)
            {
                cookies = new CookieContainer();
                foreach (Cookie item in response.Cookies.ToList())
                    cookies.Add(item);
            }
            else
            {
                Regex regex = new Regex(@"__cfduid=(?<key>.*?);.*domain=(?<domain>.*?);");
                if (response.Headers.AllKeys.Contains("Set-Cookie"))
                {
                    Match m = regex.Match(response.Headers["Set-Cookie"]);
                    if (m.Success)
                    {
                        cookies = new CookieContainer();
                        Cookie cookie = new Cookie
                        {
                            Name = "__cfduid",
                            Value = m.Groups["key"].Value,
                            Domain = m.Groups["domain"].Value
                        };
                        cookies.Add(cookie);
                    }

                }

            }
        }
    }
}
