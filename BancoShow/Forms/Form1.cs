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
    public partial class Form1 : Form
    {
        BancoDAO bancoDAO = new BancoDAO();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = bancoDAO.MostrarContas();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormCadastro frmCadastro = new FormCadastro();
            frmCadastro.Show();
            this.Hide();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var linha = dataGridView1.CurrentRow.DataBoundItem as Conta;
            FormEditar formEdit = new FormEditar(linha);
            formEdit.Show();
            this.Hide();
        }
    }
}
