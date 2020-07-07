using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Models;
using TF2ModsList.Services;
using Xamarin.Forms;

namespace TF2ModsList.ViewModel
{
    public class DetailModTF2ViewModel
    {
        private DetailMod _DetailItem;
        private string _UriSource;

        private IDataOperation _dataOperation;
        private IWebOperation _webOperation;

        public DetailMod DetailItem
        {
            get { return _DetailItem; }
            set { _DetailItem = value; }
        }
        public string UriSource
        {
            get { return _UriSource; }
        }


        public DetailModTF2ViewModel(IDataOperation operationData,IWebOperation webOperation)
        {
            this._dataOperation= operationData;
            this._webOperation = webOperation;
            
        }

        public DetailModTF2ViewModel ExecuteData(Mod itemMenu)
        {
            try
            {
                _UriSource = itemMenu.UriPage.ToString();
                _dataOperation.Html = _webOperation.ReadWeb(UriSource);
                _DetailItem = _dataOperation.ReturnDetailModTF2();
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
