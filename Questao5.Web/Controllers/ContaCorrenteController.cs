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
    public class ContaCorrenteController : ControllerBase
    {
        private readonly ContaCorrenteService _contaCorrenteService;
        public ContaCorrenteController(ContaCorrenteService contaCorrenteService)
        {
            _contaCorrenteService = contaCorrenteService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaldoContaCorrenteResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(SaldoContaCorrenteRequest request)
        {
            try
            {
                var resultado = _contaCorrenteService.GetSaldo(request);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
