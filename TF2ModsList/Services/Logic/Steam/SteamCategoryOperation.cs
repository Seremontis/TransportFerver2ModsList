using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using TF2ModsList.Services.Interface;

namespace TF2ModsList.Services.Logic
{
    public class SteamCategoryOperation:ISteamCategoryOperation
    {
        #region fields
        protected HtmlDocument _Html;
        protected HtmlNode _selectedNode;
        #endregion

        #region props
        public HtmlDocument Html
        {
            get { return _Html; }
            set { _Html = value; }
        }

        public List<string> GetCategoryOptions()
        {
            List<string> vs = new List<string>();
            var x = _Html.DocumentNode.SelectNodes(".//div[@class='filterOption']");
            foreach (var item in x)
            {
                vs.Add(item.SelectSingleNode(".//label").InnerText);
            }
            return vs;
        }
        #endregion

    }
}
