using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao5.Domain.Resources.Request
{
    public class MovimentarContaCorrenteRequest
    {
        public Guid IdMovimento { get; set; }
        public Guid IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public char TipoMovimento { get; set; }
    }
}
