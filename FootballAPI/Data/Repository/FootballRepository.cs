using FootballAPI.Data.Entities;
using FootballAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballAPI.Data.Repository
{
    public class FootballRepository : IFootballRepository
    {
        private IList<TeamEntity> _teams = new List<TeamEntity>();

        public FootballRepository()//no puse los puntos porque los añadí al final, pero inician en 0, así se distingue mejor el endpoint de las victorias y empates
        {
            _teams.Add(new TeamEntity()
            {
                Id = 1,
                name = "FC Barcelona",
                league = "La Liga BBVA",
                position = 9
            });
            _teams.Add(new TeamEntity()
            {
                Id = 2,
                name = "Manchester City",
                league = "Premier League",
                position = 3
            });
            _teams.Add(new TeamEntity()
            {
                Id = 3,
                name = "Manchester United",
                league = "Premier League",
                position = 5
            });
            _teams.Add(new TeamEntity()
            {
                Id = 4,
                name = "Napoli",
                league = "Serie A",
                position = 1
            });
            _teams.Add(new TeamEntity()
            {
                Id = 5,
                name = "PSG",
                league = "League 1",
                position = 1
            });
            _teams.Add(new TeamEntity()
            {
                Id = 6,
                name = "Real Madrid",
                league = "La Liga BBVA",
                position = 1
            });
            _teams.Add(new TeamEntity()
            {
                Id = 7,
                name = "Bayern Munich",
                league = "Bundesliga",
                position = 1
            });
            _teams.Add(new TeamEntity()
            {
                Id = 8,
                name = "Wolfsburg",
                league = "Bundesliga",
                position = 5
            });
        }

        public TeamEntity CreateTeam(TeamEntity team)
        {
            var lastTeam = _teams.OrderByDescending(r => r.Id).FirstOrDefault();
            int nextId = lastTeam != null ? lastTeam.Id + 1 : 1;//Generar el ID
            team.Id = nextId;
            _teams.Add(team);//Añadir team a la lista
            return team;
        }

        public TeamEntity DeleteTeam(int teamId)
        {
            var teamToDelete = _teams.SingleOrDefault(r => r.Id == teamId);
            if (teamToDelete != null)
            {
                _teams.Remove(teamToDelete);
            }
            return teamToDelete;
        }

        public TeamEntity GetTeam(int teamId)
        {
            var teamSelected = _teams.FirstOrDefault(r => r.Id == teamId);
            return teamSelected;
        }

        public TeamEntity UpdateTeam(int teamId, TeamEntity team)
        {
            var teamToUpdate = _teams.SingleOrDefault(r => r.Id == teamId);
            if (teamToUpdate != null)
            {
                teamToUpdate.league = team.league ?? teamToUpdate.league;
                teamToUpdate.name = team.name ?? teamToUpdate.name;
                teamToUpdate.position = team.position;
                teamToUpdate.points = team.points;
            }
            return teamToUpdate;
        }
        public IEnumerable<TeamEntity> GetTeams(string cup) //Para el endpoint de las copas, también es el get simple
        {
            switch (cup)
            {
                case "champions_league":
                    {
                        var teams =
                            from t in _teams
                            where t.position <= 4
                            orderby t.league descending
                            select t;
                        return teams;
                    }
                case "copa_del_rey":
                    {
                        var teams =
                            from t in _teams
                            where t.position <= 10 && t.league == "La Liga BBVA"
                            orderby t.position descending
                            select t;
                        return teams;
                    }
                case "europa_league":
                    {
                        var teams =
                            from t in _teams
                            where t.position <= 7 && t.position > 4
                            orderby t.league descending
                            select t;
                        return teams;
                    }
                case "fa_cup":
                    {
                        var teams =
                            from t in _teams
                            where t.league == "Premier League"
                            orderby t.position descending
                            select t;
                        return teams;
                    }
                case "dfb_pokal":
                    {
                        var teams =
                            from t in _teams
                            where t.league == "Bundesliga" && t.position < 9
                            orderby t.position descending
                            select t;
                        return teams;
                    }
                default:
                    {
                        return _teams;
                    }
            }
        }
        public IEnumerable<TeamEntity> UpdateTeamGame(int localTeamId, int VisitorTeamId, string result, int winnerId)
        {
            IEnumerable<TeamEntity> bothTeams = null;
            var team1 = _teams.SingleOrDefault(r => r.Id == localTeamId);
            var team2 = _teams.SingleOrDefault(r => r.Id == VisitorTeamId);
            var winnerTeam = _teams.SingleOrDefault(r => r.Id == winnerId);
            if (team1 != null && team2 != null && team1.league == team2.league)
            {
                if (result == "win")
                {
                    if (winnerId == team1.Id)
                        team1.points = team1.points + 3;
                    else
                        team2.points = team2.points + 3;
                }
                else
                {
                    team1.points = team1.points + 1;
                    team2.points = team2.points + 1;
                }
                bothTeams =
                        from t in _teams
                        where t.Id == team1.Id || t.Id == team2.Id
                        orderby t.Id descending
                        select t;
            }
            return bothTeams;
        }
    }
}
