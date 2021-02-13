using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Importer
{
    public interface IWebsiteLoader
    {
        public Task<HtmlDocument> GetHtml(Uri uri=null);
        public HtmlDocument GetHtml(Uri uri, bool flagOverride);
    }
}
