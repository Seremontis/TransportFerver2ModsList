using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importer.Model
{
    public class Website
    {
        public Uri UriPage { get; set; }
        public string Category{ get; set; }
        public string ParentCategory{ get; set; }
    }
}
