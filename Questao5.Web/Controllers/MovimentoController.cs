using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Services;
using Questao5.Domain.Resources.Request;
using Questao5.Domain.Resources.Response;
using System;

namespace Questao5.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentoController : ControllerBase
    {
        private readonly MovimentoService _movimentoService;
        public MovimentoController(MovimentoService movimentoService)
        {
            _movimentoService = movimentoService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovimentarContaCorrenteResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(MovimentarContaCorrenteRequest request)
        {
            try
            {
                var resultado = _movimentoService.Create(request);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
