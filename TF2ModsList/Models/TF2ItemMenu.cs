using System;
using System.Collections.Generic;
using System.Text;

namespace TF2ModsList.Models
{
    public class TF2ItemMenu
    {
        public string NameItem { get; set; }
        public string Path { get; set; }
        public List<TF2ItemMenu> SubItems { get; set; }
    }
}
