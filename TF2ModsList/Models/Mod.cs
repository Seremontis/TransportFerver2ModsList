using System;
using System.Collections.Generic;
using System.Text;

namespace TF2ModsList.Models
{
    public class Mod
    {
        public string Name { get; set; }
        public Uri UriPicture { get; set; }
        public Uri UriPage { get; set; }
        public string Author { get; set; }
        public string DataCreate { get; set; }
        public string DataUpdate { get; set; }

        public string ReturnDataRange
        {

            get
            {
                return string.Format("{0}, {1} - {2}", Author, DataCreate, DataUpdate);
            }
        }
    }
}
