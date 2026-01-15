using Microsoft.Playwright;

namespace PlaywrightTests
{
    // Basis klasse voor het definieren van basisinstellingen
    public class BasePageTest : PageTest
    {
        public override BrowserNewContextOptions ContextOptions()
        {
            return new BrowserNewContextOptions()
            {
                ViewportSize = new()
                {
                    Width = 1920,
                    Height = 1080
                },
                // Zo hoef je in je tests alleen maar de relatieve URLs te gebruiken
                BaseURL = "https://localhost:7172/",
            };
        }

    }
}
