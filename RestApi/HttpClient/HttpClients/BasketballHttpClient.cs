using BasketballClient.Communication;
using BasketballClient.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BasketballClient.HttpClients
{
    public class BasketballHttpClient : BaseHttpClient
    {
        private const string addMatchRoute = "/matches";
        private const string getMatchRoute = "/matches/{0}";
        private const string getMatchesRoute = "/matches";
        private const string updateMatchRoute = "/matches/{0}";
        private const string deleteMatchRoute = "/matches/{0}";

        public BasketballHttpClient(string baseRestApiUrl) : base(baseRestApiUrl)
        {
        }

        public async Task<HttpMatchResponse> AddMatch(Match match)
        {
            var result = await PostAsync(addMatchRoute, match);
            return await GetMatchResponseFromResponseMessage(result);
        }

        public async Task<HttpMatchResponse> GetMatchById(int id)
        {
            var result = await GetAsync(string.Format(getMatchRoute, id));
            return await GetMatchResponseFromResponseMessage(result);
        }

        public async Task<HttpMatchesResponse> GetMatches()
        {
            var result = await GetAsync(getMatchesRoute);
            if (result.IsSuccessStatusCode)
            {
                var contentString = await result.Content.ReadAsStringAsync();
                var matchResult = Deserialize<IEnumerable<Match>>(contentString);

                return new HttpMatchesResponse
                {
                    Matches = matchResult,
                    StatusCode = result.StatusCode,
                    IsSuccess = true
                };
            }

            return new HttpMatchesResponse
            {
                StatusCode = result.StatusCode,
                IsSuccess = false,
                ErrorMessage = await result.Content.ReadAsStringAsync()
            };
        }

        public async Task<HttpResponseMessage> UpdateMatch(int id, Match match)
        {
            return await PutAsync(string.Format(updateMatchRoute, id), match);
        }

        public async Task<HttpResponseMessage> DeleteMatch(int id)
        {
            return await DeleteAsync(string.Format(deleteMatchRoute, id));
        }

        private async Task<HttpMatchResponse> GetMatchResponseFromResponseMessage(HttpResponseMessage result)
        {
            if (result.IsSuccessStatusCode)
            {
                var contentString = await result.Content.ReadAsStringAsync();
                var matchResult = Deserialize<Match>(contentString);

                return new HttpMatchResponse
                {
                    Match = matchResult,
                    StatusCode = result.StatusCode,
                    IsSuccess = true
                };
            }

            return new HttpMatchResponse
            {
                StatusCode = result.StatusCode,
                IsSuccess = false,
                ErrorMessage = await result.Content.ReadAsStringAsync()
            };
        }
    }
}