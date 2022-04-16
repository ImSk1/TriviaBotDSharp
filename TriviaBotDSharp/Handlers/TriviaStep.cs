using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TriviaBotDSharp.Core.Services.ProfileServices.Contracts;
using TriviaBotDSharp.Handlers;

namespace DiscordBotTutorial.Bots.Handlers.Dialogue.Steps
{
    public class TriviaStep : DialogueStepBase
    {
        protected readonly string _difficulty;
        protected readonly string _category;
        private readonly Dictionary<DiscordEmoji, TriviaStepData> _options;
        private readonly IProfileService _profileService;

        private DiscordEmoji _selectedEmoji;

        public TriviaStep(string title, string content, string category, string difficulty, Dictionary<DiscordEmoji, TriviaStepData> options, IProfileService profileService) : base(title, content)
        {
            _options = options;
            _difficulty = difficulty;
            _category = category;
            _profileService = profileService;
        }

        public override IDialogueStep NextStep => _options[_selectedEmoji].NextStep;

        public Action<DiscordEmoji> OnValidResult { get; set; } = delegate { };

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var profile = await _profileService.GetProfileByName(user.Id, channel.Guild.Id).ConfigureAwait(false);
            

            var cancelEmoji = DiscordEmoji.FromName(client, ":x:");

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = _title,
                Description = $"{_content}",
            };

            embedBuilder.AddField("Category:", _category);
            embedBuilder.AddField("Difficulty:", _difficulty);
            embedBuilder.AddField("To Cancel Trivia", "React with the :x: emoji");
            
            embedBuilder.WithFooter("You have 15 seconds to answer your Question...", user.GetAvatarUrl(ImageFormat.Jpeg));

            var interactivity = client.GetInteractivity();

            while (true)
            {
                await channel.SendMessageAsync($"{user.Mention} Here is your question: ");
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

                if (_options[_selectedEmoji].AnsweredCorrectly)
                {
                    await _profileService.IncreaseCorrectAnswers(profile.DiscordId, profile.GuildId).ConfigureAwait(false);
                }
                else
                {
                    await _profileService.IncreaseIncorrectAnswers(profile.DiscordId, profile.GuildId).ConfigureAwait(false);
                }

                OnValidResult(_selectedEmoji);

                return false;
            }
        }
    }

    public class TriviaStepData
    {
        public string Content { get; set; }
        
        public IDialogueStep NextStep { get; set; }
        public bool AnsweredCorrectly { get; set; }
    }
}
