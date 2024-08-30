using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao5.Domain.Resources.Response
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {
            
        }
        public ErrorResponse(string message, string type)
        {
            Message = message;
            Type = type;
        }
        public string Message { get; set; }
        public string Type { get; set; }
    }
}
