using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballAPI.Exceptions;
using FootballAPI.Models;
using FootballAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootballAPI.Controllers
{
    [Route("api/[controller]")]
    public class TeamsController : Controller
    {
        private ITeamService _teamService;
        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }


        [HttpPost]
        public ActionResult<TeamModel> PostTeam([FromBody] TeamModel teamModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var newTeam = _teamService.CreateTeam(teamModel);
                return Created($"/api/teams/{teamModel.Id}", newTeam);
            }
            catch (NotFoundElementException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something happend.");
            }
        }


        [HttpGet("{teamId:int}")]
        public ActionResult<TeamModel> GetTeam(int teamId)
        {
            try
            {
                var team = _teamService.GetTeam(teamId);
                return Ok(team);
            }
            catch (NotFoundElementException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidElementOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something happend.");
            }
        }

        [HttpDelete("{teamId:int}")]
        public  ActionResult DeleteTeam(int teamId)
        {
            try
            {
                var teamDeleted = _teamService.DeleteTeam(teamId);
                return Ok();
            }
            catch (NotFoundElementException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something happend.");
            }
        }


        [HttpPut("{teamId:int}")]//update simple
        public ActionResult<TeamModel> UpdateTeam(int teamId, [FromBody] TeamModel team)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    if (ModelState.ContainsKey("position"))
                    {
                        return BadRequest(ModelState["position"].Errors);
                    }
                }
                var teamUpdated = _teamService.UpdateTeam(teamId, team);
                return Ok(teamUpdated);
            }
            catch (NotFoundElementException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something happend.");
            }
        }


        //Endpoints

        //1. Seleccionar a los equipos que pueden particiar en una determinada copa

        //Requisitos (No es el formato oficial)
        //-> CHAMPIONS LEAGUE:      Estar en las primeras 4 posiciones de sus respectivas ligas 
        //-> EUROPA LEAGUE:         Estar en las posiciones 7 Y 5 de sus respectivas ligas 
        //-> Copa del Rey:          10 primeros equipos de la Liga BBVA
        //-> FA Cup:                Todos los equipos de la Premier League
        //-> DFB Pokal:             8 primeros equipos de la Bundesliga

        //localhost:5500/api/teams?cup=champions_league         ->champions league
        //localhost:5500/api/teams?cup=europa_league            ->europa league
        //localhost:5500/api/teams?cup=copa_del_rey             ->copa del rey
        //localhost:5500/api/teams?cup=fa_cup                   ->fa cup
        //localhost:5500/api/teams?cup=dfb_pokal                ->dfb pokal

        [HttpGet]
        public ActionResult<IEnumerable<TeamModel>> GetTeams(string cup)
        {
            try
            {
                var teams = _teamService.GetTeams(cup);
                return Ok(teams);
            }
            catch (NotFoundElementException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidElementOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something happend.");
            }
        }

        //2. Simular un partido y que se modifique su puntuación según el resultado de ambos equipos 
        //(Juego es un recurso aparte de team, pero por temas de tiempo lo simularé en el recurso team)
        //Victoria +3 pts
        //Derrota +0pts
        //empate +1pts

        //Requisitos / Detalles
        //-> Deben ser equipos de la misma liga porque es un partido de liga
        //-> Primero se debe ingresar el id de los 2 equipos
        //-> Después el resultado (win) y el id del ganador
        //-> Se es empate escribir draw

        //localhost:5500/api/teams?localTeamId=6visitorTeamId=1&result=win&winnerId=1   -> victoria/derrota
        //localhost:5500/api/teams?localTeamId=6visitorTeamId=1&result=draw             -> empate

        [HttpPut]
        public ActionResult<TeamModel> UpdateTeamGame(int localTeamId, int VisitorTeamId, string result, int winnerId)
        {
            try
            {
                var bothTeams = _teamService.UpdateTeamGame(localTeamId, VisitorTeamId, result, winnerId);
                return Ok(bothTeams);
            }
            catch (NotFoundElementException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidElementOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something happend.");
            }
        }
    }
}
