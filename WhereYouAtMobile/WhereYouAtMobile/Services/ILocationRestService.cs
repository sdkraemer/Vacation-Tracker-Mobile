using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhereYouAtMobile.Models;

namespace WhereYouAtMobile.Services
{
    interface ILocationRestService
    {
        Task SaveLocationAsync(Location location);
    }
}
