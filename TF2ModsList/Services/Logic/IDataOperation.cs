using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TF2ModsList.Models;

namespace TF2ModsList.Services
{
    public interface IDataOperation
    {
        HtmlDocument Html { get; set; }
        void LoadHtml(string responseString);
        bool CheckAcceptTerms();
        byte[] PreparePostData();
        ObservableCollection<TF2ItemMenu> ReturnMainMenuTF2();
        ObservableCollection<Mod> ReturnModsItem();
        DetailMod ReturnDetailModTF2();
    }
}
