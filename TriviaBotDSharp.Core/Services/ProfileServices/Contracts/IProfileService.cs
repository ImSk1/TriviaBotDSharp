using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TriviaBotDSharp.Core.Models;
using TriviaBotDSharp.DAL.Models;

namespace TriviaBotDSharp.Core.Services.ProfileServices.Contracts
{
    public interface IProfileService
    {
        Task<PlayerProfileDBO> GetProfileByName(ulong discordId, ulong guildId);
        Task IncreaseCorrectAnswers(ulong memberId, ulong guildId);
        Task IncreaseIncorrectAnswers(ulong memberId, ulong guildId);
    }
}
