using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TF2ModsList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PictureFullScreen : ContentPage
    {

        public PictureFullScreen(Uri imageUri)
        {
            this.BindingContext = imageUri;
            InitializeComponent();
            ResizeImage();
        }

        private void ResizeImage()
        {
            test.WidthRequest = (int)Application.Current.MainPage.Height;
            test.HeightRequest = (int)Application.Current.MainPage.Width;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "setLandscape");
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "setPortrait");
        }
    }
}