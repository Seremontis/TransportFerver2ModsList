using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using TF2ModsList.Models;

namespace TF2ModsList.Services
{
    /// <summary>
    /// img wyszukać z opisu
    /// </summary>

    public class DataOperation : IDataOperation
    {
        #region fields
        private HtmlDocument _Html;
        private HtmlNode _selectedNode;
        private Dictionary<string, string> Pairs = new Dictionary<string, string>();
        private readonly string defaultPath = "html/body/div[@id='pageContainer']/section[@id='main']/div/div";
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

        public ObservableCollection<TF2ItemMenu> ReturnMainMenuTF2()
        {
            if (_Html == null)
                throw new Exception("HtmlDocument jest pusty");
            else
                return PrepareListMainMenuTF2();
        }

        public ObservableCollection<Mod> ReturnModsItem()
        {
            ObservableCollection<Mod> mods = new ObservableCollection<Mod>();
            _selectedNode = _Html.DocumentNode.SelectSingleNode(defaultPath + "/div/table/tbody");
            foreach (var item in _selectedNode.ChildNodes)
            {
                if (item.Name == "tr")
                    mods.Add(ExtractModsInfo(item));
            }
            return mods;
        }

        public DetailMod ReturnDetailModTF2()
        {
            _selectedNode = _Html.DocumentNode.SelectSingleNode(defaultPath + "[@class='content']");
            return ExtractDetailMod();
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

        private ObservableCollection<TF2ItemMenu> PrepareListMainMenuTF2()
        {
            ObservableCollection<TF2ItemMenu> list = new ObservableCollection<TF2ItemMenu>();
            _selectedNode = _Html.DocumentNode.SelectSingleNode(defaultPath + "/div[@class='section']/ul");
            foreach (var item in _selectedNode.ChildNodes)
            {
                if (item.Name == "li")
                    list.Add(ReturnMenuTF2Item(item));
            }
            return list;
        }

        private TF2ItemMenu ReturnMenuTF2Item(HtmlNode node)
        {
            var extractElement = node.SelectSingleNode("div/div/div/h3/a");
            TF2ItemMenu item = FillItem(extractElement);
            var extractSubElement = node.SelectSingleNode("div/div/ul");
            if (extractSubElement != null)
            {
                item.SubItems = new List<TF2ItemMenu>();
                foreach (var subitem in extractSubElement.ChildNodes)
                {
                    if (subitem.Name == "li")
                        item.SubItems.Add(FillItem(subitem.SelectSingleNode("div/a")));
                }
            }
            return item;
        }

        private TF2ItemMenu FillItem(HtmlNode node)
        {
            TF2ItemMenu item = new TF2ItemMenu()
            {
                NameItem = node.InnerText,
                Path = node.Attributes["href"].Value
            };
            return item;
        }

        private Mod ExtractModsInfo(HtmlNode node)
        {
            var listTime = node.SelectNodes("td/small/time");
            Mod mod = new Mod()
            {
                UriPicture = new Uri(node.SelectSingleNode("td/div/p/img").Attributes["src"].Value),
                UriPage = new Uri(node.SelectSingleNode("td/h3/a").Attributes["href"].Value),
                Name = node.SelectSingleNode("td/h3/a").InnerText,
                Author = node.SelectSingleNode("td/small/a").InnerText,
                DataCreate = listTime[0].InnerText,
                DataUpdate = listTime.Count > 1 ? listTime[1].InnerText : "No update",
            };
            return mod;
        }

        private DetailMod ExtractDetailMod()
        {
            DetailMod detailMod = new DetailMod()
            {
                Title = _selectedNode.SelectSingleNode("header/div[@class='contentHeaderTitle']/h1/span").InnerText,
                Description = _selectedNode.SelectSingleNode("div[@class='section']/ul/li/article/div/div/div[@class='messageText']").InnerHtml,
                MainPicture = GetMainPicture(), 
                SpecificationMod = StripHTML(_selectedNode.SelectSingleNode("div[@class='section']/ul/li/article/div/div/div[@class='section']").InnerHtml),
                SteamId = ExtractSteamIdFromTF2(),
            };

            detailMod.Authors = GetAuthors(_selectedNode.SelectSingleNode("div[@class='section']/ul/li/article/div/div/div[@class='section']/table[@class='table filebase contributors']"));
            detailMod.VersionFiles = GetVersionFile();
            detailMod.ListPictures = GetListPicturesSectionImage(_selectedNode.SelectSingleNode("div[@class='section']/ul/li/article/div/footer/section/ul"));
            detailMod.ListPictures.AddRange(ImageDetailModWithText(_selectedNode.SelectSingleNode("div[@class='section']/ul/li/article/div/div/div[@class='messageText']")));
            return detailMod;
        }

        private Uri GetMainPicture()
        {
            HtmlNode html = _selectedNode.SelectSingleNode("div[@class='section']/ul/li/article/div/div/div[@class='filebasePreviewImage']/a");
            if (html != null)
                return new Uri(html.Attributes["href"].Value??string.Empty);
            html = _selectedNode.SelectSingleNode("div[@class='section']/ul/li/article/div/div/div[@class='filebasePreviewImage framed']/img");
            if (html != null)
                return new Uri(html.Attributes["src"].Value ?? string.Empty);
            return null;
        }

        private List<string> GetAuthors(HtmlNode node)
        {
            List<string> vs = new List<string>();
            foreach (var item in node.ChildNodes)
            {
                if (item.Name == "tr")
                {
                    var searcher = item.SelectSingleNode("td/a[@class='userLink']");
                    string value = string.Empty;
                    if (searcher != null)
                         value = searcher.InnerText; 
                    else
                        value=item.SelectNodes("td")[1].FirstChild.InnerText;
                    vs.Add(value);
                }

            }
            return vs;
        }

        private Dictionary<string, Uri> GetVersionFile()
        {
            var nodes = _selectedNode.SelectSingleNode("div[@class='section']/ul/li/article/div/div/div[@class='filebaseFileList clearfix']/section/ul");
            Dictionary<string, Uri> pairs = new Dictionary<string, Uri>();
            foreach (var item in nodes.ChildNodes)
            {
                if (item.Name == "li")
                {
                    var ahref = item.SelectSingleNode("div/h3/a");
                    var uri = new Uri(ahref.Attributes["href"].Value);
                    pairs.Add(ahref.InnerText, uri);
                }
            }
            return pairs;
        }

        private List<Uri> GetListPicturesSectionImage(HtmlNode node)
        {
            List<Uri> uris = new List<Uri>();
            if (node != null)
            {
                foreach (var item in node.ChildNodes)
                {
                    if (item.Name == "li")
                    {
                        var uri = new Uri(item.SelectSingleNode("a").Attributes["href"].Value);
                        uris.Add(uri);
                    }
                }
            }           
            return uris;
        }

        private List<Uri> ImageDetailModWithText(HtmlNode node)
        {
            List<Uri> uris = new List<Uri>();
            var list = node.SelectNodes("p//a/img");
            if (list != null)
            {
                foreach (var item in list)
                    uris.Add(new Uri(item.Attributes["src"].Value));
            }
            
            return uris;
        }

        private string ExtractSteamIdFromTF2()
        {
            var nodes = _selectedNode.SelectNodes("div[@class='section']/ul/li/article/div/div/div[@class='section']");
            foreach (var item in nodes)
            {
                if (item.SelectSingleNode("dl/dt") != null)
                {
                    if (item.SelectSingleNode("dl/dt").InnerText == "Steam Workshop")
                        return item.SelectSingleNode("dl/dd").InnerText;
                }               
            }
            return string.Empty;
        }


        private string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        #endregion
    }
}
