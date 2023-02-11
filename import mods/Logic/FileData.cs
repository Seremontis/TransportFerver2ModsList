using ImporterMods.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImporterMods.Logic
{
    public class FileData
    {

        public List<string> list { get;set; }
        public List<string> current { get; set; }
        public List<string> steamdownloader { get; set; }

        public List<Result> setup { get; private set; }

        public FileData()
        {
            list = new List<string>();
            current = new List<string>();
            steamdownloader = new List<string>();
            setup = new List<Result>();
            LoadList();
            LoadCurrentList();
            LoadSteamDownoladerFile();
            LoadSetup();
        }

        public void CreateText(ListType type)
        {
            List<string> tmp=new List<string>();
            switch (type)
            {   
                case ListType.list:
                    tmp = list;
                    break;
                case ListType.current:
                    tmp=current;
                    break;
                case ListType.steamdownloader:
                    tmp = steamdownloader;
                    break;
                default:
                    break;
            }
            if (!File.Exists("current.txt"))
                File.Create("current.txt");
            using (StreamWriter sw = File.CreateText($"{type.ToString()}.txt"))
            {
                foreach (var item in tmp)
                    sw.WriteLine(item);
            }
        }

        private void LoadSetup()
        {
            if (File.Exists("setup.json"))
            {
                using (StreamReader r = new StreamReader("setup.json"))
                {
                    string json = r.ReadToEnd();
                    if (!string.IsNullOrEmpty(json))
                        setup = JsonConvert.DeserializeObject<List<Result>>(json);
                }
            }
        }

        private void LoadSteamDownoladerFile()
        {
            if (File.Exists("steamdownoloader.json"))
            {
                using (StreamReader sr = File.OpenText("steamdownoloader.txt"))
                {
                    steamdownloader = sr.ReadToEnd().Split(new char[] { '\n', ',' }).Select(x => x.Replace("\r", string.Empty)).ToList();
                }
            }
        }

        private void LoadCurrentList()
        {
            if (!File.Exists("current.txt"))
                File.Create("current.txt");
            else
            {
                using (StreamReader sr = File.OpenText("current.txt"))
                {
                    current = sr.ReadToEnd().Split(new char[] { '\n', ',' }).Select(x => x.Replace("\r", string.Empty)).ToList();
                    if (current.Count > 0)
                        if (string.IsNullOrEmpty(current[current.Count - 1]))
                            current.RemoveAt(current.Count - 1);
                }
            }
        }

        private void LoadList()
        {
            if (!File.Exists("nowe.txt"))
                File.Create("nowe.txt");
            else{
                using (StreamReader sr = File.OpenText("nowe.txt"))
                {
                    list = sr.ReadToEnd().Split(new char[] { '\n', ',' }).Select(x => x.Replace("\r", string.Empty)).ToList();
                }
            }
        }
    }
}
