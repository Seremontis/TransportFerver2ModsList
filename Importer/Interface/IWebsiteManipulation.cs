using HtmlAgilityPack;
using Importer.Model;
using System;
using System.Collections.Generic;

namespace Importer.Interface
{
    public interface IWebsiteManipulation
    {
        IEnumerable<Website> GetUrisCategory(HtmlDocument htmlDocumet);
        public List<Uri> GetPageList(HtmlDocument htmlDocument,Uri uri);
        public List<Mod> SearchItems(string category, HtmlDocument htmlDocument);
    }
}