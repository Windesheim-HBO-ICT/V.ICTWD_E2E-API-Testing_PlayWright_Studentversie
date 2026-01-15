using Microsoft.Playwright;
using System.Text.Json;

namespace PlaywrightTests
{
    [TestClass]
    public class BaseAPITest : PlaywrightTest
    {
        protected IAPIRequestContext Request = null!;

        protected static readonly JsonSerializerOptions deserializeOptions = new()
        {
            PropertyNameCaseInsensitive = true // De API is lowercase maar onze classes zijn UpperLowercase. Bij deseriealizeren negeren we even verschillen op deze casing.
        };

        [TestInitialize]
        public async Task SetUpAPITesting()
        {
            await CreateAPIRequestContext();
        }

        protected virtual async Task CreateAPIRequestContext()
        {
            Request = await this.Playwright.APIRequest.NewContextAsync(new()
            {
                // URL van API Endpoint
                BaseURL = "https://localhost:7172/",
                IgnoreHTTPSErrors = true
            });
        }

        [TestCleanup]
        public async Task TearDownAPITesting()
        {
            await Request.DisposeAsync();
        }

    }
}
