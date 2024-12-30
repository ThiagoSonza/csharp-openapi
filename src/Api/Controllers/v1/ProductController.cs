using Asp.Versioning;
using csharp_scalar.Features.Receitas.CadastrarReceita;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace csharp_scalar.Controllers.v1
{
    [ApiVersion("1")]
    public class ProductController(
        ILogger<ProductController> logger,
        IMediator mediator) : SharedController
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CadastrarReceitaRequest request)
        {
            var command = CadastrarReceitaCommand.Criar(
                request.Codigo,
                request.Descricao);

            if (command.IsFailure)
                return BadRequest(command.Error);

            var returns = await mediator.Send(command.Value);
            if (returns.IsFailure)
                return BadRequest(returns.Error);

            return Ok();
        }
    }
}