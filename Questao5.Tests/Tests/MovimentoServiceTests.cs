using FluentAssertions;
using NSubstitute;
using Questao5.Application.Interfaces.Persistance.Repositories;
using Questao5.Application.Interfaces.Persistance;
using Questao5.Application.Services;
using Questao5.Domain.Entites;
using Questao5.Domain.Resources.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Questao5.Domain.Enums;
using Questao5.Domain.Extensions;
using System.Linq.Expressions;

namespace Questao5.Tests.Tests
{
    public class MovimentoServiceTests
    {
        private readonly MovimentoService _movimentoService;
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly IRepository<Movimento> _movimentoRepository = Substitute.For<IRepository<Movimento>>();
        private readonly IRepository<IdEmpotencia> _idEmpotenciaRepository = Substitute.For<IRepository<IdEmpotencia>>();
        private readonly IRepository<ContaCorrente> _contaCorrenteRepository = Substitute.For<IRepository<ContaCorrente>>();

        public MovimentoServiceTests()
        {
            _unitOfWork.Repository<Movimento>().Returns(_movimentoRepository);
            _unitOfWork.Repository<IdEmpotencia>().Returns(_idEmpotenciaRepository);
            _unitOfWork.Repository<ContaCorrente>().Returns(_contaCorrenteRepository);
            _movimentoService = new MovimentoService(_unitOfWork);
        }

        [Fact]
        public void Create_IdempotenciaExistente_DeveLancarInvalidOperationException()
        {
            // Arrange
            var request = new MovimentarContaCorrenteRequest { IdMovimento = Guid.NewGuid() };
            _idEmpotenciaRepository
                .Find(Arg.Any<Expression<Func<IdEmpotencia, bool>>>())
                .Returns(new IdEmpotencia());

            // Act
            Action act = () => _movimentoService.Create(request);

            // Assert
            act.Should().Throw<Exception>().WithMessage(string.Concat(TipoErro.INVALID_TRANSACTION.GetDescription(), ": ", Mensagem.OperacaoJaRealizada));
        }

        [Fact]
        public void Create_ContaInexistente_DeveLancarInvalidOperationException()
        {
            // Arrange
            var request = new MovimentarContaCorrenteRequest { IdMovimento = Guid.NewGuid(), IdContaCorrente = Guid.NewGuid() };
            _contaCorrenteRepository
                .Find(Arg.Any<Expression<Func<ContaCorrente, bool>>>())
                .Returns((ContaCorrente)null);

            // Act
            Action act = () => _movimentoService.Create(request);

            // Assert
            act.Should().Throw<Exception>().WithMessage(string.Concat(TipoErro.INVALID_ACCOUNT.GetDescription(), ": ", Mensagem.ContaNaoExiste.GetDescription()));
        }


        [Fact]
        public void Create_ValorNegativo_DeveLancarInvalidOperationException()
        {
            // Arrange
            var contaCorrente = new ContaCorrente { IdContaCorrente = Guid.NewGuid(), Ativo = true };
            var request = new MovimentarContaCorrenteRequest { IdMovimento = Guid.NewGuid(), IdContaCorrente = contaCorrente.IdContaCorrente, Valor = -10 };

            _contaCorrenteRepository
                .Find(Arg.Any<Expression<Func<ContaCorrente, bool>>>())
                .Returns(contaCorrente);

            // Act
            Action act = () => _movimentoService.Create(request);

            // Assert
            act.Should().Throw<Exception>()
                .WithMessage(string.Concat(TipoErro.INVALID_VALUE.GetDescription(), ": ", Mensagem.ValorNaoPodeSerNegativo.GetDescription()));
        }


        [Fact]
        public void Create_MovimentoValido_DeveAdicionarMovimento()
        {
            // Arrange
            var contaCorrente = new ContaCorrente { IdContaCorrente = Guid.NewGuid(), Ativo = true };
            var request = new MovimentarContaCorrenteRequest { IdMovimento = Guid.NewGuid(), IdContaCorrente = contaCorrente.IdContaCorrente, Valor = 100, TipoMovimento = 'C' };
            _contaCorrenteRepository
                .Find(Arg.Any<Expression<Func<ContaCorrente, bool>>>())
                .Returns(contaCorrente);

            // Act
            var result = _movimentoService.Create(request);

            // Assert
            _movimentoRepository.Received(1).Add(Arg.Any<Movimento>());
            _idEmpotenciaRepository.Received(1).Add(Arg.Any<IdEmpotencia>());
            _unitOfWork.Received(1).SaveChanges();
            result.IdMovimento.Should().Be(request.IdMovimento);
        }
    }
}
