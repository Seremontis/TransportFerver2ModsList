using HtmlAgilityPack;
using System.Collections.ObjectModel;
using TF2ModsList.Models;
using TF2ModsList.Services;
using TF2ModsList.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TF2ModsList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuTF2 : ContentPage
    {
        public MenuTF2()
        {
            InitializeComponent();
            this.BindingContext = App.IocContainer.GetInstance<MenuTF2ViewModel>().ExecuteData();
        }

        private void Menu_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var listView = (ListView)sender;
            var selItem = (TF2ItemMenu)listView.SelectedItem;
            Navigation.PushAsync(new SubMenuTF2(selItem));
        }
    }
}