using Auth0.OidcClient;
using Plugin.Geolocator.Abstractions;
using System;
using System.Diagnostics;
using System.Text;
using WhereYouAtMobile.Helpers;
using WhereYouAtMobile.Messages;
using WhereYouAtMobile.Services;
using Xamarin.Forms;

namespace WhereYouAtMobile
{
	public partial class MainPage : ContentPage
	{
        private readonly IAuth0Client _auth0Client;
        Position savedPosition;
        private LocationRestService locationRestService;

        public MainPage()
        {
            InitializeComponent();
            this._auth0Client = DependencyService.Get<IAuth0ClientFactory>().Create();
            LoginButton.Clicked += LoginButton_Clicked;
            HandleReceievedMessages();
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
        }

        private void HandleReceievedMessages()
        {
            MessagingCenter.Subscribe<LocationTickMessage>(this, "LocationTickMessage", async message =>
            {
                labelTracking.Text = message.Message;
                Debug.WriteLine("LocationTickMessage receieved: "+message.Location.Longitude);
                labelTracking.Text = DateTime.Now.ToLongTimeString();
                await this.locationRestService.SaveLocationAsync(message.Location);
            });
        }

        /*private async void ButtonGetGPS_Clicked(object sender, EventArgs e)
        {
            try
            {
                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission)
                    return;

                ButtonGetGPS.IsEnabled = false;

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = DesiredAccuracy.Value;
                labelGPS.Text = "Getting gps...";

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(Timeout.Value), null, IncludeHeading.IsToggled);

                if (position == null)
                {
                    labelGPS.Text = "null gps :(";
                    return;
                }
                savedPosition = position;
                //ButtonAddressForPosition.IsEnabled = true;
                labelGPS.Text = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
            }
            finally
            {
                ButtonGetGPS.IsEnabled = true;
            }
        }*/

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
