using Microsoft.EntityFrameworkCore;
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
using System.Threading.Tasks;

namespace Questao5.Application.Services
{
    public class ContaCorrenteService : Service
    {
        public ContaCorrenteService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public SaldoContaCorrenteResponse GetSaldo(SaldoContaCorrenteRequest request)
        {
            ContaCorrente contaCorrente = UnitOfWork.Repository<ContaCorrente>().DbSet()
                .Include(x => x.Movimentos)
                .FirstOrDefault(x => x.IdContaCorrente.Equals(request.IdContaCorrente));

            Validar(request, contaCorrente);

            Movimento[] movimentosCredito = contaCorrente.Movimentos.Where(x => x.TipoMovimento.Equals('C')).ToArray();
            Movimento[] movimentosDebito = contaCorrente.Movimentos.Where(x => x.TipoMovimento.Equals('D')).ToArray();

            decimal creditos = movimentosCredito.Select(x => x.Valor).Sum();
            decimal debitos = movimentosDebito.Select(x => x.Valor).Sum();

            decimal saldo = creditos - debitos;

            SaldoContaCorrenteResponse response = new SaldoContaCorrenteResponse()
            {
                Nome = contaCorrente.Nome,
                Numero = contaCorrente.Numero,
                DataConsulta = DateTime.Now,
                SaldoAtual = saldo
            };

            return response;
        }

        private void Validar(SaldoContaCorrenteRequest request, ContaCorrente contaCorrente)
        {
            ErrorResponse erro = null;

            if (contaCorrente == null)
            {
                erro = new ErrorResponse(Mensagem.ContaNaoExiste.GetDescription(), TipoErro.INVALID_ACCOUNT.GetDescription());
            }
            else if (!contaCorrente.Ativo)
            {
                erro = new ErrorResponse(Mensagem.ContaNaoAtiva.GetDescription(), TipoErro.INACTIVE_ACCOUNT.GetDescription());
            }

            if (erro != null)
            {
                throw new Exception($"{erro.Type}: {erro.Message}");
            }
        }
    }
}