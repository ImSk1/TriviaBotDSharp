using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using TriviaBotDSharp.Core.Services.ProfileServices.Contracts;
using TriviaBotDSharp.DAL.Models;

namespace TriviaBotDSharp.CommandsFolder
{
    public class ProfileCommands : BaseCommandModule
    {
        private readonly IProfileService _profileService;
        public ProfileCommands(IProfileService profileService)
        {
            _profileService = profileService;
        }
        [Command("profile")]
        public async Task Profile(CommandContext ctx)
        {
            await GetProfileToDisplayAsync(ctx, ctx.Member.Id);
        }
        [Command("profile")]
        public async Task Profile(CommandContext ctx, DiscordMember member)
        {
            await GetProfileToDisplayAsync(ctx, member.Id);
        }

        private async Task GetProfileToDisplayAsync(CommandContext ctx, ulong memberId)
        {
            PlayerProfile profile = await _profileService.GetProfileByName(memberId, ctx.Guild.Id).ConfigureAwait(false);
            DiscordMember member = ctx.Guild.Members[profile.DiscordId];

            var profileEmbed = new DiscordEmbedBuilder
            {
                Title = $"{member.DisplayName}'s Profile",
                ImageUrl = member.AvatarUrl
            };
            profileEmbed.AddField("Correct answers", profile.CorrectAnswers.ToString());
            profileEmbed.AddField("Incorrect answers", profile.WrongAnswers.ToString());

            await ctx.Channel.SendMessageAsync(embed: profileEmbed).ConfigureAwait(false);
        }
    }
}
