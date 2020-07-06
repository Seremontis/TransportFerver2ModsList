using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using TF2ModsList.Models;
using TF2ModsList.Services;

namespace TF2ModsList.ViewModel
{
    public class MenuTF2ViewModel
    {
        #region fields
        private ObservableCollection<TF2ItemMenu> _MenutItems;
        private IDataOperation _operationData;
        private IWebOperation _webOperation;
        #endregion

        public ObservableCollection<TF2ItemMenu> MenuItems
        {
            get { return _MenutItems; }
            set { _MenutItems = value; }
        }
        public MenuTF2ViewModel(IDataOperation operationData, IWebOperation webOperation)
        {
            this._operationData = operationData;
            this._webOperation = webOperation;
        }

        public MenuTF2ViewModel ExecuteData()
        {           
            _operationData.Html=_webOperation.ReadWeb();
            MenuItems = _operationData.ReturnMainMenuTF2();
            return this;
        }
    }
}
