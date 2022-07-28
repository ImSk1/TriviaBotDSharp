using System;
using System.Collections.Generic;
using System.Text;

namespace TriviaBotDSharp.Core.Models
{
    public class PlayerProfileDBO
    {
        public ulong DiscordId { get; set; }
        public ulong GuildId { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
    }
}
