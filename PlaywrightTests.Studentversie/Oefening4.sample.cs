using Microsoft.Playwright;
using System.Text.Json;
using WebApplication1;

namespace PlaywrightTests
{
    [TestClass]
    public class Oefening4_sample : BaseAPITest
    {


        /// <summary>
        /// Bij deze test wordt de "ruwe" json getest. Je gaat dus zelf ittereren en attributen uit de JSON opzoeken en "asserten".
        /// </summary>
        /// <returns></returns>
        // [TestMethod] // Testmethod uitgeschakeld omdat deze test faalt. Dit is een voorbeeld en het endpoint bestaat niet.
        public async Task Oefening4_GetCustomer_BareJSON()
        {
            // Request uitvoeren
            var apiResponse = await Request.GetAsync("api/customers/2"); // uitvoeren van een request

            // Response verwerken
            await Expect(apiResponse).ToBeOKAsync(); // Is de repsonse OK?
            var results = await apiResponse.JsonAsync(); // JSON uit de response halen
            Assert.IsNotNull(results);
            var customer = results.Value;

            // Inhoud controleren
            Assert.AreEqual(2, customer.GetProperty("id").GetInt32()); // ID controleren. Moet overeenkomen met ID van het request.
            Assert.AreEqual("Jan Janssen", customer.GetProperty("name").GetString());
            Assert.AreEqual("janjanssen@randomdomein.com", customer.GetProperty("email").GetString());
            Assert.AreEqual(-0.5m, customer.GetProperty("afwijkingLinkerOog").GetDecimal()); // blijkbaar een oogafwijking van -0.5 dioptrie aan het linker oog
        }


    }
}
