﻿using Newtonsoft.Json;

namespace TriviaBotDSharp
{


    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
    }
}