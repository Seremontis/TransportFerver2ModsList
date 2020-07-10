using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Models;
using TF2ModsList.Services.Interface;

namespace TF2ModsList.Services.Logic
{
    public class TF2ListItemOperation : DataOperation,ITF2ListItemOperation
    {
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

    }
}
