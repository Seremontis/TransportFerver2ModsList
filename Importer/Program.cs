using System.Configuration;
using System.Collections.Specialized;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NLog;
using System.IO;

namespace Importer
{
    class Program
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        static async Task Main(string[] args)
        {
            try
            {
                CheckFolder();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            await StartRun();
        }

        private static async Task StartRun()
        {
            try
            {
                _logger.Info("Start program");
                BuilderData builderData = new BuilderData(new DataLoader(), new WebsiteManipulationTF2Net());
                await builderData.CreateJson(ConfigurationManager.AppSettings.Get("DefaultFolderMod"));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occured");
            }
            finally
            {
                _logger.Info("Ending program");
                LogManager.Shutdown();
            }
        }
        private static void CheckFolder()
        {
            if (ConfigurationManager.AppSettings.Get("DefaultFolderLog").Trim() != LogManager.Configuration.Variables["basedir"].Text.Trim())
                LogManager.Configuration.Variables["logfile"] = ConfigurationManager.AppSettings.Get("DefaultDefaultFolderLogFolder");
            if (!Directory.Exists(ConfigurationManager.AppSettings.Get("DefaultFolderLog")))
                Directory.CreateDirectory(ConfigurationManager.AppSettings.Get("DefaultFolderLog"));
            if(!Directory.Exists(ConfigurationManager.AppSettings.Get("DefaultFolderMod")))
                Directory.CreateDirectory(ConfigurationManager.AppSettings.Get("DefaultFolderMod"));
        }

    }
}
