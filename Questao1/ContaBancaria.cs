using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria {

        private const double TaxaSaque = 3.50;
        private double _saldo;

        public ContaBancaria(int numero,string titular,double depositoInicial)
        {
            Numero = numero;
            Titular = titular;
            _saldo = depositoInicial;
        }

        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
            _saldo = 0;
        }

        public int Numero { get; private set; }
        public string Titular { get; set; }
        public double Saldo => _saldo;

        public void Deposito(double quantia)
        {
            if (quantia > 0)
            {
                _saldo += quantia;
            }
        }

        public void Saque(double quantia)
        {
            double quantiaComTaxa = quantia + TaxaSaque;
            _saldo -= quantiaComTaxa;
        }
    }
}
