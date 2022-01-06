using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Importer.Interface
{
    public interface IDataLoader
    {
        public Task<HtmlDocument> GetHtmlAsync(Uri uri=null);
        public Task<List<Mod>> GetFileAsync(string path);
    }
}
