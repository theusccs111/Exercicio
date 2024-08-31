using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Services;
using Questao5.Domain.Resources.Request;
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
        public IActionResult Post(SaldoContaCorrenteRequest request)
        {
            try
            {
                var resultado = _contaCorrenteService.Create(request);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
