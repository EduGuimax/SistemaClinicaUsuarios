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
    public partial class Tela4CP : Form
    {
        private string colunaDataHora = "DataHora";

        public Tela4CP() => InitializeComponent();

        private void Tela4CP_Load(object sender, EventArgs e)
        {
            DescobrirNomesColunas();
            CarregarPacientes();
            CarregarMedicos();
        }

        private void DescobrirNomesColunas()
        {
            try
            {
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    string queryData = @"SELECT COLUMN_NAME 
                                        FROM INFORMATION_SCHEMA.COLUMNS 
                                        WHERE TABLE_NAME = 'Consulta' 
                                        AND (DATA_TYPE = 'datetime' OR DATA_TYPE = 'datetime2' OR DATA_TYPE = 'smalldatetime')
                                        ORDER BY COLUMN_NAME";
                    
                    using (SqlCommand cmd = new SqlCommand(queryData, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                colunaDataHora = reader["COLUMN_NAME"].ToString();
                            }
                        }
                    }
                }
            }
            catch
            {
                // Se falhar, usar o nome padrão
            }
        }

        private void CarregarPacientes()
        {
            try
            {
                cmbPaciente.Items.Clear();
                cmbPaciente.DisplayMember = "Nome";
                cmbPaciente.ValueMember = "IdPaciente";
                
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    string query = "SELECT IdPaciente, Nome FROM Paciente ORDER BY Nome";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.Add("IdPaciente", typeof(int));
                            dt.Columns.Add("Nome", typeof(string));
                            
                            while (reader.Read())
                            {
                                dt.Rows.Add(reader["IdPaciente"], reader["Nome"]);
                            }
                            
                            cmbPaciente.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar pacientes: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarMedicos()
        {
            try
            {
                cmbMedico.Items.Clear();
                cmbMedico.DisplayMember = "Nome";
                cmbMedico.ValueMember = "IdMedico";
                
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    string query = "SELECT IdMedico, Nome FROM Medico ORDER BY Nome";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.Add("IdMedico", typeof(int));
                            dt.Columns.Add("Nome", typeof(string));
                            
                            while (reader.Read())
                            {
                                dt.Rows.Add(reader["IdMedico"], reader["Nome"]);
                            }
                            
                            cmbMedico.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar médicos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExibir_Click(object sender, EventArgs e)
        {
            // Validar se os ComboBox possuem seleção
            if (cmbPaciente.SelectedIndex == -1 || cmbMedico.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um paciente e um médico.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Obter os valores selecionados
                int idPaciente = (int)cmbPaciente.SelectedValue;
                int idMedico = (int)cmbMedico.SelectedValue;

                dgv.Rows.Clear();
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    string query = $@"SELECT p.Nome AS Paciente, m.Nome AS Medico, c.[{colunaDataHora}] AS DataHora
                                    FROM Consulta c
                                    INNER JOIN Paciente p ON c.IdPaciente = p.IdPaciente
                                    INNER JOIN Medico m ON c.IdMedico = m.IdMedico
                                    WHERE c.IdPaciente = @IdPaciente AND c.IdMedico = @IdMedico
                                    ORDER BY c.[{colunaDataHora}] DESC";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdPaciente", idPaciente);
                        cmd.Parameters.AddWithValue("@IdMedico", idMedico);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime dataHora = Convert.ToDateTime(reader["DataHora"]);
                                dgv.Rows.Add(
                                    reader["Paciente"].ToString(),
                                    reader["Medico"].ToString(),
                                    dataHora.ToString("dd/MM/yyyy HH:mm")
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao exibir informações: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Tela4CP_FormClosed(object sender, FormClosedEventArgs e)
        {
            Menu oFrm = (Menu)this.MdiParent;
            oFrm.MnuProntuario.Enabled = true;
            oFrm.MnuCProntuario.Enabled = true;
        }
    }
}
