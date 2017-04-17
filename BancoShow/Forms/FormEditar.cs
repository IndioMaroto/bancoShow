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
    public partial class FormEditar : Form
    {
        BancoDAO bancoDAO = new BancoDAO();
        public Conta Conta { get; set; }

        public FormEditar(Conta conta)
        {
            Conta = conta;
            InitializeComponent();
        }

        private void FormEditar_Load(object sender, EventArgs e)
        {
            textBox1.Text = Conta.Id.ToString();
            textBox2.Text = Conta.Numero;
            maskedTextBox1.Text = Conta.Agencia;
            if (Conta.Tipo.Equals("C") ? radioButton1.Checked = true : radioButton2.Checked = true)
            textBox4.Text = Conta.Saldo.ToString();

            if(Conta.Saldo <= 0)
            {
                button4.Enabled = false;
                button2.Enabled = false;
            }
        }

        public void MostrarForm1()
        {
            Form1 home = new Form1();
            home.Show();
        }

        public void MostrarFormEditar()
        {
            FormEditar editar = new FormEditar(Conta);
            editar.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Conta.Agencia = maskedTextBox1.Text;
            Conta.Tipo = radioButton1.Checked == true ? "C" : "P";

            if (bancoDAO.EditarConta(Conta))
            {
                MessageBox.Show("Dados atualizados com sucesso!");
                Close();
                MostrarForm1();
            }
            else
            {
                MessageBox.Show("Não foi possível alterar os dados da conta!\nJá existe uma conta cadastrada com esse número nessa Agência!");
                Refresh();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Close();
            MostrarForm1();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBox1.Text);
            DialogResult dialogResult = MessageBox.Show("Deseja realmente excluir essa conta?", "Excluir Conta", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                bancoDAO.ExcluirConta(id);
                MessageBox.Show("Conta excluída com sucesso!");
                this.Hide();
                MostrarForm1();
            }
            else if (dialogResult == DialogResult.No)
            {
                Close();
                MostrarFormEditar();
            }
        }

        private void maskedTextBox1_Click(object sender, EventArgs e)
        {
            this.maskedTextBox1.Select(0, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            FormTransferencia formTrans = new FormTransferencia(Conta.Id);
            formTrans.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
            FormSaque formSaque = new FormSaque(Conta.Id);
            formSaque.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
            FormDeposito formDepos = new FormDeposito(Conta.Id);
            formDepos.Show();
        }
    }
}
