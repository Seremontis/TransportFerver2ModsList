using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TF2ModsList.Services
{
    public interface IWebOperation
    {
        HtmlDocument ReadWeb(string uri=null);
    }
}
