using BasketballClient.Models;
using System.Collections.Generic;
using System.Net;

namespace BasketballClient.Communication
{
    public class HttpMatchesResponse
    {
        public IEnumerable<Match> Matches { get; set; }

        public bool IsSuccess { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}