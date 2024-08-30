using Questao5.Application.Interfaces.Persistance;
using Questao5.Domain.Entites;
using Questao5.Domain.Resources.Request;
using Questao5.Domain.Resources.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Questao5.Application.Services
{
    public class MovimentoService : Service
    {
        public MovimentoService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public MovimentarContaCorrenteResponse Create(MovimentarContaCorrenteRequest request)
        {
            var idEmpotenciaExistente = UnitOfWork.Repository<IdEmpotencia>().Find(x => x.ChaveIdempotencia.Equals(request.IdMovimento));
            if (idEmpotenciaExistente != null)
            {
                throw new InvalidOperationException("INVALID_TRANSACTION: A operação já foi realizada anteriormente.");
            }

            Validar(request);

            Movimento movimento = new Movimento()
            {
                IdMovimento = request.IdMovimento,
                IdContaCorrente = request.IdContaCorrente,
                DataMovimento = DateTime.Now,
                TipoMovimento = request.TipoMovimento,
                Valor = request.Valor,
            };

            UnitOfWork.Repository<Movimento>().Add(movimento);

            var idempotencia = new IdEmpotencia
            {
                ChaveIdempotencia = request.IdMovimento,
                Requisicao = request.ToString(), 
                Resultado = "Operação realizada com sucesso"
            };
            UnitOfWork.Repository<IdEmpotencia>().Add(idempotencia);

            UnitOfWork.SaveChanges();

            MovimentarContaCorrenteResponse response = new MovimentarContaCorrenteResponse()
            {
                IdMovimento = movimento.IdMovimento
            };

            return response;
        }

        private void Validar(MovimentarContaCorrenteRequest request)
        {
            var contaCorrente = UnitOfWork.Repository<ContaCorrente>().Find(x => x.IdContaCorrente.Equals(request.IdContaCorrente));
            ErrorResponse erro = null;

            if (contaCorrente == null)
            {
                erro = new ErrorResponse("Apenas contas correntes cadastradas podem receber movimentação", "INVALID_ACCOUNT");
            }
            else if(!contaCorrente.Ativo)
            {
                erro = new ErrorResponse("Apenas contas correntes ativas podem receber movimentação", "INACTIVE_ACCOUNT");
            }
            else if (request.Valor <= 0)
            {
                erro = new ErrorResponse("Apenas valores positivos podem ser recebidos", "INVALID_VALUE");
            }
            else if (!(request.TipoMovimento.Equals('C') || request.TipoMovimento.Equals('D')))
            {
                erro = new ErrorResponse("Apenas os tipos “débito” ou “crédito” podem ser aceitos", "INVALID_TYPE");
            }

            if (erro != null)
            {
                throw new InvalidOperationException($"{erro.Type}: {erro.Message}");
            }
        }
    }
}
