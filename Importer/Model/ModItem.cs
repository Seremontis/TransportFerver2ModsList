using Importer.Model;
using System;

namespace Importer
{
    public class ModItem
    {
        public string Title{ get; set; }
        public string Author{ get; set; }
        public string Category { get; set; }
        public DateTime CreateData { get; set; }
        public DateTime UpdateData { get; set; }
        public Uri WebsiteSource { get; set; }
        public Uri Image { get; set; }
        public int Id { get; set; }
        public EnumStateFile StateFile { get; set; }
        public EnumWebsite NameWebsite { get; set; }
    }
}