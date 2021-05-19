using System;
using System.Text.Json.Serialization;

namespace BasketballClient.Models
{
    public class Match
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("homeTeam")]
        public string HomeTeam { get; set; }

        [JsonPropertyName("guestTeam")]
        public string GuestTeam { get; set; }

        [JsonPropertyName("matchType")]
        public MatchType MatchType { get; set; }

        [JsonPropertyName("availableSeats")]
        public int? AvailableSeats { get; set; }

        [JsonPropertyName("ticketPrice")]
        public float? TicketPrice { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Match match &&
                   Id == match.Id &&
                   HomeTeam == match.HomeTeam &&
                   GuestTeam == match.GuestTeam &&
                   MatchType == match.MatchType &&
                   AvailableSeats == match.AvailableSeats &&
                   TicketPrice == match.TicketPrice;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, HomeTeam, GuestTeam, MatchType, AvailableSeats, TicketPrice);
        }
    }

    public enum MatchType
    {
        Qualifying,
        Round1,
        Round2,
        Round3,
        Quarterfinals,
        Semifinals,
        Final
    }
}