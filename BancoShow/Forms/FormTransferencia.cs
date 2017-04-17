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
    public partial class FormTransferencia : Form
    {
        BancoDAO bancoDAO = new BancoDAO();
        public Conta contaOrigem { get; set; }
        
        public FormTransferencia(int Id)
        {
            InitializeComponent();
            contaOrigem = bancoDAO.CarregaConta(Id);
        }

        public void MostrarFormEditar()
        {
            FormEditar editar = new FormEditar(contaOrigem);
            editar.Show();
        }

        private void FormTransferencia_Load(object sender, EventArgs e)
        {
            textBox1.Text = contaOrigem.Numero.ToString();
            textBox2.Text = contaOrigem.Agencia;
            textBox3.Text = contaOrigem.Saldo.ToString();
        }

        public bool CamposObrigatorios()
        {
            if (textBox4.Text.Equals(string.Empty) || textBox4.Text.Equals(","))
            {
                MessageBox.Show("Valor deve ser preenchido!");
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string numero = maskedTextBox2.Text;
            string agencia = maskedTextBox3.Text;
            if (!CamposObrigatorios())
            {
                return;
            }
            else
            {
                Conta contaDestino = bancoDAO.SelecionarConta(numero, agencia);
                decimal valor = Convert.ToDecimal(textBox4.Text);

                if (contaDestino.Id == 0 && contaDestino.Numero == "0" && contaDestino.Agencia == "0")
                {
                    MessageBox.Show("Informe uma conta válida!");
                }
                else
                {
                    if (contaOrigem.Id == contaDestino.Id)
                    {
                        MessageBox.Show("Informe uma conta diferente da Conta Origem!");
                    }
                    else
                    {
                        if (valor > contaOrigem.Saldo)
                        {
                            MessageBox.Show("Valor de transferência não deve exceder saldo disponível!");
                        }
                        else
                        {
                            DialogResult dialogResult = MessageBox.Show("Deseja realmente realizar esta Transferência?", "Transferência", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                if (contaOrigem.Tipo == "C")
                                {
                                    contaOrigem.Saldo += 0.20m;
                                }
                                else
                                {
                                    contaOrigem.Saldo += 0.10m;
                                }
                                contaOrigem.Saque(valor);
                                bancoDAO.EditarConta(contaOrigem);
                                bancoDAO = new BancoDAO();
                                contaDestino.Deposito(valor);
                                bancoDAO.EditarConta(contaDestino);
                                MessageBox.Show("Transferência realizada com sucesso!");

                                Close();
                                MostrarFormEditar();
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                this.Hide();
                                FormTransferencia formTrans = new FormTransferencia(contaOrigem.Id);
                                formTrans.Show();
                            }
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            MostrarFormEditar();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
