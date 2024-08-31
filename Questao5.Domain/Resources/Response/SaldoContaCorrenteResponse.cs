using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao5.Domain.Resources.Response
{
    public class SaldoContaCorrenteResponse
    {
        
        public int Numero { get; set; }
        public string Nome { get; set; }
        public DateTime DataConsulta { get; set; }
        public decimal SaldoAtual { get; set; }
    }
}
