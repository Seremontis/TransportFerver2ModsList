using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TF2ModsList.Models
{
    public class DataApp:IDataApp
    {
        private List<string> _historyUri = new List<string>();
        private CookieCollection _cookies = new CookieCollection();

        public List<string> HistoryUri
        {
            get { return _historyUri; }
        }
        public CookieCollection Cookies
        {
            get { return _cookies; }
            set { _cookies = value; }
        }

        public void DeleteOfHistory(string uri)
        {
            _historyUri.Remove(uri);
        }
    }
}
