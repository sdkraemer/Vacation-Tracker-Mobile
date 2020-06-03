using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereYouAtMobile.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhereYouAtMobile.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
            InitializeComponent();
            TrackingIntervalInMillisecondsEntry.Text = Settings.TrackingIntervalInMilliseconds+"";
            SaveButton.Clicked += SaveButton_ClickedAsync;
        }

        private async void SaveButton_ClickedAsync(object sender, EventArgs e)
        {
            Settings.TrackingIntervalInMilliseconds = Int32.Parse(TrackingIntervalInMillisecondsEntry.Text);
            await App.NavPage.PopAsync();
        }
    }
}