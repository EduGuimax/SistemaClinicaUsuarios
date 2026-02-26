using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlClinicaUser;

namespace SqlClinicaUser
{
    public partial class Tela2CMed : Form
    {
        private int linhaEmEdicao = -1;

        public Tela2CMed()
        {
            InitializeComponent();
        }

        private void Tela2CMed_Load(object sender, EventArgs e)
        {
            ConfigurarDataGridView();
            CarregarMedicos();
        }

        private void ConfigurarDataGridView()
        {
            dgv.Columns.Clear();
            dgv.Columns.Add("Nome", "Nome do Médico");
            dgv.Columns.Add("CRM", "CRM");
            dgv.Columns.Add("Especialidade", "Especialidade");
        }

        private void CarregarMedicos()
        {
            try
            {
                dgv.Rows.Clear();
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    string query = "SELECT Nome, CRM, Especialidade FROM Medico ORDER BY Nome";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dgv.Rows.Add(
                                    reader["Nome"].ToString(),
                                    reader["CRM"].ToString(),
                                    reader["Especialidade"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar médicos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) || 
                string.IsNullOrWhiteSpace(mtbCRM.Text) || 
                string.IsNullOrWhiteSpace(txtEspecialidade.Text))
            {
                MessageBox.Show("Preencha todos os campos!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    string query;
                    if (linhaEmEdicao >= 0)
                    {
                        // Atualizar médico existente
                        string crmAntigo = dgv.Rows[linhaEmEdicao].Cells["CRM"].Value?.ToString() ?? "";
                        query = @"UPDATE Medico SET Nome = @Nome, CRM = @CRM, Especialidade = @Especialidade 
                                 WHERE CRM = @CRMAntigo";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Nome", txtNome.Text.Trim());
                            cmd.Parameters.AddWithValue("@CRM", mtbCRM.Text.Trim());
                            cmd.Parameters.AddWithValue("@Especialidade", txtEspecialidade.Text.Trim());
                            cmd.Parameters.AddWithValue("@CRMAntigo", crmAntigo);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Médico atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Inserir novo médico
                        query = @"INSERT INTO Medico (Nome, CRM, Especialidade) 
                                 VALUES (@Nome, @CRM, @Especialidade)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Nome", txtNome.Text.Trim());
                            cmd.Parameters.AddWithValue("@CRM", mtbCRM.Text.Trim());
                            cmd.Parameters.AddWithValue("@Especialidade", txtEspecialidade.Text.Trim());
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Médico cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                CarregarMedicos();
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione uma linha para editar!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            linhaEmEdicao = dgv.SelectedRows[0].Index;
            txtNome.Text = dgv.Rows[linhaEmEdicao].Cells["Nome"].Value?.ToString() ?? "";
            mtbCRM.Text = dgv.Rows[linhaEmEdicao].Cells["CRM"].Value?.ToString() ?? "";
            txtEspecialidade.Text = dgv.Rows[linhaEmEdicao].Cells["Especialidade"].Value?.ToString() ?? "";
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            mtbCRM.Clear();
            txtEspecialidade.Clear();
            linhaEmEdicao = -1;
            dgv.ClearSelection();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione uma linha para excluir!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgv.SelectedRows[0];
            string crm = selectedRow.Cells["CRM"].Value?.ToString() ?? "";

            DialogResult resultado = MessageBox.Show("Tem certeza que deseja excluir este médico?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (resultado == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = ConexaoSQL.ObterConexao())
                    {
                        string query = "DELETE FROM Medico WHERE CRM = @CRM";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@CRM", crm);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Médico excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarMedicos();
                    LimparCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao excluir: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Tela2CMed_FormClosed(object sender, FormClosedEventArgs e)
        {
            Menu oFrm = (Menu)this.MdiParent;
            oFrm.MnuMedico.Enabled = true;
            oFrm.MnuCMedico.Enabled = true;
        }
    }
}
