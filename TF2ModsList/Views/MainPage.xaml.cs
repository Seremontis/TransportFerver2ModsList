using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Views;
using TF2ModsList.Views.Steam;
using Xamarin.Forms;

namespace TF2ModsList
{

    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ShowTF2_Click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuTF2());
        }

        private void ShowSteam_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SteamCategory());
        }
    }
}
