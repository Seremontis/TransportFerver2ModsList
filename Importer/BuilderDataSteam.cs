using HtmlAgilityPack;
using Importer.Interface;
using Importer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Importer
{
    public class BuilderDataSteam: BuilderData, IBuilderData
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public BuilderDataSteam(IDataLoader dataLoader,IWebsiteManipulation websiteManipulation,EnumWebsite website): base (dataLoader, websiteManipulation, website)
        {
        }


    }
}
