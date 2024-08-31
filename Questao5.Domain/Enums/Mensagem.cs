using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao5.Domain.Enums
{
    public enum Mensagem
    {
        [Description("A operação já foi realizada anteriormente.")]
        OperacaoJaRealizada = 0,
        [Description("Apenas contas correntes cadastradas podem receber movimentação ou ter saldo")]
        ContaNaoExiste = 1,
        [Description("Apenas contas correntes ativas podem receber movimentação")]
        ContaNaoAtiva = 2,
        [Description("Apenas valores positivos podem ser recebidos")]
        ValorNaoPodeSerNegativo = 3,
        [Description("Apenas os tipos “débito” ou “crédito” podem ser aceitos")]
        TipoDiferenteDeDebitoOuCredito = 4,
    }
}
