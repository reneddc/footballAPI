using FootballAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballAPI.Data.Repository
{
    public interface IFootballRepository
    {
        //Teams
        TeamEntity GetTeam(int teamId);
        TeamEntity CreateTeam(TeamEntity team);
        TeamEntity UpdateTeam(int teamId, TeamEntity team);
        TeamEntity DeleteTeam(int teamId);
        IEnumerable<TeamEntity> GetTeams(string cup);
        IEnumerable<TeamEntity> UpdateTeamGame(int localTeamId, int visitorTeamId, string result, int winnerId);
    }
}
