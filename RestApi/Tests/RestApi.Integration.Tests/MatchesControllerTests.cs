using FluentAssertions;
using RestApi.Integration.Tests.Infrastructure;
using RestApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RestApi.Integration.Tests
{
    public class MatchesControllerTests : BaseIntegrationTest
    {
        private readonly Uri addMatchRoute;
        private readonly Uri getMatchRoute;
        private readonly Uri getMatchesRoute;
        private readonly Uri updateMatchRoute;
        private readonly Uri deleteMatchRoute;

        public MatchesControllerTests()
        {
            addMatchRoute = new Uri(_client.BaseAddress + "basketball/matches");
            getMatchRoute = new Uri(_client.BaseAddress + "basketball/matches/{0}");
            getMatchesRoute = new Uri(_client.BaseAddress + "basketball/matches");
            updateMatchRoute = new Uri(_client.BaseAddress + "basketball/matches/{0}");
            deleteMatchRoute = new Uri(_client.BaseAddress + "basketball/matches/{0}");
        }

        [Fact]
        public async Task NoMatches_GetAllMatches_ReturnsEmpty()
        {
            var response = await _client.GetAsync(getMatchesRoute);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var matches = await response.Content.ReadAsAsync<IEnumerable<Match>>();
            matches.Should().BeEmpty();
        }
    }
}