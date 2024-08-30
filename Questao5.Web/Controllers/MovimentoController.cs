using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Services;
using Questao5.Domain.Resources.Request;

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
        public IActionResult Post(MovimentarContaCorrenteRequest request)
        {
            var resultado = _movimentoService.Create(request);
            return Ok(resultado);
        }
    }
}
