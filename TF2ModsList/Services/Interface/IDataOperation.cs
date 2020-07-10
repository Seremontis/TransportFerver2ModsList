using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Models;

namespace TF2ModsList.Services.Interface
{
    public interface IDataOperation
    {
        HtmlDocument Html { get; set; }
        void LoadHtml(string responseString);
        bool CheckAcceptTerms();
        byte[] PreparePostData(); 
    }
}
