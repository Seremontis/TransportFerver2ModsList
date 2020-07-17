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
    public class TF2MenuOperation:DataOperation, ITF2MenuOperation
    {
        public async Task<ObservableCollection<TF2ItemMenu>> ReturnMainMenuTF2()
        {
            if (_Html == null)
                throw new Exception("HtmlDocument jest pusty");
            else
                return await Task.Run(()=> PrepareListMainMenuTF2());
        }
        private ObservableCollection<TF2ItemMenu> PrepareListMainMenuTF2()
        {
            ObservableCollection<TF2ItemMenu> list = new ObservableCollection<TF2ItemMenu>();
            var liElement = _Html.DocumentNode.SelectNodes("//ul[@class='tabularBox filebaseCategoryList']/li");
            foreach (var item in liElement)
            {
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

    }
}
