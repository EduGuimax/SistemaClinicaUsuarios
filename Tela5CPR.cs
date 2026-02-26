using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlClinicaUser;

namespace SqlClinicaUser
{
    public partial class Tela5CPR : Form
    {
        private string colunaDataHora = "DataHora";
        private string colunaValor = "Valor";

        public Tela5CPR()
        {
            InitializeComponent();
        }

        private void Tela5CPR_Load(object sender, EventArgs e)
        {
            DescobrirNomesColunas();
            CarregarPacientes();
        }

        private void DescobrirNomesColunas()
        {
            try
            {
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    // Descobrir coluna de data/hora
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

                    // Descobrir coluna de valor/preço
                    string queryValor = @"SELECT COLUMN_NAME 
                                         FROM INFORMATION_SCHEMA.COLUMNS 
                                         WHERE TABLE_NAME = 'Consulta' 
                                         AND (DATA_TYPE = 'money' OR DATA_TYPE = 'decimal' OR DATA_TYPE = 'float')
                                         AND (COLUMN_NAME LIKE '%Valor%' OR COLUMN_NAME LIKE '%Preco%' OR COLUMN_NAME LIKE '%Preço%')
                                         ORDER BY COLUMN_NAME";
                    
                    using (SqlCommand cmd = new SqlCommand(queryValor, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                colunaValor = reader["COLUMN_NAME"].ToString();
                            }
                        }
                    }
                }
            }
            catch
            {
                // Se falhar, usar os nomes padrão
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

        private void btnExibir_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPaciente.SelectedIndex == -1)
                {
                    MessageBox.Show("Selecione um paciente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int pacienteId = (int)cmbPaciente.SelectedValue;
                DateTime dataSelecionada = dtp.Value.Date;

                dgv.Rows.Clear();
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    string query = $@"SELECT p.Nome AS Paciente, c.[{colunaDataHora}] AS DataPagamento, 
                                    c.[{colunaValor}] AS Valor, m.Nome AS Medico
                                    FROM Consulta c
                                    INNER JOIN Paciente p ON c.IdPaciente = p.IdPaciente
                                    INNER JOIN Medico m ON c.IdMedico = m.IdMedico
                                    WHERE c.IdPaciente = @IdPaciente 
                                    AND CAST(c.[{colunaDataHora}] AS DATE) = @DataConsulta
                                    ORDER BY c.[{colunaDataHora}] DESC";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdPaciente", pacienteId);
                        cmd.Parameters.AddWithValue("@DataConsulta", dataSelecionada);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime dataPagamento = Convert.ToDateTime(reader["DataPagamento"]);
                                decimal valorDecimal = reader["Valor"] != DBNull.Value ? Convert.ToDecimal(reader["Valor"]) : 0;
                                string valor = valorDecimal.ToString("F2").Replace(".", ",");
                                
                                dgv.Rows.Add(
                                    reader["Paciente"].ToString(),
                                    dataPagamento.ToString("dd/MM/yyyy"),
                                    valor,
                                    reader["Medico"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao buscar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Tela5CPR_FormClosed(object sender, FormClosedEventArgs e)
        {
            Menu oFrm = (Menu)this.MdiParent;
            oFrm.MnuPagamentos.Enabled = true;
            oFrm.MnuCPagamentos.Enabled = true;
        }
    }
}
