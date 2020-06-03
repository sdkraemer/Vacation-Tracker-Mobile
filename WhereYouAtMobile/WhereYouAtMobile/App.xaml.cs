using System;
using WhereYouAtMobile.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace WhereYouAtMobile
{
	public partial class App : Application
	{
        public static NavigationPage NavPage;

        public App ()
		{
			InitializeComponent();

            //MainPage = new MainPage();
            //var mainPage = new MainPage();
            //_NavPage = new NavigationPage(mainPage);
            //MainPage = _NavPage;

            NavPage = new NavigationPage(new HomePage());
            MainPage = NavPage;
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
