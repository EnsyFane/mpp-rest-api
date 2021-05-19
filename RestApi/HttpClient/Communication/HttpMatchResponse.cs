using BasketballClient.Models;
using System.Net;

namespace BasketballClient.Communication
{
    public class HttpMatchResponse
    {
        public Match Match { get; set; }

        public bool IsSuccess { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}