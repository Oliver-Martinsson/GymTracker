using System.Text.Json.Serialization;

namespace GymTracker.Core.Models
{
    public class ExerciseResponse
    {
        [JsonPropertyName("data")]
        public Exercise? Data { get; set; }
    }
}
