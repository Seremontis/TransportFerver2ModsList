﻿using Android.Test.Suitebuilder.Annotation;
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

    public partial class ItemTF2 : ContentPage
    {
        private TF2ItemMenu _itemMenu;
        public ItemTF2(TF2ItemMenu itemMenu)
        {
            _itemMenu = itemMenu;
            InitializeComponent();
            this.BindingContext = App.IocContainer.GetInstance<ItemsTF2ViewModel>().ExecuteData(itemMenu);
        }

        private void ItemDetail_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = (ListView)sender;
            var item = (Mod)list.SelectedItem;
            Navigation.PushAsync(new ItemDetailTF2(item));
        }

        private void Previous_Clicked(object sender, EventArgs e)
        {
            var model = (ItemsTF2ViewModel)BindingContext;
            _itemMenu.Path = model.PreviousPage;
            BindingContext = App.IocContainer.GetInstance<ItemsTF2ViewModel>().ExecuteData(_itemMenu);
        }
        private void Next_Clicked(object sender, EventArgs e)
        {
            var model = (ItemsTF2ViewModel)BindingContext;
            _itemMenu.Path = model.NextPage;
            this.BindingContext = App.IocContainer.GetInstance<ItemsTF2ViewModel>().ExecuteData(_itemMenu);
        }


    }
}