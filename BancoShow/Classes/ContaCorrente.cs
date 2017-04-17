namespace BancoShow
{
    internal class ContaCorrente : Conta
    {
        public override decimal Saque(decimal valor)
        {
            Saldo -= (valor + 0.20m);
            return Saldo;
        }
    }
}