using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Dtos
{
    public class UpdateMatchDto
    {
        [Required]
        public string HomeTeam { get; set; }

        [Required]
        public string GuestTeam { get; set; }

        [Required]
        public MatchType? MatchType { get; set; }

        [Required]
        public int? AvailableSeats { get; set; }

        [Required]
        public float? TicketPrice { get; set; }
    }
}