using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Importer;
using Importer.Interface;
using Importer.Model;

namespace ImporterSteam.Logic
{
    public class WebsiteManipulationSteam: IWebsiteManipulation
    {
        private string _defaultUrl = "https://steamcommunity.com/workshop/browse/?appid=1066780&browsesort=mostrecent&section=readytouseitems&actualsort=mostrecent&";
        private string _firstPartUrl = "https://steamcommunity.com/workshop/browse/?appid=1066780&searchtext=&childpublishedfileid=0&browsesort=mostrecent&section=readytouseitems&requiredtags%5B%5D=Train+Depot";
        private string secondPartUrl= "&created_date_range_filter_start=0&created_date_range_filter_end=0&updated_date_range_filter_start=0&updated_date_range_filter_end=0";
        private List<string> _listParams = new List<string>();
        private List<string> _NotDownloadCategoryList = new List<string>();
        private string _pageParams="p=";
        private int _maxPageNumber = 1;
        private List<Mod> _mods = new List<Mod>();
        private HtmlWeb _web = new HtmlWeb();
        private HtmlNode _html;

        public void TestConnect()
        {
            LoadSite(_maxPageNumber);
            GetMaxPage();
            FillListMods();
            for (int i = _maxPageNumber; i > 0; i--)
            {
                LoadSite(i);
                FillListMods();
            }
        }


        private void LoadSite(int number) =>  _html = _web.Load(_defaultUrl + _pageParams + number)?.DocumentNode;

        private void GetMaxPage()
        {
            _maxPageNumber = _html.SelectSingleNode("//div[@class='workshopBrowsePagingControls']").ChildNodes.Select(x =>
                {
                    int res;
                    int.TryParse(x.InnerText, out res);
                    return res;
                }).Max();
        }

        private void FillListMods()
        {
            var t = _html;
        }

        public List<Website> GetUrisCategory(HtmlDocument htmlDocumet)
        {
            throw new NotImplementedException();
        }

        public List<Uri> GetPageList(HtmlDocument htmlDocument)
        {
            throw new NotImplementedException();
        }

        public List<Mod> SearchItems(string category, HtmlDocument htmlDocument)
        {
            throw new NotImplementedException();
        }
    }
}
