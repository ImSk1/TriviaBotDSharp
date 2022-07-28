using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaBotDSharp.Core.Models;
using TriviaBotDSharp.Core.Services.ProfileServices.Contracts;
using TriviaBotDSharp.DAL;
using TriviaBotDSharp.DAL.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;



namespace TriviaBotDSharp.Core.Services.ProfileServices
{
    public class ProfileService : IProfileService
    {
        private readonly TriviaContext _context;
        private readonly IMapper _mapper;
        public ProfileService(TriviaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PlayerProfileDBO> GetProfileByName(ulong discordId, ulong guildId)
        {
            List<PlayerProfileDBO> profileDbos;
            try
            {
                profileDbos = _context.Profiles                               
                               .ProjectTo<PlayerProfileDBO>(_mapper.ConfigurationProvider).ToList();
            }
            catch (Exception)
            {

                throw;
            }
           
            var profileDbo = profileDbos.Where(x => x.GuildId == guildId).ToList().FirstOrDefault(x => x.DiscordId == discordId);
            if (profileDbo != null)
            {                
                return profileDbo;
            }
            var profile = new PlayerProfile
            {
                DiscordId = discordId,
                GuildId = guildId
            };
            _context.Add(profile);
            profileDbo = _mapper.Map<PlayerProfileDBO>(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return profileDbo;
        }
        public async Task IncreaseCorrectAnswers(ulong memberId, ulong guildId)
        {
            var profileDbo = await GetProfileByName(memberId, guildId);
            var profile = _context.Profiles.FirstOrDefault(x => x.GuildId == profileDbo.GuildId && x.DiscordId == profileDbo.DiscordId);
            profile.CorrectAnswers++;
            _context.Update(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);

        }
        public async Task IncreaseIncorrectAnswers(ulong memberId, ulong guildId)
        {
            var profileDbo = await GetProfileByName(memberId, guildId);
            var profile = _context.Profiles.FirstOrDefault(x => x.GuildId == profileDbo.GuildId && x.DiscordId == profileDbo.DiscordId);
            profile.WrongAnswers++;
            _context.Update(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
