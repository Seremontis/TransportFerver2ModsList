using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Importer
{
    public interface IDataLoader
    {
        public Task<HtmlDocument> GetHtml(Uri uri=null);
        public Task<List<ModItem>> GetFile(string path);
    }
}
