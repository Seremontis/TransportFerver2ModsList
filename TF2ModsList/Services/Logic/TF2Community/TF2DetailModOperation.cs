using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Models;
using TF2ModsList.Services.Interface;

namespace TF2ModsList.Services.Logic
{
    public class TF2DetailModOperation : DataOperation, ITF2DetailModOperation
    {
        public DetailMod ReturnDetailModTF2()
        {
            _selectedNode = _Html.DocumentNode.SelectSingleNode("//div[@class='content']");
            var descripton = _selectedNode.SelectSingleNode(".//div[@class='messageText']");
            DetailMod detailMod = new DetailMod()
            {
                Title = _selectedNode.SelectSingleNode(".//span[@itemprop='name headline']").InnerText,
                Description = descripton.InnerHtml,
                MainPicture = GetMainPicture(),
                SpecificationMod = StripHTML(ReturnSpecMod()),
                SteamId = ReturnSteamId(),
            };

            detailMod.Authors = GetAuthors(_selectedNode.SelectSingleNode(".//table[@class='table filebase contributors']"));
            detailMod.VersionFiles = GetVersionFile();
            detailMod.ListPictures = GetListPicturesSectionImage(_selectedNode.SelectSingleNode(".//h2[contains(.,'Images')]"));
            detailMod.ListPictures.AddRange(ImageDetailModWithDescription(descripton));
            return detailMod;
        }
        private string ReturnSpecMod()
        {
            string result = string.Empty;
            var node = _selectedNode.SelectSingleNode("//h3[contains(.,'Vehicle data')]");
            if(node!=null)
                result =node.ParentNode.InnerHtml;

            return result;
        }
        private string ReturnSteamId()
        {
            var result = _selectedNode.SelectSingleNode("//h3[contains(.,'Workshop')]");
            if (result != null)
            {
                result = result.ParentNode.SelectSingleNode(".//dl");
                if (result != null)
                    return result.InnerText;
            }
            return string.Empty;
        }
        private Uri GetMainPicture()
        {
            var html = _selectedNode.SelectSingleNode(".//div[contains(@class,'filebasePreviewImage')]/a") ?? _selectedNode.SelectSingleNode(".//div[contains(@class,'filebasePreviewImage')]/img");
            if (html != null)
            {
                var t = html.Attributes["href"]?? html.Attributes["src"];
                if(t!=null)
                    return new Uri(t.Value);
            }               
            return null;
        }
        private List<string> GetAuthors(HtmlNode node)
        {
            var linkAuthors = node.SelectNodes(".//a[@class='userLink']");
            List<string> vs = new List<string>();
            foreach (var item in linkAuthors)               
                    vs.Add(item.InnerText);
            return vs;
        }
        private Dictionary<string, Uri> GetVersionFile()
        {
            var nodes = _selectedNode.SelectNodes(".//div[@class='filebaseFileList clearfix']/section/ul/li");
            Dictionary<string, Uri> pairs = new Dictionary<string, Uri>();
            foreach (var item in nodes)
            {
                var ahref = item.SelectSingleNode("div/h3/a");
                var uri = new Uri(ahref.Attributes["href"].Value);
                pairs.Add(ahref.InnerText, uri);
            }
            return pairs;
        }
        private List<Uri> GetListPicturesSectionImage(HtmlNode node)
        {
            List<Uri> uris = new List<Uri>();
            if (node != null)
            {
                var nodes = node.ParentNode.SelectNodes(".//li[@class='attachmentThumbnail']");              
                if (node != null)
                {
                    foreach (var item in nodes)
                    {
                        string uri = item.SelectSingleNode("a").Attributes["href"].Value;
                        if(uri.Contains("jpg") || uri.Contains("jpeg") || uri.Contains("png"))
                            uris.Add(new Uri(uri));
                    }
                }              
            }
            return uris;
        }
        private List<Uri> ImageDetailModWithDescription(HtmlNode node)
        {
            List<Uri> uris = new List<Uri>();
            var list = node.SelectNodes(".//img");
            if (list != null)
            {
                foreach (var item in list)
                {
                    string uri = item.Attributes["src"].Value;
                    if (uri.Contains("jpg") || uri.Contains("jpeg") || uri.Contains("png"))
                        uris.Add(new Uri(uri));
                }                 
            }

            return uris;
        }
    }
}
