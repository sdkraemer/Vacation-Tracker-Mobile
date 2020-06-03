using Auth0.OidcClient;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereYouAtMobile.Helpers;
using WhereYouAtMobile.Messages;
using WhereYouAtMobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhereYouAtMobile.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
        private readonly IAuth0Client _auth0Client;
        private LocationRestService locationRestService;
        private ObservableCollection<string> locationsRetrievedList;

        public HomePage ()
		{
            InitializeComponent();
            this._auth0Client = DependencyService.Get<IAuth0ClientFactory>().Create();
            LoginButton.Clicked += LoginButton_Clicked;
            SettingsPageButton.Clicked += SettingsPageButton_Clicked;
            MomentPageButton.Clicked += MomentPageButton_Clicked;
            HandleReceievedMessages();

            this.locationsRetrievedList = new ObservableCollection<string>();
            this.LocationsRetrievedListView.ItemsSource = this.locationsRetrievedList;
        }

        private void MomentPageButton_Clicked(object sender, EventArgs e)
        {
            App.NavPage.PushAsync(new MomentPage(""));
        }

        private void SettingsPageButton_Clicked(object sender, EventArgs e)
        {
            App.NavPage.PushAsync(new SettingsPage());
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            var loginResult = await _auth0Client.LoginAsync(new { audience = "http://www.whereuat.net/api" });

            var sb = new StringBuilder();

            if (loginResult.IsError)
            {
                LoginResultLabel.Text = "An error occurred during login...";

                sb.AppendLine("An error occurred during login:");
                sb.AppendLine(loginResult.Error);
            }
            else
            {
                LoginResultLabel.Text = $"Welcome {loginResult.User.Identity.Name}";
                AccessTokenExpirationLabel.Text = "Access Token Expires: "+loginResult.AccessTokenExpiration.ToLongDateString()+" "+loginResult.AccessTokenExpiration.ToLongTimeString();

                sb.AppendLine($"ID Token: {loginResult.IdentityToken}");
                sb.AppendLine($"Access Token: {loginResult.AccessToken}");
                sb.AppendLine($"Refresh Token: {loginResult.RefreshToken}");
                sb.AppendLine();
                sb.AppendLine("-- Claims --");
                
                foreach (var claim in loginResult.User.Claims)
                {
                    sb.AppendLine($"{claim.Type} = {claim.Value}");
                }

                Settings.AuthToken = loginResult.AccessToken;
                this.locationRestService = new LocationRestService(new ResilientRequestProvider());
                EnableLocationButtons();
                Debug.WriteLine("Auth Token In Settings:" + Settings.AuthToken);
            }

            Debug.WriteLine(sb.ToString());

        }

        private void EnableLocationButtons()
        {
            StartTrackingButton.IsEnabled = true;
            StopTrackingButton.IsEnabled = true;
            MomentPageButton.IsEnabled = true;
        }

        private void HandleReceievedMessages()
        {
            MessagingCenter.Subscribe<LocationTickMessage>(this, "LocationTickMessage", async message =>
            {
                //labelTracking.Text = message.Message;
                //Debug.WriteLine("LocationTickMessage receieved: " + message.RetrievedAt.ToLongTimeString());
                //labelTracking.Text = message.RetrievedAt.ToLongTimeString();
                //AddLocationRetrievedAtToListView(message);
                //await this.locationRestService.SaveLocationAsync(message.Location);
            });

            MessagingCenter.Subscribe<LocationReceivedMessage>(this, "LocationReceivedMessage", message =>
            {
                AddMessageToListView("Location Received: "+message.ReceivedAt.ToLongTimeString());
            });
            
            MessagingCenter.Subscribe<LocationSentToApiSuccessMessage>(this, "LocationSentToApiSuccessMessage", message =>
            {
                AddMessageToListView("Location sent to API: " + message.ReceivedAt.ToLongTimeString());
            });
        }

        private void AddMessageToListView(string message)
        {
            if (this.locationsRetrievedList.Count > 9)
            {
                this.locationsRetrievedList.RemoveAt(this.locationsRetrievedList.Count - 1);
            }
            this.locationsRetrievedList.Insert(0, message);
        }

        private void AddLocationRetrievedAtToListView(LocationTickMessage message)
        {
            if(this.locationsRetrievedList.Count > 4)
            {
                this.locationsRetrievedList.RemoveAt(this.locationsRetrievedList.Count-1);
            }
            this.locationsRetrievedList.Insert(0, message.RetrievedAt.ToLongTimeString());
        }

        private void StartTrackingButton_Clicked(object sender, EventArgs e)
        {
            var message = new StartTrackingMessage();
            MessagingCenter.Send(message, "StartTrackingMessage");
        }

        private void StopTrackingButton_Clicked(object sender, EventArgs e)
        {
            var message = new StopTrackingMessage();
            MessagingCenter.Send(message, "StopTrackingMessage");
        }
    }
}