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
            BuilderData builderData = new BuilderData(new WebsiteLoader(), new WebsiteManipulation());
            await builderData.CreateJson();
            Console.WriteLine("Import Complete");
        }
    }
}
