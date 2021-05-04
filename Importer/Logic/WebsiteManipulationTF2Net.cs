using HtmlAgilityPack;
using Importer.Model;
using System;
using System.Collections.Generic;

namespace Importer
{
    public class WebsiteManipulationTF2Net : IWebsiteManipulation
    {
        public List<Website> GetUrisCategory(HtmlDocument htmlDocumet)
        {
            HtmlNodeCollection nodes = htmlDocumet.DocumentNode.SelectNodes("//ul[@class='inlineList filebaseSubCategories']");
            List<Website> pairs = new List<Website>();
            foreach (HtmlNode node in nodes)
                foreach (HtmlNode item in node.SelectNodes(".//a"))
                    pairs.Add(new Website() {ParentCategory=node.ParentNode.ChildNodes.FindFirst("div").InnerText.Trim() ,Category = item.InnerText, UriPage = new Uri(item.Attributes["href"].Value) });
            return pairs;
        }
        public List<Uri> GetPageList(HtmlDocument htmlDocument)
        {
            List<Uri> pairs = new List<Uri>();
            HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode(".//nav[@class='pagination']");
            if (node != null)
                if (node.SelectNodes(".//a") != null)
                    foreach (HtmlNode item in node.SelectNodes(".//a"))
                        if (int.TryParse(item.InnerText, out int number))
                            pairs.Add(new Uri(item.Attributes["href"].Value.Replace("&amp;", "&")));
            return pairs;
        }
        public List<Mod> SearchItems(string category, HtmlDocument htmlDocument)
        {
            HtmlNode doc = htmlDocument.DocumentNode;
            List<Mod> mods = new List<Mod>();
            foreach (HtmlNode item in doc.SelectNodes(@"//table[@class='table']/tbody/tr"))
            {

                Mod modItem = new Mod()
                {
                    Category = category,
                    Title = item.SelectSingleNode(@".//h3/a")?.InnerText,
                    WebsiteSource = new Uri(item.SelectSingleNode(@".//h3/a").Attributes["href"].Value),
                    Id = int.Parse(item.SelectSingleNode(@".//h3/a").Attributes["data-entry-id"].Value),
                    Image = new Uri(item.SelectSingleNode(@".//img").Attributes["src"].Value),
                    Author = item.SelectSingleNode(@".//small/a[@class='userLink']")?.InnerText,
                    NameWebsite=EnumWebsite.transportfeverNet,
                    StateFile=EnumStateFile.New
                };
                HtmlNodeCollection times = item.SelectNodes(@".//small/time");
                if (times.Count > 1)
                {
                    modItem.CreateData = DateTime.Parse(times[0].Attributes["datetime"].Value);
                    modItem.UpdateData = DateTime.Parse(times[1].Attributes["datetime"].Value);
                }
                else
                    modItem.CreateData = DateTime.Parse(times[0].Attributes["datetime"].Value);
                mods.Add(modItem);
            };

            return mods;
        }
        public List<SchemaCategory> CreateStrucuteCategory(HtmlDocument htmlDocument)
        {
            HtmlNode doc = htmlDocument.DocumentNode;
            List<SchemaCategory> mods = new List<SchemaCategory>();
            foreach (HtmlNode item in doc.SelectNodes(@"//table[@class='table']/tbody/tr"))
            {

                SchemaCategory modItem = new SchemaCategory()
                {
                    ParentElement = null,
                    ChildElement = null,
                };
                mods.Add(modItem);
            };

            return mods;
        }
    }
}
