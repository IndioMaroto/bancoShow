using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoShow
{
    public class Conta
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Agencia { get; set; }
        public string Tipo { get; set; }
        public decimal Saldo { get; set; }

        public virtual decimal Saque(decimal valor)
        {
            Saldo -= valor;
            return Saldo;
        }
        public decimal Deposito(decimal valor)
        {
            Saldo += valor;
            return Saldo;
        }
    }
}
