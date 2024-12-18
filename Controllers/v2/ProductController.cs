using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace csharp_scalar.Controllers.v2
{
    [ApiController]
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController(ILogger<ProductController> logger) : Controller
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