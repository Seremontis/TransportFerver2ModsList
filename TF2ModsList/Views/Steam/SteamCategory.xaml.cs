using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TF2ModsList.Views.Steam
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SteamCategory : ContentPage
    {
        /// <summary>
        /// TODO
        /// checkboxList categories
        /// </summary>
        public SteamCategory()
        {
            InitializeComponent();
            this.BindingContext = App.IocContainer.GetInstance<SteamCategoryViewModel>().ExecuteData();
        }

        private void Search_Clicked(object sender, EventArgs e)
        {

        }
    }
}