using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TriviaBotDSharp.Core.Services.APIServices.Contracts;

namespace TriviaBotDSharp.Core.Services.APIServices
{
    public class APIService : IAPIService
    {
        public async Task<T> GetDataAsync<T>(string url)
        {
            var json = await new WebClient().DownloadStringTaskAsync(url);
            if (string.IsNullOrEmpty(json))
            {
                Console.WriteLine("Json is empty");
            }
            var triviaObj = JsonConvert.DeserializeObject<T>(json);           
            return triviaObj;
        }
    }
}
