namespace BancoShow
{
    internal class ContaPoupanca : Conta
    {
        public override decimal Saque(decimal valor)
        {
            Saldo -= (valor + 0.10m);
            return Saldo;
        }
    }
}