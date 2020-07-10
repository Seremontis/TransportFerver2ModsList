using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Models;
using TF2ModsList.Services;
using TF2ModsList.Services.Interface;
using Xamarin.Forms;

namespace TF2ModsList.ViewModel
{
    public class MenuTF2ViewModel: BaseViewModel
    {

        #region fields
        private ObservableCollection<TF2ItemMenu> _MenutItems;
        private ITF2MenuOperation _operationDataTF2Menu;
        private IWebOperation _webOperation;
        private bool _isVisible = false;
        private bool _IsVisibleNegation = true;

        #endregion

        #region properties
        public ObservableCollection<TF2ItemMenu> MenuItems
        {
            get { return _MenutItems; }
            set { _MenutItems = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region methods
        public MenuTF2ViewModel(ITF2MenuOperation operationData, IWebOperation webOperation)
        {
            this._operationDataTF2Menu = operationData;
            this._webOperation = webOperation;
        }

        public MenuTF2ViewModel ExecuteData()
        {
            Task.Run(async () => _operationDataTF2Menu.Html = await _webOperation.ReadWeb())
                 .ContinueWith(async (t1) =>
                 {
                     MenuItems = await _operationDataTF2Menu.ReturnMainMenuTF2();
                     IsVisible = true;
                 });
            return this;
        }

        #endregion
    }
}
