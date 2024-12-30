using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace csharp_scalar.Controllers.v2
{
    [ApiVersion("2")]
    public class ProductController(ILogger<ProductController> logger) : SharedController
    {
        /// <summary>
        /// Gera uma proposta
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Retorna OK</response>
        [HttpGet]
        public IActionResult Index()
        {
            logger.LogInformation("Chegou na controller");
            return Ok();
        }
    }
}