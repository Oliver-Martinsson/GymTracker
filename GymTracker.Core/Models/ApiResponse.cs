using System.Text.Json.Serialization;

namespace GymTracker.Core.Models
{
    public class ApiResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public List<Exercise> Data { get; set; } = new();
    }
}
