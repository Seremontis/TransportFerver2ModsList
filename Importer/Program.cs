using System;

namespace Importer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            WebsiteLoader websiteLoader = new WebsiteLoader();
            websiteLoader.GetHtml("https://www.transportfever.net/filebase/index.php?filebase/162-transportmittel/");
        }
    }
}
