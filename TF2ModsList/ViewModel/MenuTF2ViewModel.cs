using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Models;
using TF2ModsList.Services;
using Xamarin.Forms;

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
            try
            {
                _operationData.Html = _webOperation.ReadWeb();
                _MenutItems = _operationData.ReturnMainMenuTF2();
            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Problem", "Wystąpił problem, przepraszamy", "Koniec");
                Environment.Exit(0);
            }
            return this;
        }
    }
}
