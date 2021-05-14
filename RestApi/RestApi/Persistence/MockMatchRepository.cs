using RestApi.Models;
using System;
using System.Collections.Generic;

namespace RestApi.Persistence
{
    public class MockMatchRepository : IMatchRepository
    {
        public void Add(Match entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Match entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Match> GetAll()
        {
            return new List<Match>
            {
                new Match
                {
                    Id = 1,
                    HomeTeam = "home-team1",
                    GuestTeam = "guest-team1",
                    AvailableSeats = 8,
                    MatchType = MatchType.Final,
                    TicketPrice = 10.6f
                },
                new Match
                {
                    Id = 2,
                    HomeTeam = "home-team2",
                    GuestTeam = "guest-team2",
                    AvailableSeats = 16,
                    MatchType = MatchType.Quarterfinals,
                    TicketPrice = 21.2f
                }
            };
        }

        public Match GetById(int id)
        {
            return new Match
            {
                Id = id,
                HomeTeam = "home-team",
                GuestTeam = "guest-team",
                AvailableSeats = 8,
                MatchType = MatchType.Final,
                TicketPrice = 10.6f
            };
        }

        public bool SaveChages()
        {
            return true;
        }

        public void Update(Match entity)
        {
            throw new NotImplementedException();
        }
    }
}