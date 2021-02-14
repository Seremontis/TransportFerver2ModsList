using HtmlAgilityPack;
using System;
using System.Threading.Tasks;

namespace Importer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BuilderData builderData = new BuilderData(new DataLoader(), new WebsiteManipulationTF2Net());
            await builderData.CreateJson(@"C:\Users\komp2\Documents\GitHub\TransportFerver2ModsList\Importer\bin\Debug\net5.0\ListMods_14-02-2021.json",true);
            Console.WriteLine("Import Complete");
        }
    }
}
