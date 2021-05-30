using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestApi.Infrastructure.Mapper;
using RestApi.Models;
using RestApi.Models.Dtos;
using RestApi.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpPost("matches/filtered")]
        public ActionResult<IEnumerable<Match>> GetFilteredMatcehs(FilterMatchDto filter)
        {
            var matches = _matchRepo.GetAll();

            return Ok(matches
                .Where(m => m.Id == (filter.Id ?? m.Id))
                .Where(m => m.HomeTeam.ToLower().Contains((filter.HomeTeam ?? "").ToLower()))
                .Where(m => m.GuestTeam.ToLower().Contains((filter.GuestTeam ?? "").ToLower()))
                .Where(m => m.MatchType == (filter.MatchType ?? m.MatchType))
                .Where(m => m.TicketPrice == (filter.TicketPrice ?? m.TicketPrice))
                .Where(m => m.AvailableSeats == (filter.AvailableSeats ?? m.AvailableSeats))
                .ToList());
        }

        [HttpPost("matches")]
        public async Task<ActionResult<Match>> AddMatchAsync(CreateMatchDto createMatchDto)
        {
            var match = _mapper.Map<CreateMatchDto, Match>(createMatchDto);
            _matchRepo.Add(match);
            await _matchRepo.SaveChagesAsync();

            return CreatedAtRoute(nameof(GetMatchById), new { match.Id }, match);
        }

        [HttpPut("matches/{id}")]
        public async Task<ActionResult> UpdateMatchAsync(int id, UpdateMatchDto updateMatchDto)
        {
            var matchFromRepo = _matchRepo.GetById(id);

            if (matchFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(updateMatchDto, matchFromRepo);

            _matchRepo.Update(matchFromRepo);
            await _matchRepo.SaveChagesAsync();

            return NoContent();
        }

        [HttpPatch("matches/{id}")]
        public async Task<ActionResult> PatchMatchAsync(int id, JsonPatchDocument<UpdateMatchDto> patchDocument)
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
            await _matchRepo.SaveChagesAsync();

            return NoContent();
        }

        [HttpDelete("matches/{id}")]
        public async Task<ActionResult> DeleteMatchAsync(int id)
        {
            var matchFromRepo = _matchRepo.GetById(id);

            if (matchFromRepo == null)
            {
                return NotFound();
            }

            _matchRepo.Delete(matchFromRepo);
            await _matchRepo.SaveChagesAsync();

            return NoContent();
        }
    }
}