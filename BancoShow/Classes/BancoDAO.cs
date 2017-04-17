using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoShow
{
    class BancoDAO
    {
        SqlConnection conexao;
        public BancoDAO()
        {
            var stringConexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BancoShow;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            conexao = new SqlConnection(stringConexao);
            conexao.Open();
        }

        public List<Conta> MostrarContas()
        {
            SqlCommand comando = new SqlCommand("select * from Conta", conexao);
            SqlDataReader reader = comando.ExecuteReader();

           List<Conta> Contas = new List<Conta>();
           while (reader.Read())
           {
                Conta conta = new Conta();
                conta.Id = Convert.ToInt32(reader["Id"]);
                conta.Numero = reader["Numero"].ToString();
                conta.Agencia = reader["Agencia"].ToString();
                conta.Tipo = reader["Tipo"].ToString();
                conta.Saldo = Convert.ToDecimal(reader["Saldo"]);
                Contas.Add(conta);
            }
            reader.Close();
            return Contas;
        }

        public bool ConsultarConta(int Id, string Numero, string Agencia)
        {
            SqlCommand comando = new SqlCommand("select * from Conta where Id <> @id and Numero = @numero and Agencia = @agencia", conexao);
            comando.Parameters.AddWithValue("@id", Id);
            comando.Parameters.AddWithValue("@numero", Numero);
            comando.Parameters.AddWithValue("@agencia", Agencia);
            SqlDataReader reader = comando.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }

        public bool EditarConta(Conta conta)
        {
            if (ConsultarConta(conta.Id, conta.Numero, conta.Agencia))
            {
                return false;
            }
            
            using (conexao)
            {
                SqlTransaction sqlTrans = conexao.BeginTransaction();
                SqlCommand comando = conexao.CreateCommand();
                comando.Transaction = sqlTrans;
                comando.CommandText = "update Conta set Agencia = @agencia, Tipo = @tipo, Saldo = @saldo where Id = @id";

                comando.Parameters.AddWithValue("@id", conta.Id);
                comando.Parameters.AddWithValue("@agencia", conta.Agencia);
                comando.Parameters.AddWithValue("@tipo", conta.Tipo);
                comando.Parameters.AddWithValue("@saldo", conta.Saldo);

                var retorno = comando.ExecuteNonQuery();
                if (retorno != 1)
                {
                    sqlTrans.Rollback();
                }
                sqlTrans.Commit();
                return true;
            }
        }

        public Conta SelecionarConta(string Numero, string Agencia)
        {
            Conta conta = new Conta();

            SqlCommand comando = new SqlCommand("select * from Conta where Numero = @numero and Agencia = @agencia", conexao);
            comando.Parameters.AddWithValue("@numero", Numero);
            comando.Parameters.AddWithValue("@agencia", Agencia);

            SqlDataReader reader = comando.ExecuteReader();
            if (reader.Read())
            {
                conta.Id = Convert.ToInt32(reader["Id"]);
                conta.Numero = reader["Numero"].ToString();
                conta.Agencia = reader["Agencia"].ToString();
                conta.Tipo = reader["Tipo"].ToString();
                conta.Saldo = Convert.ToDecimal(reader["Saldo"]);
                reader.Close();
                return conta;
            }
            else
            {
                conta.Id = 0;
                conta.Numero = "0";
                conta.Agencia = "0";
                reader.Close();
                return conta;
            }
        }

        public bool CadastrarConta(Conta conta)
        {
            if (ConsultarConta(conta.Id, conta.Numero, conta.Agencia))
            {
                return false;
            }
            SqlCommand comando = new SqlCommand("insert into Conta (Numero, Agencia, Tipo, Saldo) values (@numero, @agencia, @tipo, @saldo)", conexao);

            comando.Parameters.AddWithValue("@numero", conta.Numero);
            comando.Parameters.AddWithValue("@agencia", conta.Agencia);
            comando.Parameters.AddWithValue("@tipo", conta.Tipo);
            comando.Parameters.AddWithValue("@saldo", conta.Saldo);

            var retorno = comando.ExecuteNonQuery();
            return true;
        }

        public bool ExcluirConta(int id)
        {
            SqlCommand comando = new SqlCommand("delete from Conta where Id = @id", conexao);
            comando.Parameters.AddWithValue("@id", id);
            var retorno = comando.ExecuteNonQuery();
            return true;
        }

        public Conta CarregaConta(int id)
        {
            SqlCommand comando = new SqlCommand("select * from Conta where Id = @id", conexao);
            comando.Parameters.AddWithValue("@id", id);
            var retorno = comando.ExecuteNonQuery();
            SqlDataReader reader = comando.ExecuteReader();
            if (reader.Read())
            {
                Conta conta;
                if (reader["Tipo"].ToString() == "C")
                {
                    conta = new ContaCorrente();
                }
                else
                {
                    conta = new ContaPoupanca();
                }
                conta.Id = Convert.ToInt32(reader["Id"]);
                conta.Numero = reader["Numero"].ToString();
                conta.Agencia = reader["Agencia"].ToString();
                conta.Tipo = reader["Tipo"].ToString();
                conta.Saldo = Convert.ToDecimal(reader["Saldo"]);

                reader.Close();
                return conta;
            }
            else
            {
                reader.Close();
                throw new Exception("Erro ao acessar o Banco de Dados!");
            }
        }
    }
}
