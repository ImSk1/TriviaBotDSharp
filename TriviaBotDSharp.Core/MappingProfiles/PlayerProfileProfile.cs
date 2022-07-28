using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TriviaBotDSharp.Core.Models;
using TriviaBotDSharp.DAL.Models;

namespace TriviaBotDSharp.Core.MappingProfiles
{
    public class PlayerProfileProfile : Profile
    {
        public PlayerProfileProfile()
        {
            CreateMap<PlayerProfile, PlayerProfileDBO>().ReverseMap();
        }
    }
}
