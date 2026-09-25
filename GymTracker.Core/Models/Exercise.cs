using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GymTracker.Core.Models
{
    public class Exercise
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("bodyPart")]
        public string BodyPart { get; set; } = string.Empty;

        [JsonPropertyName("equipment")]
        public string Equipment { get; set; } = string.Empty;

        [JsonPropertyName("apparatus")]
        public List<string> Apparatus { get; set; } = new();

        [JsonPropertyName("target")]
        public string Target { get; set; } = string.Empty;

        [JsonPropertyName("secondaryMuscles")]
        public List<string> SecondaryMuscles { get; set; } = new();

        [JsonPropertyName("instructions")]
        public List<string> Instructions { get; set; } = new();

        [JsonPropertyName("gifUrl")]
        public string GifUrl { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("difficulty")]
        public string Difficulty { get; set; } = string.Empty;

        [JsonPropertyName("mechanic")]
        public string Mechanic { get; set; } = string.Empty;

        [JsonPropertyName("force")]
        public string Force { get; set; } = string.Empty;

        [JsonPropertyName("met")]
        public double Met { get; set; }

        [JsonPropertyName("caloriesPerMinute")]
        public double CaloriesPerMinute { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("isUnilateral")]
        public bool IsUnilateral { get; set; }

        [JsonPropertyName("popularityRank")]
        public int PopularityRank { get; set; }

        [JsonPropertyName("recommendedSets")]
        public string RecommendedSets { get; set; } = string.Empty;

        [JsonPropertyName("recommendedReps")]
        public string RecommendedReps { get; set; } = string.Empty;

        [JsonPropertyName("joint_focus")]
        public string JointFocus { get; set; } = string.Empty;

        [JsonPropertyName("intensity_level")]
        public string IntensityLevel { get; set; } = string.Empty;

        [JsonPropertyName("movement_tags")]
        public List<string> MovementTags { get; set; } = new();
    }
}
