using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using TF2ModsList.Models;
using TF2ModsList.Services.Interface;

namespace TF2ModsList.Services.Logic
{
    public class TF2DetailModOperation:DataOperation,ITF2DetailModOperation
    {
        public DetailMod ReturnDetailModTF2()
        {
            _selectedNode = _Html.DocumentNode.SelectSingleNode(defaultPath + "[@class='content']");
            return ExtractDetailMod();
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
    }
}
