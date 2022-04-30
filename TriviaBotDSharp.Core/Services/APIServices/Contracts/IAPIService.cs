using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TriviaBotDSharp.Core.Services.APIServices.Contracts
{
    public interface IAPIService
    {
        Task<T> GetDataAsync<T>(string url);
    }
}
