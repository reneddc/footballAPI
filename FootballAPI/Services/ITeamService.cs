using FootballAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballAPI.Services
{
    public interface ITeamService
    {
        TeamModel GetTeam(int teamId);
        TeamModel CreateTeam(TeamModel team);
        TeamModel UpdateTeam(int teamId, TeamModel team);
        TeamModel DeleteTeam(int teamId);
        IEnumerable<TeamModel> GetTeams(string cup);
        IEnumerable<TeamModel> UpdateTeamGame(int localTeamId, int visitorTeamId, string result, int winnerId);
    }
}
