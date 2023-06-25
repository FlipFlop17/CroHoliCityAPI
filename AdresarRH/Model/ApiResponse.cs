using System.Net;

namespace CroHoliCityAPI.Model
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; } = true;
        public List<string> Errors { get; set; } = new List<string>();
        public object Data { get; set; }

        public ApiResponse() { }
    }
}
