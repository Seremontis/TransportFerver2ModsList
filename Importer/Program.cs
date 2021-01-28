using HtmlAgilityPack;
using System;

namespace Importer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            WebsiteLoader websiteLoader = new WebsiteLoader();
            WebsiteManipulation websiteManipulation = new WebsiteManipulation();
            HtmlDocument doc=websiteLoader.GetHtml();
            websiteManipulation.GetUris(doc);
        }
    }
}
