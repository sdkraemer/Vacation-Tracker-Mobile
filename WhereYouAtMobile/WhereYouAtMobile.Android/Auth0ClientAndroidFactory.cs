using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Auth0.OidcClient;
using WhereYouAtMobile.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(Auth0ClientAndroidFactory))]
namespace WhereYouAtMobile.Droid
{
    class Auth0ClientAndroidFactory : IAuth0ClientFactory
    {
        public IAuth0Client Create()
        {
            var client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = "feeldaburn.auth0.com",
                ClientId = "TSWTGq6o5dDKUYt1qxvSGWOjikQZ38VX",
                Scope = "openid profile"
            });
            return client;
        }
    }
}