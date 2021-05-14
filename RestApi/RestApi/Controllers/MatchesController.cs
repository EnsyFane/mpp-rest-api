﻿using Microsoft.AspNetCore.Mvc;
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
    }
}