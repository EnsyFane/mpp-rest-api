namespace RestApi.Models.Dtos
{
    public class CreateMatchDto
    {
        public string HomeTeam { get; set; }
        public string GuestTeam { get; set; }
        public MatchType MatchType { get; set; }
        public int AvailableSeats { get; set; }
        public float TicketPrice { get; set; }
    }
}