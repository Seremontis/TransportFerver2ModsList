using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TF2ModsList.Models;
using TF2ModsList.Services;

namespace TF2ModsList.ViewModel
{
    public class ItemTF2ViewModel
    {
        #region fields
        private ObservableCollection<Mod> _ListItems;
        private string _NameCategory;

        private IWebOperation _webOperation;
        private IDataOperation _dataOperation;
        #endregion

        #region prop
        public ObservableCollection<Mod> ListItems
        {
            get { return _ListItems; }
            set { _ListItems = value; }
        }
        public string NameCategory
        {
            get { return _NameCategory; }
        }
        #endregion

        public ItemTF2ViewModel(IDataOperation dataOperation, IWebOperation webOperation)
        {
            this._dataOperation = dataOperation;
            this._webOperation = webOperation;
            
        }
        public ItemTF2ViewModel ExecuteData(TF2ItemMenu itemMenu)
        {
            _dataOperation.Html = _webOperation.ReadWeb(itemMenu.Path);
            ListItems = _dataOperation.ReturnModsItem();
            _NameCategory = itemMenu.NameItem;
            return this;
        }
    }
}
