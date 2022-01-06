using HtmlAgilityPack;
using Importer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importer.Logic
{
    public class DataLoaderSteam: DataLoader, IDataLoader
    {
        private readonly Uri _uriStart = new Uri(@"https://steamcommunity.com/workshop/browse/?appid=1066780&browsesort=mostrecent&section=readytouseitems&actualsort=mostrecent&");
        public async Task<HtmlDocument> GetHtmlAsync(Uri uri = null)
        {
            HtmlWeb _web = new HtmlWeb();
            return await _web.LoadFromWebAsync(uri == null ? _uriStart.AbsoluteUri : uri.AbsoluteUri);
        }
    }
}
