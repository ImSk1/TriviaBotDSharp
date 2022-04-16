using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TriviaBotDSharp.CommandsFolder;

namespace TriviaBotDSharp
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public Bot(IServiceProvider services)
        {
            var json = string.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = sr.ReadToEnd();

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug
            };
            Client = new DiscordClient(config);


            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromSeconds(30)
            });

            var commandsConfig = new CommandsNextConfiguration()
            {
                CaseSensitive = false,
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = false,
                Services = services
            };
            var commands = Client.UseCommandsNext(commandsConfig);
            commands.RegisterCommands<Commands>();
            commands.RegisterCommands<ProfileCommands>();

            Client.ConnectAsync();

        }
        
        
    }
}
