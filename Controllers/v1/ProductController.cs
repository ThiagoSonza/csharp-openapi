using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace csharp_scalar.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
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

        /// <summary>
        /// Gera uma proposta
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Retorna OK</response>
        [HttpGet("test2")]
        public IActionResult Index2()
        {
            logger.LogInformation("Chegou na controller 2");
            return Ok();
        }

        /// <summary>
        /// Gera uma proposta
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Retorna OK</response>
        [HttpGet("test3")]
        public IActionResult Index3()
        {
            logger.LogInformation("Chegou na controller 3");
            return Ok();
        }
    }
}