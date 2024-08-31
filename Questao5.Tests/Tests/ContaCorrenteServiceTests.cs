using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using NSubstitute;
using Questao5.Application.Interfaces.Persistance;
using Questao5.Application.Interfaces.Persistance.Repositories;
using Questao5.Application.Services;
using Questao5.Domain.Entites;
using Questao5.Domain.Enums;
using Questao5.Domain.Extensions;
using Questao5.Domain.Resources.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Questao5.Tests.Tests
{
    public class ContaCorrenteServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IRepository<ContaCorrente>> _contaCorrenteRepository;
        private readonly ContaCorrenteService _contaCorrenteService;

        public ContaCorrenteServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _contaCorrenteRepository = new Mock<IRepository<ContaCorrente>>();
            _unitOfWork.Setup(u => u.Repository<ContaCorrente>()).Returns(_contaCorrenteRepository.Object);

            _contaCorrenteService = new ContaCorrenteService(_unitOfWork.Object);
        }

        [Fact]
        public void GetSaldo_ContaExistente_DeveRetornarSaldo()
        {
            // Arrange
            var request = new SaldoContaCorrenteRequest
            {
                IdContaCorrente = Guid.NewGuid()
            };

            var movimentos = new List<Movimento>
        {
            new Movimento { TipoMovimento = 'C', Valor = 100 },
            new Movimento { TipoMovimento = 'D', Valor = 50 }
        };

            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = request.IdContaCorrente,
                Nome = "Conta Teste",
                Numero = 123,
                Ativo = true,
                Movimentos = movimentos
            };

            _contaCorrenteRepository.Setup(x => x.DbSet())
                .ReturnsDbSet(new List<ContaCorrente> { contaCorrente });

            // Act
            var resultado = _contaCorrenteService.GetSaldo(request);

            // Assert
            Assert.Equal("Conta Teste", resultado.Nome);
            Assert.Equal(123, resultado.Numero);
            Assert.Equal(50, resultado.SaldoAtual);
        }

        [Fact]
        public void GetSaldo_ContaInexistente_DeveLancarInvalidOperationException()
        {
            // Arrange
            var contaId = Guid.NewGuid();

            _contaCorrenteRepository.Setup(x => x.DbSet())
                .ReturnsDbSet(new List<ContaCorrente>()); 

            var request = new SaldoContaCorrenteRequest { IdContaCorrente = contaId };

            // Act
            Action act = () => _contaCorrenteService.GetSaldo(request);

            // Assert
            act.Should().Throw<Exception>()
               .WithMessage(string.Concat(TipoErro.INVALID_ACCOUNT.GetDescription(), ": ", Mensagem.ContaNaoExiste.GetDescription()));
        }

    }
}
