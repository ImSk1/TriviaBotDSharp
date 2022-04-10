using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TriviaBotDSharp.Handlers
{
    public class DialogueHandler
    {
        private readonly DiscordClient _client;
        private readonly DiscordChannel _channel;
        private readonly DiscordUser _user;
        private IDialogueStep _currentStep;

        public DialogueHandler(DiscordClient client, DiscordChannel channel, DiscordUser user, IDialogueStep startingStep)
        {
            _client = client;
            _channel = channel;
            _user = user;
            _currentStep = startingStep;
        }

        public async Task<bool> ProcessDialogue()
        {
            while (_currentStep != null)
            {
                bool cancelled = await _currentStep.ProcessStep(_client, _channel, _user).ConfigureAwait(false);
                if (cancelled)
                {
                    var cancelEmbed = new DiscordEmbedBuilder
                    {
                        Title = ":cold_face: Trivia cancelled or time ran out",
                        Description = _user.Mention,
                        Color = DiscordColor.Violet

                    };
                    await _channel.SendMessageAsync(embed: cancelEmbed).ConfigureAwait(false);
                    return false;
                }
                _currentStep = _currentStep.NextStep;
            }
            return true;
        }
    }
}
