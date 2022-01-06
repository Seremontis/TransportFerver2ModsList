using Importer;
using Importer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImporterSteam
{
    public class BuilderDataSteam : IBuilderData
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private IDataLoader dataLoader;
        private IWebsiteManipulation websiteManipulation;
        public JsonModsFile NewModItems = new JsonModsFile();
        public JsonModsFile OldModItems = new JsonModsFile();
        private object locker = new object();

        public BuilderDataSteam(IDataLoader dataLoader,IWebsiteManipulation websiteManipulation)
        {
            this.dataLoader = dataLoader;
            this.websiteManipulation = websiteManipulation;
        }
        public Task CreateJson(string localPath = null, bool update = false)
        {
            throw new NotImplementedException();
        }
    }
}
