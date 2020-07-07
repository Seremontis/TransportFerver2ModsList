using System;
using System.Collections.Generic;
using System.Text;

namespace TF2ModsList.Models
{
    public class DetailMod
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri MainPicture { get; set; }
        public List<Uri> ListPictures { get; set; }
        public Dictionary<string, Uri> VersionFiles { get; set; }
        public string SteamId { get; set; }
        public List<string> Authors { get; set; }
        public string SpecificationMod { get; set; }
        //Odnośnik do strony źródłowej
        //public string Source { get; set; }

        public string ReturnAuthors
        {
            get 
            {
                string word = string.Empty;
                foreach (var item in Authors)
                {
                    if (!string.IsNullOrEmpty(word))
                        word += "<br/>" + item;
                    else
                        word += item;
                }
                return word;
            }
        }
    }
}
