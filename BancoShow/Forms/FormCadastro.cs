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
    public partial class FormCadastro : Form
    {
        BancoDAO bancoDAO = new BancoDAO();
        public Conta Conta { get; set; }
        public FormCadastro()
        {
            InitializeComponent();
            Conta = new Conta();
        }

        private void maskedTextBox1_Click(object sender, EventArgs e)
        {
            this.maskedTextBox1.Select(0, 0);
        }

        private void maskedTextBox2_Click(object sender, EventArgs e)
        {
            this.maskedTextBox2.Select(0, 0);
        }

        public bool CamposObrigatorios()
        {
            if (!maskedTextBox1.MaskFull)
            {
                MessageBox.Show("Número da conta deve ser preenchido!");
                return false;
            }
            if (!maskedTextBox2.MaskFull)
            {
                MessageBox.Show("Número da agência deve ser preenchido!");
                return false;
            }
            if(textBox3.Text.Equals(string.Empty) || textBox3.Text.Equals(","))
            {
                MessageBox.Show("Saldo deve ser preenchido!");
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!CamposObrigatorios())
            {
                return;
            }
            else
            {
                Conta.Numero = maskedTextBox1.Text;
                Conta.Agencia = maskedTextBox2.Text;
                Conta.Tipo = radioButton1.Checked == true ? "C" : "P";
                Conta.Saldo = Convert.ToDecimal(textBox3.Text);

                if (bancoDAO.CadastrarConta(Conta))
                {
                    MessageBox.Show("Conta cadastrada com sucesso!");
                    Close();
                    Form1 home = new Form1();
                    home.Show();

                }
                else
                {
                    MessageBox.Show("Já existe uma conta cadastrada com esse número nessa Agência!");
                }
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            Form1 home = new Form1();
            home.Show();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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
