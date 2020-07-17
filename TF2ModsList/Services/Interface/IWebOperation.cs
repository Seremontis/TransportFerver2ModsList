using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Models;

namespace TF2ModsList.Services.Interface
{
    public interface IWebOperation
    {
        Task<HtmlDocument> ReadWeb(string uri = null,EnumWebsite enumWebsite=0);
    }
}
