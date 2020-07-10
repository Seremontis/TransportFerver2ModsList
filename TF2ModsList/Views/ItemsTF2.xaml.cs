using System;
using System.Collections.Generic;
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

    ///add more page (now is first page from website)
    public partial class ItemTF2 : ContentPage
    {
        public ItemTF2(TF2ItemMenu itemMenu)
        {
            InitializeComponent();
            this.BindingContext = App.IocContainer.GetInstance<ItemsTF2ViewModel>().ExecuteData(itemMenu);
        }

        private void ItemDetail_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = (ListView)sender;
            var item = (Mod)list.SelectedItem;
            Navigation.PushAsync(new ItemDetailTF2(item));
        }
    }
}