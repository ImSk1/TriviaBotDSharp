﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TriviaBotDSharp.DAL.Models
{
    public class PlayerProfile
    {
        [Key]
        public int Id { get; set; }
        public ulong DiscordId { get; set; }
        public ulong GuildId { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
               
    }
}
