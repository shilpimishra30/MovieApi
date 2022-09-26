using System.Net;
using System.Text.Json;

namespace MovieApi.Common
{
    public class ErrorDetails
    {
        public string Type { get; set; }

        public int Status { get; set; }

        public string Instance { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
