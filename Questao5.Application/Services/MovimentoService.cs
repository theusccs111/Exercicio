using Questao5.Application.Interfaces.Persistance;
using Questao5.Domain.Entites;
using Questao5.Domain.Enums;
using Questao5.Domain.Extensions;
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
            IdEmpotencia idEmpotenciaExistente = UnitOfWork.Repository<IdEmpotencia>().Find(x => x.ChaveIdempotencia.Equals(request.IdMovimento));
            if (idEmpotenciaExistente != null)
            {
                throw new InvalidOperationException(string.Concat(TipoErro.INVALID_TRANSACTION.GetDescription(),": ",Mensagem.OperacaoJaRealizada));
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

            IdEmpotencia idempotencia = new IdEmpotencia
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
            ContaCorrente contaCorrente = UnitOfWork.Repository<ContaCorrente>().Find(x => x.IdContaCorrente.Equals(request.IdContaCorrente));
            ErrorResponse erro = null;

            if (contaCorrente == null)
            {
                erro = new ErrorResponse(Mensagem.ContaNaoExiste.GetDescription(), TipoErro.INVALID_ACCOUNT.GetDescription());
            }
            else if(!contaCorrente.Ativo)
            {
                erro = new ErrorResponse(Mensagem.ContaNaoAtiva.GetDescription(), TipoErro.INACTIVE_ACCOUNT.GetDescription());
            }
            else if (request.Valor <= 0)
            {
                erro = new ErrorResponse(Mensagem.ValorNaoPodeSerNegativo.GetDescription(), TipoErro.INVALID_VALUE.GetDescription());
            }
            else if (!(request.TipoMovimento.Equals('C') || request.TipoMovimento.Equals('D')))
            {
                erro = new ErrorResponse(Mensagem.TipoDiferenteDeDebitoOuCredito.GetDescription(), TipoErro.INVALID_TYPE.GetDescription());
            }

            if (erro != null)
            {
                throw new Exception($"{erro.Type}: {erro.Message}");
            }
        }
    }
}
