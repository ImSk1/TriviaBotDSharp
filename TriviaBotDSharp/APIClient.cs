using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TriviaController
{
    public class APIClient
    {
        public APIClient(string url)
        {
            URL = url;
        }
        public string URL { get; private set; }
        
        public T GetDataAsync<T>()
        {
            var json = new WebClient().DownloadString(URL);
            var triviaObj = JsonConvert.DeserializeObject<T>(json);
            return triviaObj;
        }
      
    }
}
