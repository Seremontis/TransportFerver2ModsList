using HtmlAgilityPack;
using Java.Text;
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
        private string _PreviousPage;
        private string _NextPage;

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

        public string PreviousPage
        {
            get { return _PreviousPage; }
            set { _PreviousPage = value;
                NotifyPropertyChanged("PreviousPageBool");
            }
        }
        public string NextPage
        {
            get { return _NextPage; }
            set { _NextPage = value;
                NotifyPropertyChanged("NextPageBool");
            }
        }


        public bool PreviousPageBool{ get {return  !string.IsNullOrEmpty(PreviousPage); } }
        public bool NextPageBool{ get {return  !string.IsNullOrEmpty(NextPage); } }
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
            IsVisible = false;
            Task.Run(async () => _tf2Operation.Html = await _webOperation.ReadWeb(itemMenu.Path))
                .ContinueWith((t1) =>
                {
                    ListItems = _tf2Operation.ReturnModsItem();
                    var infoPage = _tf2Operation.ReturnAccesPage();
                    PreviousPage = infoPage.Item1;
                    NextPage = infoPage.Item2;
                    IsVisible = true;
                });
            return this;
        }
        #endregion
    }
}
