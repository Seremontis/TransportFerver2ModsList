using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Models;
using TF2ModsList.Services;
using TF2ModsList.Services.Interface;
using Xamarin.Forms;
namespace TF2ModsList.ViewModel
{
    public class ItemsTF2ViewModel:BaseViewModel
    {

        #region fields
        private ObservableCollection<Mod> _ListItems;
        private string _NameCategory;

        private IWebOperation _webOperation;
        private ITF2ListItemOperation _tf2Operation;
        #endregion

        #region prop
        public ObservableCollection<Mod> ListItems
        {
            get { return _ListItems; }
            set { 
                _ListItems = value;
                NotifyPropertyChanged();
            }
        }
        public string NameCategory
        {
            get { return _NameCategory; }
        }
        #endregion

        #region methods
        public ItemsTF2ViewModel(ITF2ListItemOperation dataOperation, IWebOperation webOperation)
        {
            this._tf2Operation = dataOperation;
            this._webOperation = webOperation;
            
        }
        public ItemsTF2ViewModel ExecuteData(TF2ItemMenu itemMenu)
        {
            _NameCategory = itemMenu.NameItem;
            Task.Run(async () => _tf2Operation.Html = await _webOperation.ReadWeb(itemMenu.Path))
                .ContinueWith((t1) =>
                {
                    ListItems = _tf2Operation.ReturnModsItem();
                    IsVisible = true;
                });

            return this;
        }
        #endregion
    }
}
