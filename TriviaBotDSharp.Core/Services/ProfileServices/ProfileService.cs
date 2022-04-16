using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaBotDSharp.Core.Services.ProfileServices.Contracts;
using TriviaBotDSharp.DAL;
using TriviaBotDSharp.DAL.Models;

namespace TriviaBotDSharp.Core.Services.ProfileServices
{
    public class ProfileService : IProfileService
    {
        private readonly TriviaContext _context;
        public ProfileService(TriviaContext context)
        {
            _context = context;
        }
        
        public async Task<PlayerProfile> GetProfileByName(ulong discordId, ulong guildId)
        {
            var profile = await _context.Profiles
                .Where(x => x.GuildId == guildId)
                .FirstOrDefaultAsync(x => x.DiscordId == discordId).ConfigureAwait(false);
            if (profile != null) return profile;

            profile = new PlayerProfile
            {
                DiscordId = discordId,
                GuildId = guildId
            };
            _context.Add(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return profile;
        }
        public async Task IncreaseCorrectAnswers(ulong memberId, ulong guildId)
        {
            var profile = await GetProfileByName(memberId, guildId);
            profile.CorrectAnswers++;
            _context.Update(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);

        }
        public async Task IncreaseIncorrectAnswers(ulong memberId, ulong guildId)
        {
            var profile = await GetProfileByName(memberId, guildId);
            profile.WrongAnswers++;
            _context.Update(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
