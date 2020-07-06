using SimpleInjector;
using System;
using System.ComponentModel;
using TF2ModsList.Services;
using TF2ModsList.ViewModel;
using TF2ModsList.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Container = SimpleInjector.Container;

namespace TF2ModsList
{
    public partial class App : Application
    {
        private static Container _iocContainer= new SimpleInjector.Container();
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

            IocContainer.Register<IDataOperation, DataOperation>(Lifestyle.Transient);
            IocContainer.Register<IWebOperation, WebOperation>(Lifestyle.Transient);
            IocContainer.Register<MenuTF2ViewModel>();
            IocContainer.Register<ItemTF2ViewModel>();
            IocContainer.Register<DetailModTF2ViewModel>();


            IocContainer.Verify();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static Container IocContainer
        {
            get { return _iocContainer; }
            set { _iocContainer = value; }
        }
    }
}
