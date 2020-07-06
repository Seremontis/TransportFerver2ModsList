using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Models;
using TF2ModsList.Services;
using TF2ModsList.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TF2ModsList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubMenuTF2 : ContentPage
    {
        public SubMenuTF2(TF2ItemMenu viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }

        private void SubMenu_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var listview = (ListView)sender;
            var item = (TF2ItemMenu)listview.SelectedItem;
            Navigation.PushAsync(new ItemTF2(item));
        }
    }
}