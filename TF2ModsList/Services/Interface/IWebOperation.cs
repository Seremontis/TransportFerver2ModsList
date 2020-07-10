using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TF2ModsList.Services.Interface
{
    public interface IWebOperation
    {
        Task<HtmlDocument> ReadWeb(string uri = null);
    }
}
