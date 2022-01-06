using HtmlAgilityPack;
using Importer.Interface;
using Importer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Importer
{
    public class BuilderDataTFnet: BuilderData, IBuilderData
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public BuilderDataTFnet(IDataLoader websiteLoader, IWebsiteManipulation websiteManipulation,EnumWebsite website) : base(websiteLoader, websiteManipulation, website)
        {         
        }

        public async Task CreateJson(string localPath = null, bool update = false)
        {
            //if (string.IsNullOrEmpty(localPath))
            //    localPath = Directory.GetCurrentDirectory() + $"\\ListModsTFnet_{DateTime.Now.ToString("dd-MM-yyyy")}.json";
            //else
            //    localPath += $"\\ListModsTFnet_{DateTime.Now.ToString("dd-MM-yyyy")}.json";
            //await base.PrepareDataBeforeSave(localPath, update);
            //string json = JsonSerializer.Serialize(NewModItems);
            //_logger.Info("TransportFeverNet mods count:" + NewModItems.Mods.Count);
            //File.WriteAllText(localPath, json);
            //_logger.Info("Save file complete");
        }

    }
}
