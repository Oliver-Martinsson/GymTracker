using System.Text.Json.Serialization;

namespace GymTracker.Core.Models
{
    public class Exercise
    {
        [JsonPropertyName("exerciseId")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("gifUrl")]
        public string GifUrl { get; set; } = string.Empty;

        [JsonPropertyName("bodyParts")]
        public List<string> BodyParts { get; set; } = new();

        [JsonPropertyName("targetMuscles")]
        public List<string> TargetMuscles { get; set; } = new();

        [JsonPropertyName("equipments")]
        public List<string> Equipments { get; set; } = new();

        [JsonPropertyName("secondaryMuscles")]
        public List<string> SecondaryMuscles { get; set; } = new();

        [JsonPropertyName("instructions")]
        public List<string> Instructions { get; set; } = new();
    }
}
