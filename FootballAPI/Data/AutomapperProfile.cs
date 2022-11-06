using AutoMapper;
using FootballAPI.Data.Entities;
using FootballAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballAPI.Data
{
    public class AutomapperProfile:Profile
    {
        public AutomapperProfile()
        {
            this.CreateMap<TeamEntity, TeamModel>()
                .ReverseMap();
        }
    }
}
