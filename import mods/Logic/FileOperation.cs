using HtmlAgilityPack;
using ImporterMods.Logic;
using ImporterMods.Model;
using System.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImporterMods.Logic
{
    public class FileOperation
    {
        public List<string> extraItems { get; private set; }
        private Regex regex = new Regex(@"id=(?<id>\d+)");
        private Regex regexextract = new Regex(@"(?<id>\d+)_");
        private HtmlWeb web = new HtmlWeb();
        private string path = ConfigurationManager.AppSettings.Get("DefaultPathUnzip");
        private string idgame = ConfigurationManager.AppSettings.Get("DefaultIdGame");

        public FileOperation()
        {
            extraItems = new List<string>();
        }

        public void CheckCorretionFile(FileOperation FileOperation, FileData fileData)
        {
            foreach (string item in fileData.list.Except(fileData.current.Select(x => x)))
                FileOperation.Run(item, fileData);
            for (int i = 0; i < extraItems.Count; i++)
                FileOperation.Run(extraItems[i], fileData);

            string fileName = "setup.json";
            string jsonString = JsonConvert.SerializeObject(fileData.setup);
            File.WriteAllText(fileName, jsonString);

            fileData.CreateText(ListType.steamdownloader);
            fileData.CreateText(ListType.current);
        }

        public void Extract(Result result, string[] listfiles, ref List<string> current)
        {
            var x = listfiles.Where(x => x.Contains(result.Id)).FirstOrDefault();
            if (x != null)
            {
                Console.WriteLine($"Rozpakowywanie id: {result.Id}");
                string pathunzip = $"./pliki/{result.TypeMod}/{result.Id}";
                if (!Directory.Exists(pathunzip))
                    Directory.CreateDirectory(pathunzip);

                System.IO.Compression.ZipFile.ExtractToDirectory(x.Replace("\\", "/"), pathunzip);

                current.Add(result.Id);
            }
            else
            {

                /*if (File.Exists("setup.json"))
                {*/
                using (StreamWriter sw = File.CreateText("error.txt"))
                    sw.WriteLine(result.Id + DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss"));
                //}
                Console.WriteLine($"Brak pliku {result.Id}");
            }

        }

        public void RenameSteammods()
        {
            Regex regex = new Regex(@"\\\d+$");
            Regex regex1 = new Regex("name\\s*=\\s*_\\(\\s*\"\\s*(?<val>.*?)\"\\s*\\)");
            Regex regex2 = new Regex("\\[\"((name_mod)|(mod_name)|(modname)|(name\\s*mod)|(mod\\s*name)|(name)|(info_name))\"\\]\\s*=\\s*\"(?<val>.*?)\"", RegexOptions.IgnoreCase);
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                if (regex.IsMatch(dir) && File.Exists(dir + "\\mod.lua"))
                {
                    string modfilevalue = File.ReadAllText(dir + "\\mod.lua");
                    Match match = regex1.Match(modfilevalue);
                    if (match.Success)
                        if (new[] { "name_mod", "mod_name", "modname", "name mod", "mod name", "name", "info_name" }.Contains(match.Groups["val"].Value.ToLower()))
                        {
                            string strings = File.ReadAllText(dir + "\\strings.lua");
                            string stringSearch = File.ReadAllText(dir + "\\strings.lua");
                            Match matchs = regex2.Match(stringSearch);
                            if (!matchs.Success)
                                Directory.Move(dir, dir + "_1");

                        }
                        else
                            Directory.Move(dir, path + "\\" + Replacer(match.Groups["val"].Value));
                }
            }

        }

        public void CorrectNameMod()
        {
            string[] dirs = Directory.GetDirectories(path);
            Regex regex = new Regex(@"_\d+$");
            foreach (string dir in dirs)
            {
                Match match = regex.Match(dir);
                if (!match.Success)
                    Directory.Move(dir, dir + "_1");
            }
        }

        public void WriteScript(FileData fileData)
        {
            if (!Directory.Exists("./skrypty"))
                Directory.CreateDirectory("./skrypty");
            using (StreamWriter sw = File.CreateText($"./skrypty/skrypt-{DateTime.Now.ToString("yyyyMMddhhmmss")}.txt"))
            {
                sw.WriteLine("login anonymous");
                foreach (var item in fileData.steamdownloader)
                {
                    if (!string.IsNullOrEmpty(item))
                        sw.WriteLine($"workshop_download_item {idgame} {item}");
                }
            }
        }

        private string Replacer(string val)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add(@"\", "(");
            dict.Add(@"/", "(");
            dict.Add(@"&", "and");
            dict.Add(@":", " ");

            foreach (var item in dict)
            {
                val = val.Replace(item.Key, item.Value);
            }

            return val;
        }

        private void Run(string id,FileData fileData)
        {
            Console.WriteLine($"Rozpoczęcie procedury dla id: {id}");
            var url = $"https://steamcommunity.com/sharedfiles/filedetails/?id={id}";
            var doc = web.Load(url);
            var collection = doc.DocumentNode.SelectNodes("//div[@class='workshopItem']//a");
            if (collection?.Count > 0)
            {
                FilteredIds(collection, fileData);
                Console.WriteLine($"Znaleziono dodatkowe elementy: {collection.Count} ");
            }
            else
            {
                string typeMods = doc.DocumentNode.SelectSingleNode("//span[@class='workshopTagsTitle']")?.InnerText?.Split(":")[0];

                var requiredItems = doc.DocumentNode.SelectNodes("//div[@class='requiredItemsContainer']//a");

                if (requiredItems?.Count > 0)
                {
                    FilteredIds(requiredItems,fileData);
                    Console.WriteLine($"Wymagane elementy: {requiredItems.Count}");
                }
                if (string.IsNullOrEmpty(typeMods))
                {
                    using (StreamWriter sw = File.CreateText("error.txt"))
                        sw.WriteLine(id + " " + DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss") + "brak typu");
                    Console.WriteLine($"{id} > brak typu, sprawdź czy poprawny");
                    typeMods = "Mods";
                }
                else if (typeMods.ToLower().Contains("tags"))
                    typeMods = "Mods";

                else if (typeMods.ToLower().Contains("misc"))
                    typeMods = "ColorCorrections";

                if (fileData.current.All(x => x != id))
                {
                    fileData.current.Add(id);
                    //setup.Add(new Result() { Id = id, TypeMod = typeMods });
                    fileData.steamdownloader.Add(id);
                }
            }
        }

        private void FilteredIds(HtmlNodeCollection collection, FileData fileData)
        {
            foreach (var item in collection)
            {
                string href = item.Attributes["href"].Value;
                Match match = regex.Match(href);
                if (match.Success)
                {
                    string hrefID = match.Groups["id"].Value;
                    if (!fileData.list.Contains(hrefID) && !fileData.current.Contains(hrefID) && !extraItems.Contains(hrefID))
                        extraItems.Add(hrefID);
                }
            }
        }


    }
}
