using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlClinicaUser
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private void cadastroPacientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tela1CPac oFrm = new Tela1CPac();
            oFrm.MdiParent = this;
            this.MnuPaciente.Enabled = false;
            this.MnuCPaciente.Enabled = false;
            oFrm.Show();
        }

        private void MnuMedico_Click(object sender, EventArgs e)
        {
            Tela2CMed oFrm = new Tela2CMed();
            oFrm.MdiParent = this;
            this.MnuMedico.Enabled = false;
            this.MnuCMedico.Enabled = false;
            oFrm.Show();
        }

        private void MnuConsultas_Click(object sender, EventArgs e)
        {
            Tela3ACC oFrm = new Tela3ACC();
            oFrm.MdiParent = this;
            this.MnuConsultas.Enabled = false;
            this.MnuCConsultas.Enabled = false;
            oFrm.Show();
        }

        private void MnuProntuario_Click(object sender, EventArgs e)
        {
            Tela4CP oFrm = new Tela4CP();
            oFrm.MdiParent = this;
            this.MnuProntuario.Enabled = false;
            this.MnuCProntuario.Enabled = false;
            oFrm.Show();
        }

        private void MnuPagamentos_Click(object sender, EventArgs e)
        {
            Tela5CPR oFrm = new Tela5CPR();
            oFrm.MdiParent = this;
            this.MnuPagamentos.Enabled = false;
            this.MnuCPagamentos.Enabled = false;
            oFrm.Show();
        }

        private void MnuSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MnuCPaciente_Click(object sender, EventArgs e)
        {
            cadastroPacientesToolStripMenuItem_Click(sender, e);
        }

        private void MnuCMedico_Click(object sender, EventArgs e)
        {
            MnuMedico_Click(sender, e);
        }

        private void MnuCConsultas_Click(object sender, EventArgs e)
        {
            MnuConsultas_Click(sender, e);
        }

        private void MnuCProntuario_Click(object sender, EventArgs e)
        {
            MnuProntuario_Click(sender, e);
        }

        private void MnuCPagamentos_Click(object sender, EventArgs e)
        {
            MnuPagamentos_Click(sender, e);
        }

        private void MnuCSair_Click(object sender, EventArgs e)
        {
            MnuSair_Click(sender, e);
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicaConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                MessageBox.Show("Conexão funcionando!");
            }
        }
    }
}
