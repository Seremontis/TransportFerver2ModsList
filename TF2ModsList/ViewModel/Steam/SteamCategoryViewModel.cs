using Android.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using TF2ModsList.Models;
using TF2ModsList.Services.Interface;

namespace TF2ModsList.ViewModel
{
    public class SteamCategoryViewModel:BaseViewModel
    {
        private IWebOperation _webOperation;
        private ISteamCategoryOperation _steamCategoryOperation;
        private List<string> _Categories;

        public List<string> Categories
        {
            get { return _Categories; }
            set { _Categories = value;
                NotifyPropertyChanged();
            }
        }

        public SteamCategoryViewModel(IWebOperation webOperation,ISteamCategoryOperation steamCategoryOperation)
        {
            this._webOperation = webOperation;
            this._steamCategoryOperation = steamCategoryOperation;
        }

        public SteamCategoryViewModel ExecuteData()
        {
            Task.Run(async () =>
            {
                _steamCategoryOperation.Html = await _webOperation.ReadWeb("https://steamcommunity.com/app/1066780/workshop/",EnumWebsite.Steam);

            }).ContinueWith((t1) =>
            {
               _Categories = _steamCategoryOperation.GetCategoryOptions();
            });
            return this;
        }
    }
}
