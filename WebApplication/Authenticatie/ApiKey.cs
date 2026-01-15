using AspNetCore.Authentication.ApiKey;
using System.Security.Claims;

namespace WebApplication1.Authenticatie
{
    public class ApiKey : IApiKey
    {
        // Required property: Key
        public string Key { get; set; } = string.Empty;

        // Required property: Name
        public string Name { get; set; } = string.Empty;

        // Required property: OwnerName
        public string OwnerName { get; set; } = string.Empty;

        // Required property: Claims
        public IReadOnlyCollection<Claim> Claims { get; set; } = Array.Empty<Claim>();

        // Optional property: Roles
        public IReadOnlyCollection<string> Roles { get; set; } = Array.Empty<string>();
    }


    public class DatabaseApiKeyProvider : IApiKeyProvider
    {
        // Simulated list - replace with DB call in production
        private readonly List<ApiKey> _apiKeys = new()
        {
            new ApiKey { Key = "your-secure-api-key-here-12345", Name = "APIClient1" },
        };

        /// <summary>
        /// Validates if the provided API key exists and is valid.
        /// </summary>
        public async Task<IApiKey?> IsValidAsync(string key)
        {
            var apiKey = _apiKeys.FirstOrDefault(k => k.Key == key);
            return apiKey;
        }

        /// <summary>
        /// Provides additional details about the API key (e.g., metadata).
        /// </summary>
        public async Task<IApiKey?> ProvideAsync(string key)
        {
            return await IsValidAsync(key); // Reuse logic
        }
    }
}
