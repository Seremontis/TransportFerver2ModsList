using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using TF2ModsList.Models;
using TF2ModsList.Services.Interface;

namespace TF2ModsList.Services
{


    public class DataOperation : IDataOperation
    {
        #region fields
        protected HtmlDocument _Html;
        protected HtmlNode _selectedNode;
        protected Dictionary<string, string> Pairs = new Dictionary<string, string>();
        protected readonly string defaultPath = "html/body/div[@id='pageContainer']/section[@id='main']/div/div";
        #endregion

        #region public method

        public HtmlDocument Html
        {
            get { return _Html; }
            set { _Html = value; }
        }

        public void LoadHtml(string responseString)
        {
            _Html = _Html ?? new HtmlDocument();
            _Html.LoadHtml(responseString);
        }

        public bool CheckAcceptTerms()
        {
            if (_Html == null)
                throw new Exception("Brak HtmlDocument");
            else
            {
                _selectedNode = _Html.DocumentNode.SelectSingleNode(defaultPath + "/form/div[@class='formSubmit']");
                if (_selectedNode is null)
                    return false;
                else
                    return true;
            }
        }

        public byte[] PreparePostData()
        {
            if (_selectedNode != null)
                PrepareAccessResource();
            string postData = string.Empty;
            foreach (var item in Pairs)
            {
                if (!string.IsNullOrEmpty(postData))
                    postData += "&";
                postData += item.Key + "=" + item.Value;
            };
            return Encoding.ASCII.GetBytes(postData);
        }
      
        #endregion

        #region Private Method
        private void PrepareAccessResource()
        {
            foreach (var item in _selectedNode.ChildNodes)
            {
                if (item.Name == "input")
                {
                    var shortpath = item.Attributes;
                    CheckDictonary(shortpath);
                }
            }
        }

        private void CheckDictonary(HtmlAttributeCollection shortpath)
        {
            if (shortpath[0].Name == "type" && shortpath[0].Value == "hidden")
            {
                if (Pairs.ContainsKey(shortpath[1].Value))
                    Pairs[shortpath[1].Value] = shortpath[2].Value;
                else
                    Pairs.Add(shortpath[1].Value, shortpath[2].Value);
            }
        }
        #endregion

        protected string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

       
    }
}
