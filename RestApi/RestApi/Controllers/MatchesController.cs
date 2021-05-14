using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestApi.Infrastructure.Mapper;
using RestApi.Models;
using RestApi.Models.Dtos;
using RestApi.Persistence;
using System.Collections.Generic;

namespace RestApi.Controllers
{
    [Route("basketball")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchRepository _matchRepo;
        private readonly IMappingCoordinator _mapper;

        public MatchesController(IMatchRepository matchRepo, IMappingCoordinator mapper)
        {
            _matchRepo = matchRepo;
            _mapper = mapper;
        }

        [HttpGet("matches/{id}", Name = "GetMatchById")]
        public ActionResult<Match> GetMatchById(int id)
        {
            var match = _matchRepo.GetById(id);
            if (match != null)
            {
                return Ok(match);
            }

            return NotFound();
        }

        [HttpGet("matches")]
        public ActionResult<IEnumerable<Match>> GetAllMatches()
        {
            var matches = _matchRepo.GetAll();

            return Ok(matches);
        }

        [HttpPost("matches")]
        public ActionResult<Match> AddMatch(CreateMatchDto createMatchDto)
        {
            var match = _mapper.Map<CreateMatchDto, Match>(createMatchDto);
            _matchRepo.Add(match);
            _matchRepo.SaveChages();

            return CreatedAtRoute(nameof(GetMatchById), new { match.Id }, match);
        }

        [HttpPut("matches/{id}")]
        public ActionResult UpdateMatch(int id, UpdateMatchDto updateMatchDto)
        {
            var matchFromRepo = _matchRepo.GetById(id);

            if (matchFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(updateMatchDto, matchFromRepo);

            _matchRepo.Update(matchFromRepo);
            _matchRepo.SaveChages();

            return NoContent();
        }

        [HttpPatch("matches/{id}")]
        public ActionResult PatchMatch(int id, JsonPatchDocument<UpdateMatchDto> patchDocument)
        {
            var matchFromRepo = _matchRepo.GetById(id);

            if (matchFromRepo == null)
            {
                return NotFound();
            }

            var matchToPatch = _mapper.Map<Match, UpdateMatchDto>(matchFromRepo);
            patchDocument.ApplyTo(matchToPatch, ModelState);
            if (!TryValidateModel(matchToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(matchToPatch, matchFromRepo);

            _matchRepo.Update(matchFromRepo);
            _matchRepo.SaveChages();

            return NoContent();
        }
    }
}