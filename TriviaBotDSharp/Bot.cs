using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TriviaBotDSharp
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public async Task RunAsync()
        {
            var config = new DiscordConfiguration
            {
                Token = "OTUxNTI4MjU0NDkyMjYyNDYw.Yioxvg.VY2oXwAgIKIINo7sC1VRyCIexLE",
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged,
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
                StringPrefixes = BotConstants.STRINGPREFIXES,
                EnableMentionPrefix = true,
                EnableDms = false,               
            };
            var commands = Client.UseCommandsNext(commandsConfig);
            commands.RegisterCommands<Commands>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }
        
    }
}
