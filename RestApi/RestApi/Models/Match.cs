using System.ComponentModel.DataAnnotations;

namespace RestApi.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HomeTeam { get; set; }

        [Required]
        public string GuestTeam { get; set; }

        [Required]
        public MatchType MatchType { get; set; }

        [Required]
        public int AvailableSeats { get; set; }

        [Required]
        public float TicketPrice { get; set; }
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