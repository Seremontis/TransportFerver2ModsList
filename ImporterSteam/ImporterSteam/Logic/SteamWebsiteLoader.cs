using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Importer;

namespace ImporterSteam.Logic
{
    public class SteamWebsiteLoader
    {
        private string _defaultUrl = "https://steamcommunity.com/workshop/browse/?appid=1066780&browsesort=mostrecent&section=readytouseitems&actualsort=mostrecent&";
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
            for (int i = 2; i < _maxPageNumber; i++)
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
    }
}
