using System.Net.Http.Json;
using GymTracker.Core.Interfaces;
using GymTracker.Core.Models;

namespace GymTracker.Infrastructure.Services;

public class ExerciseService : IExerciseService
{
    private readonly HttpClient _http;

    public ExerciseService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Exercise>> GetAllAsync()
    {
        var response = await _http.GetFromJsonAsync<ApiResponse>("api/v1/exercises?limit=100&offset=0");
        return response?.Data ?? new List<Exercise>();
    }

    public async Task<Exercise?> GetByIdAsync(string id)
    {
        var response = await _http.GetFromJsonAsync<ExerciseResponse>($"api/v1/exercises/{id}");
        return response?.Data;
    }


    public async Task<List<Exercise>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return await GetAllAsync();
        var url = $"api/v1/exercises/search?{Uri.EscapeDataString(query)}&limit=100";
        var response = await _http.GetFromJsonAsync<ApiResponse>(url);
        return response?.Data ?? new List<Exercise>();
    }


    public async Task<List<Exercise>> FilterAsync(string? bodyPart, string? equipment)
    {
        var query = new List<string>();
        if (!string.IsNullOrEmpty(bodyPart))
            query.Add($"bodyParts={Uri.EscapeDataString(bodyPart)}");
        
        if (!string.IsNullOrEmpty(equipment))
            query.Add($"equipment={Uri.EscapeDataString(equipment)}");
        query.Add("limit=100");
        
        
        var url = "api/v1/exercises/filter?" + string.Join("&", query);
        var response = await _http.GetFromJsonAsync<ApiResponse>(url);
        return response?.Data ?? new List<Exercise>();
    }

    
    
    
}
