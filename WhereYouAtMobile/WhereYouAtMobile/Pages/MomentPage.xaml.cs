using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereYouAtMobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhereYouAtMobile.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MomentPage : ContentPage
	{
		public MomentPage (String twitterUrl)
		{
			InitializeComponent ();
            TwitterUrlEntry.Text = twitterUrl;
            PostButton.Clicked += PostButton_ClickedAsync;
        }

        private async void PostButton_ClickedAsync(object sender, EventArgs e)
        {
            StatusLabel.Text = "Saving";
            var locationService = new LocationService();
            StatusLabel.Text = "Retrieving Location";
            var location = await locationService.RetrieveLocationAsync(MessageEditor.Text, TwitterUrlEntry.Text);
            StatusLabel.Text = "Posting to API";
            var locationRestService = new LocationRestService(new ResilientRequestProvider());
            await locationRestService.SaveLocationAsync(location);
            await App.NavPage.PopAsync();
        }
    }
}