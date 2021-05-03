using Importer.Model;
using System;
using System.Collections.Generic;

namespace Importer
{
    public class JsonModsFile
    {
        public List<Mod> Mods { get; set; }
        public List<SchemaCategory> CategoriesMap { get; set; }

        public JsonModsFile()
        {
            Mods = new List<Mod>();
            CategoriesMap = new List<SchemaCategory>();
        }
    }
    public class Mod
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public DateTime CreateData { get; set; }
        public DateTime UpdateData { get; set; }
        public Uri WebsiteSource { get; set; }
        public Uri Image { get; set; }
        public int Id { get; set; }
        public EnumStateFile StateFile { get; set; }
        public EnumWebsite NameWebsite { get; set; }
    }

    public class SchemaCategory
    {
        public string ParentElement { get; set; }
        public string ChildElement { get; set; }
    }
}