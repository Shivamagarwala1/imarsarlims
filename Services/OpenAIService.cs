using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;


namespace iMARSARLIMS.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public OpenAIService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["OpenAI:ApiKey"];
            _baseUrl = configuration["OpenAI:BaseUrl"];
        }

        public async Task<string> GetChatResponseAsync(string prompt)
        {
            var request = new
            {
                model = "o1-preview", // Use the model you prefer
                messages = new[]
                {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = prompt }
            },
                temperature = 0.7
            };

            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync(_baseUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);
                return jsonResponse.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"OpenAI API error: {errorResponse}");
            }
        }

    }
}
