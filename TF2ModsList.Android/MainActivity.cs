using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using TF2ModsList.Views;

namespace TF2ModsList.Droid
{
    [Activity(Label = "TF2ModsList", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.SetFlags("Shapes_Experimental");
            MessagingCenter.Subscribe<PictureFullScreen>(this, "setLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
                Window.AddFlags(WindowManagerFlags.Fullscreen);
            });
            MessagingCenter.Subscribe <PictureFullScreen>(this, "setPortrait", sender =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;
                Window.ClearFlags(WindowManagerFlags.Fullscreen);
            });
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}