using FluentAssertions;
using RestApi.Integration.Tests.Infrastructure;
using RestApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace RestApi.Integration.Tests
{
    public class MatchesControllerTests : BaseIntegrationTest
    {
        private readonly string addMatchRoute;
        private readonly string getMatchRoute;
        private readonly string getMatchesRoute;
        private readonly string updateMatchRoute;
        private readonly string deleteMatchRoute;

        private readonly Match defaultMatch = new Match
        {
            HomeTeam = "home-team",
            GuestTeam = "guest-team",
            AvailableSeats = 100,
            MatchType = MatchType.Final,
            TicketPrice = 26.02f
        };

        public MatchesControllerTests()
        {
            addMatchRoute = _client.BaseAddress + "basketball/matches";
            getMatchRoute = _client.BaseAddress + "basketball/matches/{0}";
            getMatchesRoute = _client.BaseAddress + "basketball/matches";
            updateMatchRoute = _client.BaseAddress + "basketball/matches/{0}";
            deleteMatchRoute = _client.BaseAddress + "basketball/matches/{0}";

            _client = _factory.CreateClient();
            ClearDB().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task NoMatches_GetAllMatches_ReturnsEmpty()
        {
            var response = await _client.GetAsync(getMatchesRoute);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var matches = await response.Content.ReadAsAsync<IEnumerable<Match>>();
            matches.Should().BeEmpty();
        }

        [Theory]
        [InlineData(2)]
        [InlineData(100)]
        [InlineData(10000)]
        public async Task NMatches_GetAllMatches_ReturnsMatches(int N)
        {
            var matchesInApp = await AddMatches(N);

            var response = await _client.GetAsync(getMatchesRoute);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var matches = await response.Content.ReadAsAsync<IEnumerable<Match>>();
            matches.Should().HaveCountGreaterOrEqualTo(N);
            matchesInApp.Should().BeSubsetOf(matches);
        }

        [Fact]
        public async Task NoMatches_AddMatch_MatchIsAdded()
        {
            var matchToAdd = new Match
            {
                HomeTeam = "home-team",
                GuestTeam = "guest-team",
                AvailableSeats = 100,
                MatchType = MatchType.Quarterfinals,
                TicketPrice = 20.2f
            };

            var response = await _client.PostAsync(addMatchRoute, JsonContent.Create(matchToAdd));

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var addedMatch = await response.Content.ReadAsAsync<Match>();
            addedMatch.Id.Should().NotBeNull();
            matchToAdd.Id = addedMatch.Id;
            addedMatch.Should().Be(matchToAdd);
        }

        [Fact]
        public async Task NoMatches_AddInvalidMatch_ReturnsBadRequest()
        {
            var matchToAdd = new Match
            {
                HomeTeam = "home-team",
                GuestTeam = "guest-team",
                AvailableSeats = 100,
                MatchType = MatchType.Quarterfinals,
            };

            var response = await _client.PostAsync(addMatchRoute, JsonContent.Create(matchToAdd));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var getAllResponse = await (await _client.GetAsync(getMatchesRoute)).Content.ReadAsAsync<IEnumerable<Match>>();
            getAllResponse.Should().BeEmpty();
        }

        [Fact]
        public async Task NoMatches_AddNullMatch_ReturnsBadRequest()
        {
            Match matchToAdd = null;

            var response = await _client.PostAsync(addMatchRoute, JsonContent.Create(matchToAdd));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var getAllResponse = await (await _client.GetAsync(getMatchesRoute)).Content.ReadAsAsync<IEnumerable<Match>>();
            getAllResponse.Should().BeEmpty();
        }

        [Fact]
        public async Task NoMatches_AddEmptyMatch_ReturnsBadRequest()
        {
            var matchToAdd = new Match();

            var response = await _client.PostAsync(addMatchRoute, JsonContent.Create(matchToAdd));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var getAllResponse = await (await _client.GetAsync(getMatchesRoute)).Content.ReadAsAsync<IEnumerable<Match>>();
            getAllResponse.Should().BeEmpty();
        }

        [Fact]
        public async Task OneMatch_GetMatchById_ReturnsMatch()
        {
            var matchInApp = await AddMatch();

            var response = await _client.GetAsync(getMatchRoute.Replace("{0}", matchInApp.Id.ToString()));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var match = await response.Content.ReadAsAsync<Match>();
            match.Should().Be(matchInApp);
        }

        [Fact]
        public async Task OneMatch_GetMatchByInvalidId_ReturnsBadRequest()
        {
            await AddMatch();

            var response = await _client.GetAsync(getMatchRoute.Replace("{0}", "invalid-id"));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task NoMatch_GetMatchByNonExistentId_ReturnsNotFound()
        {
            var response = await _client.GetAsync(getMatchRoute.Replace("{0}", 1.ToString()));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task OneMatch_UpdateMatch_MatchIsUpdated()
        {
            var oldMatch = new Match
            {
                HomeTeam = "home-team",
                GuestTeam = "guest-team",
                AvailableSeats = 100,
                MatchType = MatchType.Final,
                TicketPrice = 26.02f
            };
            var newMatch = new Match
            {
                HomeTeam = "updated-home-team",
                GuestTeam = "updated-guest-team",
                AvailableSeats = 9000,
                MatchType = MatchType.Qualifying,
                TicketPrice = 2600.02f
            };
            await AddMatch(oldMatch);

            var response = await _client.PutAsync(updateMatchRoute.Replace("{0}", oldMatch.Id.ToString()), JsonContent.Create(newMatch));
            var updatedMatch = await (await _client.GetAsync(getMatchRoute.Replace("{0}", oldMatch.Id.ToString()))).Content.ReadAsAsync<Match>();
            newMatch.Id = oldMatch.Id;

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            updatedMatch.Should().Be(newMatch);
        }

        [Fact]
        public async Task OneMatch_UpdateMatchWithInvalidData_ReturnsBadRequest()
        {
            var oldMatch = new Match
            {
                HomeTeam = "home-team",
                GuestTeam = "guest-team",
                AvailableSeats = 100,
                MatchType = MatchType.Final,
                TicketPrice = 26.02f
            };
            var newMatch = new Match
            {
                HomeTeam = "updated-home-team",
                GuestTeam = "updated-guest-team",
                AvailableSeats = 9000,
                MatchType = MatchType.Qualifying
            };
            await AddMatch(oldMatch);

            var response = await _client.PutAsync(updateMatchRoute.Replace("{0}", oldMatch.Id.ToString()), JsonContent.Create(newMatch));
            var updatedMatch = await (await _client.GetAsync(getMatchRoute.Replace("{0}", oldMatch.Id.ToString()))).Content.ReadAsAsync<Match>();
            newMatch.Id = oldMatch.Id;

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            updatedMatch.Should().NotBe(newMatch);
            updatedMatch.Should().Be(oldMatch);
        }

        [Fact]
        public async Task NoMatch_UpdateMatch_ReturnsNotFound()
        {
            var newMatch = new Match
            {
                HomeTeam = "updated-home-team",
                GuestTeam = "updated-guest-team",
                AvailableSeats = 9000,
                MatchType = MatchType.Qualifying,
                TicketPrice = 2600.02f
            };

            var response = await _client.PutAsync(updateMatchRoute.Replace("{0}", 1.ToString()), JsonContent.Create(newMatch));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task OneMatch_DeleteMatch_MatchIsDeleted()
        {
            var match = await AddMatch();

            var response = await _client.DeleteAsync(deleteMatchRoute.Replace("{0}", match.Id.ToString()));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var findResponse = await _client.GetAsync(getMatchRoute.Replace("{0}", match.Id.ToString()));
            findResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task NoMatch_DeleteNonExistentMatch_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync(deleteMatchRoute.Replace("{0}", 1.ToString()));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<Match> AddMatch()
        {
            var response = await _client.PostAsync(addMatchRoute, JsonContent.Create(defaultMatch));
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var addedMatch = await response.Content.ReadAsAsync<Match>();
            return addedMatch;
        }

        private async Task AddMatch(Match matchToAdd)
        {
            var response = await _client.PostAsync(addMatchRoute, JsonContent.Create(matchToAdd));
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var addedMatch = await response.Content.ReadAsAsync<Match>();
            matchToAdd.Id = addedMatch.Id;
        }

        private async Task<IEnumerable<Match>> AddMatches(int amount)
        {
            var addedMatches = new List<Match>();
            for (int i = 0; i < amount; i++)
            {
                addedMatches.Add(await AddMatch());
            }
            return addedMatches;
        }

        private async Task ClearDB()
        {
            var matches = await (await _client.GetAsync(getMatchesRoute)).Content.ReadAsAsync<IEnumerable<Match>>();
            foreach (var match in matches)
            {
                await _client.DeleteAsync(deleteMatchRoute.Replace("{0}", match.Id.ToString()));
            }
        }
    }
}