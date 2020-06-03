using Auth0.OidcClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace WhereYouAtMobile
{
    public interface IAuth0ClientFactory
    {
        IAuth0Client Create();
    }
}
