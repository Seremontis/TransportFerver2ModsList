using HtmlAgilityPack;
using Importer.Interface;
using Importer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Importer.Logic
{
    public class WebsiteManipulationSteam : IWebsiteManipulation
    {
        private string _firstPartUrl = "https://steamcommunity.com/workshop/browse/?appid=1066780&searchtext=&childpublishedfileid=0&browsesort=mostrecent&section=readytouseitems&requiredtags%5B%5D=";
        private string _secondPartUrl = "&created_date_range_filter_start=0&created_date_range_filter_end=0&updated_date_range_filter_start=0&updated_date_range_filter_end=0";
        private List<string> _NotDownloadCategoryList = new List<string>() { "Temperate", "Dry", "Tropical", "Europe", "USA", "Asia", "incompatibleCheckbox" };
        private string _pageParams = "p=";
        private Regex exId = new Regex(@"id=(?<value>\d+)");

        public IEnumerable<Website> GetUrisCategory(HtmlDocument htmlDocumet)
        {
            HtmlNode htmlnode = htmlDocumet.DocumentNode;
            foreach (string item in htmlnode.SelectNodes("//div[@class='filterOption']/label/input").Select(x => x.Attributes["value"].Value))
            {
                if (!_NotDownloadCategoryList.Contains(item))
                {
                    yield return  new Website()
                    {
                        UriPage = new Uri(string.Format("{0}{1}{2}", _firstPartUrl, item.Replace(" ", "+"), _secondPartUrl)),
                        Category = item
                    };
                }
            }
        }

        public List<Uri> GetPageList(HtmlDocument htmlDocument,Uri uri)
        {
            List<Uri> list = new List<Uri>();
            
            int maxPageNumber = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='workshopBrowsePagingControls']").ChildNodes.Select(x =>
            {
                int res;
                int.TryParse(x.InnerText, out res);
                return res;
            })?.Max()??1;
            if (maxPageNumber <= 1 && uri != null)
                list.Add(uri);       
            else
            {
                string uriString = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='workshopBrowsePagingControls']/a").Attributes["href"].Value;
                for (int i = 1; i <= maxPageNumber; i++)
                    list.Add(new Uri(Regex.Replace(
                        uriString, _pageParams + @"\d+",string.Format("{0}{1}",_pageParams,i))));
            }
            return list;
        }

        public List<Mod> SearchItems(string category, HtmlDocument htmlDocument)
        {
            HtmlNode doc = htmlDocument.DocumentNode;
            List<Mod> mods = new List<Mod>();
            foreach (HtmlNode item in doc.SelectNodes("//div[@class='workshopItem']"))
            {
                Mod modItem = new Mod()
                {
                    Category = category,
                    Title = item.SelectSingleNode(@".//a/div[contains(@class,'workshopItemTitle')]").InnerText,
                    WebsiteSource = new Uri(item.SelectSingleNode(@".//a").Attributes["href"].Value),
                    Image = new Uri(item.SelectSingleNode(@".//a/div/img[@class='workshopItemPreviewImage ']").Attributes["src"].Value),
                    Author = item.SelectSingleNode(@".//div[contains(@class,'workshopItemAuthorName')]/a").InnerText,
                    NameWebsite = EnumWebsite.Steam,
                    StateFile = EnumStateFile.New
                };
                modItem.Id = long.Parse(exId.Match(modItem.WebsiteSource.AbsoluteUri).Groups["value"].Value);
                mods.Add(modItem);
            };

            return mods;
        }

    }
}
