using AutoMapper;
using FootballAPI.Data.Entities;
using FootballAPI.Data.Repository;
using FootballAPI.Exceptions;
using FootballAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballAPI.Services
{
    public class TeamService : ITeamService
    {
        private IFootballRepository _footballRepository;
        private IMapper _mapper;

        public TeamService(IFootballRepository footballRepository, IMapper mapper)
        {
            _footballRepository = footballRepository;
            _mapper = mapper;
        }
        private HashSet<string> _cupsValues = new HashSet<string> { "champions_league", "europa_league", "copa_del_rey", "fa_cup", "dfb_pokal" };

        public TeamModel CreateTeam(TeamModel team)
        {
            var teamEntity = _mapper.Map<TeamEntity>(team);
            var newTeamEntity = _footballRepository.CreateTeam(teamEntity);
            var newTeamModel = _mapper.Map<TeamModel>(newTeamEntity);
            return newTeamModel;
        }

        public TeamModel DeleteTeam(int teamId)
        {
            var teamEntityDeleted = _footballRepository.DeleteTeam(teamId);
            if (teamEntityDeleted == null)
            {
                throw new NotFoundElementException($"The team with id:{teamId} doesn't exist in the repository.");
            }
            var teamModelDeleted = _mapper.Map<TeamModel>(teamEntityDeleted);
            return teamModelDeleted;
        }

        public IEnumerable<TeamModel> GetTeams(string cup)//para el endpoint de las copas
        {
            if (cup!= null && !_cupsValues.Contains(cup.ToLower()))
                throw new InvalidElementOperationException($"invalid orderBy value : {_cupsValues}. The allowed values for param are: {string.Join(',', _cupsValues)}");

            var teamsCupEntity = _footballRepository.GetTeams(cup);
            var teamsCupModel = _mapper.Map<IEnumerable<TeamModel>>(teamsCupEntity);
            return teamsCupModel;
        }

        public TeamModel GetTeam(int teamId)
        {
            var teamEntity = _footballRepository.GetTeam(teamId);
            if (teamEntity == null)
            {
                throw new NotFoundElementException($"The team with id:{teamId} doesn't exist in the repository.");
            }
            var teamModel = _mapper.Map<TeamModel>(teamEntity);
            return teamModel;
        }

        public TeamModel UpdateTeam(int teamId, TeamModel team)
        {
            var teamEntity = _mapper.Map<TeamEntity>(team);
            var teamUpdatedEntity = _footballRepository.UpdateTeam(teamId, teamEntity);
            if (teamUpdatedEntity == null)
            {
                throw new NotFoundElementException($"The team with id:{teamId} doesn't exist in the repository.");
            }
            var teamUpdatedModel = _mapper.Map<TeamModel>(teamUpdatedEntity);
            return teamUpdatedModel;
        }

        public IEnumerable<TeamModel> UpdateTeamGame(int localTeamId, int visitorTeamId, string result, int winnerId)//endpoint
        {
            if (result == null || (result.ToLower() != "win" && result.ToLower() != "draw"))
                throw new InvalidElementOperationException("Invalid result value, the allowed params are: win and draw");
           
            if (localTeamId == visitorTeamId)
                throw new InvalidElementOperationException("Invalid Id' values, the teams must to be diferents.");

            if (result == "win" && (winnerId != localTeamId && winnerId != visitorTeamId))
                throw new InvalidElementOperationException("Invalid winner Id.");

            if (result == "draw" && winnerId != 0)
                throw new InvalidElementOperationException("Invalid operation, Winner Id have to be empty.");

            var bothTeamsEntity = _footballRepository.UpdateTeamGame(localTeamId, visitorTeamId, result, winnerId);
            if (bothTeamsEntity == null)
            {
                throw new NotFoundElementException($"Invalid Id's.");
            }
            var bothTeamsModel = _mapper.Map<IEnumerable<TeamModel>>(bothTeamsEntity);
            return bothTeamsModel;
        }
    }
}
