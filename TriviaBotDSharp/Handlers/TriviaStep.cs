using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TriviaBotDSharp.Handlers;

namespace DiscordBotTutorial.Bots.Handlers.Dialogue.Steps
{
    public class TriviaStep : DialogueStepBase
    {
        private readonly Dictionary<DiscordEmoji, TriviaStepData> _options;

        private DiscordEmoji _selectedEmoji;

        public TriviaStep(string title, string content, Dictionary<DiscordEmoji, TriviaStepData> options) : base(title, content)
        {
            _options = options;
        }

        public override IDialogueStep NextStep => _options[_selectedEmoji].NextStep;

        public Action<DiscordEmoji> OnValidResult { get; set; } = delegate { };

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var cancelEmoji = DiscordEmoji.FromName(client, ":x:");

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = _title,
                Description = $"{_content}",
            };

            embedBuilder.AddField("To Cancel Trivia", "React with the :x: emoji");

            embedBuilder.WithFooter("You have 15 seconds to answer your Question...");

            var interactivity = client.GetInteractivity();

            while (true)
            {
                var embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
                

                foreach (var emoji in _options.Keys)
                {
                    await embed.CreateReactionAsync(emoji).ConfigureAwait(false);
                }

                await embed.CreateReactionAsync(cancelEmoji).ConfigureAwait(false);

                var reactionResult = await interactivity.WaitForReactionAsync(
                    x => _options.ContainsKey(x.Emoji) || x.Emoji == cancelEmoji,
                    embed,
                    user,
                    new TimeSpan(0, 0, 15)

                ).ConfigureAwait(false);
                if (reactionResult.TimedOut)
                {
                    return true;
                }
                if (reactionResult.Result.Emoji == cancelEmoji)
                {
                    return true;
                }

                _selectedEmoji = reactionResult.Result.Emoji;

                OnValidResult(_selectedEmoji);

                return false;
            }
        }
    }

    public class TriviaStepData
    {
        public string Content { get; set; }
        public IDialogueStep NextStep { get; set; }
    }
}
