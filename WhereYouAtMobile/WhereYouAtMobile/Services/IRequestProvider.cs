using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WhereYouAtMobile.Services
{
    interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri);
        Task<bool> PostAsync<TResult>(string uri, TResult data);
        Task<bool> PutAsync<TResult>(string uri, TResult data);
        Task<bool> DeleteAsync(string uri);
    }
}
