using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SowManager
{
    public partial class App : Application
    {

        public static string FilePath;

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        public App(string filePath)
        {
            InitializeComponent();

            // load starting page
            MainPage = new NavigationPage(new MainPage());

            // the path of the sqlite database file
            // it is determined differently on Android and iOS then passed to this class
            FilePath = filePath;
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
    }
}
