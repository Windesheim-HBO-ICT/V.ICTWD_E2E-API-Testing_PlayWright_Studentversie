using Microsoft.Playwright;

namespace PlaywrightTests
{
 
    public class BaseAuthenticatedAPITest : BaseAPITest
    {
        protected override async Task CreateAPIRequestContext()
        {
            var headers = new Dictionary<string, string>();

            headers.Add("Accept", "application/json");
            // Add authorization token to all requests.
            headers.Add("X-API-Key", "your-secure-api-key-here-12345");

            Request = await this.Playwright.APIRequest.NewContextAsync(new()
            {
                // All requests we send go to this API endpoint.
                BaseURL = "https://localhost:7172/",
                ExtraHTTPHeaders = headers,
                IgnoreHTTPSErrors = true
            });
        }
    }
}
