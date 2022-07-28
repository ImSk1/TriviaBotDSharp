using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TriviaBotDSharp.API.Services
{
    public interface IAPIService
    {
        Task<T> GetDataAsync<T>(string url);
    }
}
