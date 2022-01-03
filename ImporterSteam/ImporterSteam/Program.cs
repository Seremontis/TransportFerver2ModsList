using ImporterSteam.Logic;
using System;

namespace ImporterSteam
{
    class Program
    {
        static void Main(string[] args)
        {
            SteamWebsiteLoader steamWebsiteLoader = new SteamWebsiteLoader();
            steamWebsiteLoader.TestConnect();
            Console.WriteLine("Hello World!");
        }
    }
}
