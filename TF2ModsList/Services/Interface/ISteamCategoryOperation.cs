using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace TF2ModsList.Services.Interface
{
    public interface ISteamCategoryOperation
    {
        HtmlDocument Html { get; set; }
        public List<string> GetCategoryOptions();
    }
}
