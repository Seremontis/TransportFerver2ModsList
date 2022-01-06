using HtmlAgilityPack;
using Importer;
using Importer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImporterSteam.Logic
{
    public class DataLoaderSteam : IDataLoader
    {
        public Task<List<Mod>> GetFile(string path)
        {
            throw new NotImplementedException();
        }

        public Task<HtmlDocument> GetHtml(Uri uri = null)
        {
            throw new NotImplementedException();
        }
    }
}
