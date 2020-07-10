using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Models;
using TF2ModsList.Services;
using TF2ModsList.Services.Interface;
using Xamarin.Forms;

namespace TF2ModsList.ViewModel
{

    /// <summary>
    /// repair gallery
    /// </summary>
    public class DetailModTF2ViewModel : BaseViewModel
    {
       
        #region fields
        private DetailMod _DetailItem;
        private string _UriSource;


        private ITF2DetailModOperation _tf2DetailOperation;
        private IWebOperation _webOperation;
        #endregion

        #region properties
        public DetailMod DetailItem
        {
            get { return _DetailItem; }
            set
            {
                _DetailItem = value;
                NotifyPropertyChanged();
            }
        }
        public string UriSource
        {
            get { return _UriSource; }
        }

        #endregion

        #region methods
        public DetailModTF2ViewModel(ITF2DetailModOperation operationData, IWebOperation webOperation)
        {
            this._tf2DetailOperation = operationData;
            this._webOperation = webOperation;

        }
        public DetailModTF2ViewModel ExecuteData(Mod itemMenu)
        {
            Task.Run(async () =>
            {
                _UriSource = itemMenu.UriPage.ToString();
                _tf2DetailOperation.Html = await _webOperation.ReadWeb(UriSource);

            }).ContinueWith((t1) => {
                DetailItem = _tf2DetailOperation.ReturnDetailModTF2();
                IsVisible = true;
            });
            return this;
        }

        #endregion
    }
}
