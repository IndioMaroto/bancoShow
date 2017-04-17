using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BancoShow
{
    public partial class FormSaque : Form
    {
        BancoDAO bancoDAO = new BancoDAO();

        public Conta Conta { get; set; }

        public FormSaque(int Id)
        {
            InitializeComponent();
            Conta = bancoDAO.CarregaConta(Id);
        }

        public void MostrarFormEditar()
        {
            FormEditar editar = new FormEditar(Conta);
            editar.Show();
        }

        private void FormSaque_Load(object sender, EventArgs e)
        {
            textBox1.Text = Conta.Id.ToString();
            textBox2.Text = Conta.Numero;
            textBox3.Text = Conta.Agencia;
            if (Conta.Tipo.Equals("C"))
            {
                radioButton1.Checked = true;
                label7.Text = "- Limite de saque: Ilimitado\n- Taxa: R$ 0,20";
            }
            else
            {
                radioButton2.Checked = true;
                label7.Text = "- Limite de saque: $ 1.000,00\n- Taxa: R$ 0,10";
            }
            textBox4.Text = Conta.Saldo.ToString();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 44))
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == 44)
            {
                if ((sender as TextBox).Text.IndexOf(e.KeyChar) != -1)
                    e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!CamposObrigatorios())
            {
                return;
            }
            else
            {
                decimal valor = Convert.ToDecimal(textBox5.Text);
                if (Conta.Tipo == "P" && valor > 1000)
                {
                    MessageBox.Show("Saque limitado a R$ 1.000,00!");
                }
                else
                {
                    if (valor > Conta.Saldo)
                    {
                        MessageBox.Show("Valor de saque não deve exceder o saldo da conta!");
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("Deseja realmente realizar este saque?", "Saque", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Conta.Saque(valor);
                            bancoDAO.EditarConta(Conta);
                            MessageBox.Show("Saque realizado com sucesso!");

                            Close();
                            MostrarFormEditar();
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            this.Hide();
                            MostrarFormEditar();
                        }
                    }
                }
            }
        }

        public bool CamposObrigatorios()
        {
            if (textBox5.Text.Equals(string.Empty) || textBox5.Text.Equals(","))
            {
                MessageBox.Show("Valor deve ser informado!");
                return false;
            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            MostrarFormEditar();
        }
    }
}
