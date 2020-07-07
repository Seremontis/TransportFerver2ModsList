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
    public partial class ItemDetailTF2 : ContentPage
    {
        public ItemDetailTF2(Mod item)
        {
            InitializeComponent();
            this.BindingContext = App.IocContainer.GetInstance<DetailModTF2ViewModel>().ExecuteData(item);
            ReturnImageToGrid();
        }

        private void ShowHideGallery_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            GridImage.IsVisible = !GridImage.IsVisible;
            string value = GetString(button);
            var but = (Button)StackLyt.FindByName("BelowGalleryBut");
            but.IsVisible = !but.IsVisible;
 
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var image = (Image)sender;
            var uri = (Uri)image.Source.GetValue(UriImageSource.UriProperty);
            Navigation.PushAsync(new PictureFullScreen(uri));
        }

        private string GetString(Button button)
        {
            if(button.Text=="Show gallery")
            {
                string value = "Hide gallery";
                button.Text = value;
                return value;
            }
            else
            {
                string value = "Show gallery";
                button.Text = value;
                return value;
            }
        }

        private void ReturnImageToGrid()
        {
            var context = (DetailModTF2ViewModel)BindingContext;
            var listImage = context.DetailItem.ListPictures;
            if (listImage.Count == 0)
            {
                var but = (Button)StackLyt.FindByName("AboveGalleryBut");
                but.IsVisible = !but.IsVisible;
            }
            else
            {
                GridImage.Children.Clear();
                GridImage.ColumnDefinitions.Clear();
                GridImage.RowDefinitions.Clear();
                int maxElementInRow = 4;
                AddColumn(maxElementInRow);
                AddRow(listImage.Count / maxElementInRow);
                for (int i = 0; i < listImage.Count; i++)
                    GridImage.Children.Add(LoadImage(listImage[i], i % maxElementInRow, i / maxElementInRow));
                GridImage.MinimumHeightRequest = 100;
            }         
        }

        private void AddColumn(int maxCol)
        {
            for (int i = 0; i < maxCol; i++)
            {
                ColumnDefinition definition = new ColumnDefinition();
                definition.Width = new GridLength(100, GridUnitType.Absolute);
                GridImage.ColumnDefinitions.Add(definition);
            }
        }

        private void AddRow(int maxRow)
        {
            for (int i = 0; i < maxRow; i++)
            {
                RowDefinition definition = new RowDefinition();
                definition.Height = new GridLength(1, GridUnitType.Star);
                GridImage.RowDefinitions.Add(definition);
            }
        }
        private Image LoadImage(Uri uri,int columnNumber,int rowNumber)
        {
            Image image = new Image()
            {
                Source = uri,
                Aspect=Aspect.AspectFill,
            };           
            var tapped = new TapGestureRecognizer();
            tapped.Tapped += (sender, e) =>
            {
                TapGestureRecognizer_Tapped(sender, e);
            };
            image.GestureRecognizers.Add(tapped);
            Grid.SetRow(image, rowNumber);
            Grid.SetColumn(image, columnNumber);
            return image;
        }

       
    }
}