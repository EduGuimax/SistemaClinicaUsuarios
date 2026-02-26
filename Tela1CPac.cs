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
    public partial class Tela1CPac : Form
    {
        // Índice da linha que está sendo editada; -1 = nenhum
        private int editingRowIndex = -1;

        public Tela1CPac()
        {
            InitializeComponent();
            CarregarPacientes();
        }

        private void cadastroToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            string nome = txtNome?.Text?.Trim() ?? string.Empty;
            string cpf = mtbCPF?.Text?.Trim() ?? string.Empty;
            string telefone = mtbTelefone?.Text?.Trim() ?? string.Empty;
            DateTime? dataNascimento = dtp?.Value;

            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(cpf))
            {
                MessageBox.Show("Nome e CPF são obrigatórios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    string query;
                    if (editingRowIndex >= 0)
                    {
                        // Atualizar paciente existente
                        string cpfAntigo = dgv.Rows[editingRowIndex].Cells["CPF"].Value?.ToString() ?? "";
                        query = @"UPDATE Paciente SET Nome = @Nome, CPF = @CPF, Telefone = @Telefone, 
                                 DataNascimento = @DataNascimento WHERE CPF = @CPFAntigo";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Nome", nome);
                            cmd.Parameters.AddWithValue("@CPF", cpf);
                            cmd.Parameters.AddWithValue("@Telefone", string.IsNullOrEmpty(telefone) ? DBNull.Value : (object)telefone);
                            cmd.Parameters.AddWithValue("@DataNascimento", dataNascimento.HasValue ? (object)dataNascimento.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@CPFAntigo", cpfAntigo);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Paciente atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Inserir novo paciente
                        query = @"INSERT INTO Paciente (Nome, CPF, Telefone, DataNascimento)
                                 VALUES (@Nome, @CPF, @Telefone, @DataNascimento)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Nome", nome);
                            cmd.Parameters.AddWithValue("@CPF", cpf);
                            cmd.Parameters.AddWithValue("@Telefone", string.IsNullOrEmpty(telefone) ? DBNull.Value : (object)telefone);
                            cmd.Parameters.AddWithValue("@DataNascimento", dataNascimento.HasValue ? (object)dataNascimento.Value : DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Paciente cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                CarregarPacientes();
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarPacientes()
        {
            try
            {
                dgv.Rows.Clear();
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    string query = "SELECT Nome, CPF, Telefone, DataNascimento FROM Paciente ORDER BY Nome";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string dataNasc = reader["DataNascimento"] != DBNull.Value 
                                    ? Convert.ToDateTime(reader["DataNascimento"]).ToString("dd/MM/yyyy") 
                                    : "";
                                dgv.Rows.Add(
                                    reader["Nome"].ToString(),
                                    reader["CPF"].ToString(),
                                    reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : "",
                                    dataNasc
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar pacientes: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCampos()
        {
            if (txtNome != null) txtNome.Text = string.Empty;
            if (mtbCPF != null) mtbCPF.Text = string.Empty;
            if (mtbTelefone != null) mtbTelefone.Text = string.Empty;
            if (dtp != null) dtp.Value = DateTime.Now;
            editingRowIndex = -1;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgv == null || dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione uma linha para editar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgv.SelectedRows[0];
            if (selectedRow.IsNewRow) return;

            txtNome.Text = selectedRow.Cells["Nome"].Value?.ToString() ?? "";
            mtbCPF.Text = selectedRow.Cells["CPF"].Value?.ToString() ?? "";
            mtbTelefone.Text = selectedRow.Cells["Telefone"].Value?.ToString() ?? "";
            
            string dataStr = selectedRow.Cells["DataNascimento"].Value?.ToString() ?? "";
            if (!string.IsNullOrEmpty(dataStr))
            {
                // Tentar parse com formato brasileiro primeiro
                if (DateTime.TryParseExact(dataStr, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, 
                    System.Globalization.DateTimeStyles.None, out DateTime data))
                {
                    dtp.Value = data;
                }
                else if (DateTime.TryParse(dataStr, out DateTime data2))
                {
                    dtp.Value = data2;
                }
            }

            editingRowIndex = selectedRow.Index;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgv == null || dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione uma linha para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgv.SelectedRows[0];
            if (selectedRow.IsNewRow) return;

            string cpf = selectedRow.Cells["CPF"].Value?.ToString() ?? "";

            if (MessageBox.Show("Deseja excluir este paciente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    string query = "DELETE FROM Paciente WHERE CPF = @CPF";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CPF", cpf);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Paciente excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarPacientes();
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void Tela1CPac_FormClosed(object sender, FormClosedEventArgs e)
        {
            Menu oFrm = (Menu)this.MdiParent;
            oFrm.MnuPaciente.Enabled = true;
            oFrm.MnuCPaciente.Enabled = true;
        }
    }
}

