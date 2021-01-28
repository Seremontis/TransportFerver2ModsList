using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Importer
{
    interface IWebsiteLoader
    {
        public HtmlDocument GetHtml(string uri);
        public HtmlDocument GetHtml(string uri, bool flagOverride);
    }
}
