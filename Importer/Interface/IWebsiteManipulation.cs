using HtmlAgilityPack;
using Importer.Model;
using System;
using System.Collections.Generic;

namespace Importer
{
    public interface IWebsiteManipulation
    {
        List<Website> GetUrisCategory(HtmlDocument htmlDocumet);
        public List<Uri> GetPageList(HtmlDocument htmlDocument);
        public List<ModItem> SearchItems(string category, HtmlDocument htmlDocument);
    }
}