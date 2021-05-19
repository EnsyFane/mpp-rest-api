using BasketballClient.HttpClients;
using BasketballClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BasketballClient
{
    public class Program
    {
        private static readonly JsonSerializerOptions _serializerSettings = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter(),
            },
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };

        private static Config _config;

        private static void Main(string[] args)
        {
            _config = Config.ReadConfig("appsettings.json");
            TestBasketballApi().GetAwaiter().GetResult();
        }

        private static async Task TestBasketballApi()
        {
            var basketballClient = new BasketballHttpClient(_config.BasketballBaseUrl);

            var matchToAdd = new Match
            {
                HomeTeam = "My team",
                GuestTeam = "Your team",
                AvailableSeats = 100,
                MatchType = MatchType.Qualifying,
                TicketPrice = 1.2f
            };

            // Create match
            var createdMatch = await AddMatch(basketballClient, matchToAdd);
            if (createdMatch == null)
            {
                return;
            }

            if (!createdMatch.Id.HasValue)
            {
                WriteRed("Match ID was not returned by server. Aborting future operations...");
                return;
            }

            // Get by id
            var foundMatch = await GetMatchById(basketballClient, createdMatch.Id.Value);
            if (foundMatch == null)
            {
                return;
            }

            if (!foundMatch.Equals(createdMatch))
            {
                WriteRed("The created match and the found match are different.");
                return;
            }

            // Get all
            var foundMatches = await GetMatches(basketballClient);
            if (foundMatches.FirstOrDefault(m => m.Equals(foundMatch)) == null)
            {
                WriteRed("The created match was not found in the list of all matches.");
                return;
            }

            // Update
            var matchUpdate = new Match
            {
                HomeTeam = "My updated team",
                GuestTeam = "Your updated team",
                AvailableSeats = 9001,
                MatchType = MatchType.Final,
                TicketPrice = 420.0f
            };
            if (!await UpdateMatch(basketballClient, createdMatch.Id.Value, matchUpdate))
            {
                return;
            }

            var updatedMatch = await GetMatchById(basketballClient, createdMatch.Id.Value);
            matchUpdate.Id = updatedMatch.Id;
            if (!updatedMatch.Equals(matchUpdate))
            {
                WriteRed("Match was not updated correctly.");
                return;
            }

            // Delete
            if (!await DeleteMatch(basketballClient, createdMatch.Id.Value))
            {
                return;
            }

            var finalMatches = await GetMatches(basketballClient);
            if (finalMatches.FirstOrDefault(m => m.Equals(updatedMatch)) != null)
            {
                WriteRed("Match was not actually deleted.");
                return;
            }

            Console.WriteLine();
            WriteGreen("Test ended successfully.");
        }

        private static async Task<Match> AddMatch(BasketballHttpClient basketballClient, Match matchToAdd)
        {
            Console.WriteLine("Adding match: ");
            Console.WriteLine(JsonSerializer.Serialize(matchToAdd, _serializerSettings));

            var matchAddResult = await basketballClient.AddMatch(matchToAdd);
            if (!matchAddResult.IsSuccess)
            {
                WriteRed("Match not added.");
                Console.WriteLine($"{matchAddResult.StatusCode}:{matchAddResult.ErrorMessage}");
                Console.WriteLine();
                return null;
            }

            var createdMatch = matchAddResult.Match;

            WriteGreen("Match added.");
            Console.WriteLine(JsonSerializer.Serialize(createdMatch));
            Console.WriteLine();

            return createdMatch;
        }

        private static async Task<Match> GetMatchById(BasketballHttpClient basketballClient, int matchId)
        {
            Console.WriteLine("Getting match with id: " + matchId);

            var matchGetResult = await basketballClient.GetMatchById(matchId);
            if (!matchGetResult.IsSuccess)
            {
                WriteRed("Match not found.");
                Console.WriteLine($"{matchGetResult.StatusCode}:{matchGetResult.ErrorMessage}");
                Console.WriteLine();
                return null;
            }

            var foundMatch = matchGetResult.Match;

            WriteGreen("Match found.");
            Console.WriteLine(JsonSerializer.Serialize(foundMatch));
            Console.WriteLine();

            return foundMatch;
        }

        private static async Task<IEnumerable<Match>> GetMatches(BasketballHttpClient basketballClient)
        {
            Console.WriteLine("Getting matches");

            var matchGetResult = await basketballClient.GetMatches();
            if (!matchGetResult.IsSuccess)
            {
                WriteRed("No matches found.");
                Console.WriteLine($"{matchGetResult.StatusCode}:{matchGetResult.ErrorMessage}");
                Console.WriteLine();
                return null;
            }

            var foundMatches = matchGetResult.Matches;

            WriteGreen("Matches found.");
            Console.WriteLine(JsonSerializer.Serialize(foundMatches));
            Console.WriteLine();

            return foundMatches;
        }

        private static async Task<bool> DeleteMatch(BasketballHttpClient basketballClient, int matchId)
        {
            Console.WriteLine("Deleting match with id " + matchId);

            var matchDeleteResult = await basketballClient.DeleteMatch(matchId);

            if (!matchDeleteResult.IsSuccessStatusCode)
            {
                WriteRed("Match not deleted.");
                Console.WriteLine($"{matchDeleteResult.StatusCode}:{await matchDeleteResult.Content.ReadAsStringAsync()}");
                Console.WriteLine();
                return false;
            }

            WriteGreen("Match deleted.");
            Console.WriteLine();
            return true;
        }

        private static async Task<bool> UpdateMatch(BasketballHttpClient basketballClient, int matchId, Match updatedMatch)
        {
            Console.WriteLine("Updating match with id: " + matchId);
            Console.WriteLine(JsonSerializer.Serialize(updatedMatch, _serializerSettings));

            var matchUpdateResult = await basketballClient.UpdateMatch(matchId, updatedMatch);

            if (!matchUpdateResult.IsSuccessStatusCode)
            {
                WriteRed("Match not updated.");
                Console.WriteLine($"{matchUpdateResult.StatusCode}:{await matchUpdateResult.Content.ReadAsStringAsync()}");
                Console.WriteLine();
                return false;
            }

            WriteGreen("Match updated.");
            Console.WriteLine();
            return true;
        }

        private static void WriteGreen(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static void WriteRed(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}