using AspNetCore.Authentication.ApiKey;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WebApplication1
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Statische collectie van producten voor de API
        private static readonly List<Product> Products = new()
        {
            new Product
            {
                Id = 1,
                Name = "Luxe Koptelefoon",
                Description = "Ruisonderdrukking, comfortabel ontwerp, 30 uur batterij",
                Price = 79.00m,
                InStock = true
            },
            new Product
            {
                Id = 2,
                Name = "Draadloze Muis",
                Description = "Ergonomisch model, bluetooth en USB",
                Price = 29.00m,
                InStock = true
            },
            new Product
            {
                Id = 3,
                Name = "Laptop 14 inch",
                Description = "8GB RAM, 256GB SSD, Full HD scherm",
                Price = 899.00m,
                InStock = false
            }
        };


        // GET: api/<ProductsController>
        /// <summary>
        /// Via dit endpoint kun je een collectie van producten ophalen. De producten worden gesorteerd volgens het <paramref name="sorting"/> argument. Als 
        /// er geen sortering wordt meegegeven wordt de standaard volgorde aangehouden.
        /// </summary>
        /// <param name="sorting">NULL of een geldige sortering ("name" of "price")</param>
        /// <returns>Collectie met de producten</returns>
        /// <exception cref="ArgumentOutOfRangeException">Exceptie als er een ongeldige sortering is meegegeven.</exception>
        /// <response code="200">De producten</response>
        /// <response code="400">Ongeldig argument</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Product>> Get(string? sorting)
        {
            if (sorting == null)
                return Ok(Products);

            return sorting.ToLower() switch
            {
                "name" => Ok(Products.OrderBy(p => p.Name)),
                "price" => Ok(Products.OrderBy(p => p.Price)),
                _ => BadRequest("The supplied sorting is not a valid argument.")
            };
        }

        // GET api/<ProductsController>/5
        /// <summary>
        /// Haalt een specifiek product op door middel van het meegegeven <paramref name="id"/>.
        /// </summary>
        /// <param name="id">geldig productid</param>
        /// <returns>Bij een bestaand product status 200 + <see cref="Product"/> entity. Niet bestaand product geeft status 404.</returns>
        /// <response code="200">Het product</response>
        /// <response code="404">Product niet gevonden</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> Get(int id)
        {
            var product = Products.SingleOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST api/<ProductsController>
        /// <summary>
        /// Voegt het meegestuurde <paramref name="product"/> toe aan de bestaande collectie met producten. 
        /// Let op! Deze methode heeft het <see cref="AuthorizeAttribute"/> attribuut en dus moet je hiervoor geauthoriseerd zijn. Dit gebeurt in dit geval met een API key in de header. In 
        /// Swagger zou je "your-secure-api-key-here-12345" als API key kunnen gebruiken.
        /// </summary>
        /// <param name="product">product entiteit</param>
        /// <returns>Satuscode 201 of 400</returns>
        /// <response code="200">Het product</response>
        /// <response code="404">Product niet gevonden</response>
        [Authorize(AuthenticationSchemes = ApiKeyDefaults.AuthenticationScheme)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Post([FromBody] Product product)
        {
            if (product == null) return BadRequest();

            Products.Add(product);
            return Created();
        }


        // DELETE api/<ProductsController>/5
        /// <summary>
        /// Verwijderd het product op basis van het <paramref name="id"/> uit de collectie producten. 
        /// Let op! Deze methode heeft het <see cref="AuthorizeAttribute"/> attribuut en dus moet je hiervoor geauthoriseerd zijn. Dit gebeurt in dit geval met een API key in de header. In 
        /// Swagger zou je "your-secure-api-key-here-12345" als API key kunnen gebruiken.
        /// </summary>
        /// <param name="id">Id van het product</param>
        /// <returns>Status 404 of 204</returns>
        /// <response code="404">Te verwijderen product bestaat niet</response>
        /// <response code="204">Product verwijderd</response>
        [Authorize(AuthenticationSchemes = ApiKeyDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult Delete(int id)
        {
            var productToRemove = Products.SingleOrDefault(p => p.Id == id);

            if (productToRemove == null)
                return NotFound();

            Products.Remove(productToRemove);
            return NoContent(); // 204 status = OK; maar geen return van content
        }
    }

    /// <summary>
    /// Simpel Product model (normaal in aparte map / file)
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Uniek ID van het product
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Korte Productnaam
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Lange beschrijving 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Prijs in euro
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Status van de voorraad
        /// </summary>
        public bool InStock { get; set; }
    }
}
