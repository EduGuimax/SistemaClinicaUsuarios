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
    public partial class Tela3ACC : Form
    {
        private string colunaDataHora = "DataHora";
        private string colunaValor = "Valor";
        private bool tentandoCarregar = false;

        public Tela3ACC()
        {
            InitializeComponent();
            DescobrirNomesColunas();
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

                    // Descobrir coluna de valor/preço - primeiro tentar com LIKE
                    string queryValor = @"SELECT COLUMN_NAME 
                                         FROM INFORMATION_SCHEMA.COLUMNS 
                                         WHERE TABLE_NAME = 'Consulta' 
                                         AND (DATA_TYPE = 'money' OR DATA_TYPE = 'decimal' OR DATA_TYPE = 'float' OR DATA_TYPE = 'numeric')
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
                            else
                            {
                                // Se não encontrou com LIKE, buscar qualquer coluna numérica que não seja Id
                                reader.Close();
                                string queryValor2 = @"SELECT COLUMN_NAME 
                                                      FROM INFORMATION_SCHEMA.COLUMNS 
                                                      WHERE TABLE_NAME = 'Consulta' 
                                                      AND (DATA_TYPE = 'money' OR DATA_TYPE = 'decimal' OR DATA_TYPE = 'float' OR DATA_TYPE = 'numeric')
                                                      AND COLUMN_NAME NOT LIKE '%Id%'
                                                      ORDER BY COLUMN_NAME";
                                
                                using (SqlCommand cmd2 = new SqlCommand(queryValor2, conn))
                                {
                                    using (SqlDataReader reader2 = cmd2.ExecuteReader())
                                    {
                                        if (reader2.Read())
                                        {
                                            colunaValor = reader2["COLUMN_NAME"].ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Se falhar, tentar nomes comuns
                string[] nomesDataHora = { "DataHora", "DataConsulta", "Data", "Hora" };
                string[] nomesValor = { "Valor", "Preco", "Preço", "ValorConsulta" };
                
                // Tentar testar os nomes
                try
                {
                    using (SqlConnection conn = ConexaoSQL.ObterConexao())
                    {
                        foreach (string nome in nomesDataHora)
                        {
                            try
                            {
                                string testQuery = $"SELECT TOP 1 [{nome}] FROM Consulta";
                                using (SqlCommand cmd = new SqlCommand(testQuery, conn))
                                {
                                    cmd.ExecuteScalar();
                                    colunaDataHora = nome;
                                    break;
                                }
                            }
                            catch { continue; }
                        }
                        
                        foreach (string nome in nomesValor)
                        {
                            try
                            {
                                string testQuery = $"SELECT TOP 1 [{nome}] FROM Consulta";
                                using (SqlCommand cmd = new SqlCommand(testQuery, conn))
                                {
                                    cmd.ExecuteScalar();
                                    colunaValor = nome;
                                    break;
                                }
                            }
                            catch { continue; }
                        }
                    }
                }
                catch { }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CarregarPacientes();
            CarregarMedicos();
            CarregarConsultas();
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

        private void CarregarConsultas()
        {
            if (tentandoCarregar) return; // Evitar recursão infinita
            
            try
            {
                tentandoCarregar = true;
                dgv.Rows.Clear();
                
                // Adicionar coluna oculta para IdConsulta se não existir
                if (!dgv.Columns.Contains("IdConsulta"))
                {
                    DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                    colId.Name = "IdConsulta";
                    colId.HeaderText = "IdConsulta";
                    colId.Visible = false;
                    dgv.Columns.Insert(0, colId);
                }
                
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    // Tentar descobrir novamente se ainda não descobriu
                    if (colunaDataHora == "DataHora" || colunaValor == "Valor")
                    {
                        DescobrirNomesColunas();
                    }
                    
                    string query = $@"SELECT c.IdConsulta, c.[{colunaDataHora}] AS DataHora, p.Nome AS Paciente, m.Nome AS Medico, 
                                    c.[{colunaValor}] AS Valor, c.Status
                                    FROM Consulta c
                                    INNER JOIN Paciente p ON c.IdPaciente = p.IdPaciente
                                    INNER JOIN Medico m ON c.IdMedico = m.IdMedico
                                    ORDER BY c.[{colunaDataHora}] DESC";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int idConsulta = Convert.ToInt32(reader["IdConsulta"]);
                                DateTime dataHora = Convert.ToDateTime(reader["DataHora"]);
                                decimal valorDecimal = reader["Valor"] != DBNull.Value ? Convert.ToDecimal(reader["Valor"]) : 0;
                                string valor = valorDecimal.ToString("F2").Replace(".", ",");
                                string status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : "Agendado";
                                
                                dgv.Rows.Add(
                                    idConsulta,
                                    dataHora.ToString("dd/MM/yyyy HH:mm"),
                                    reader["Paciente"].ToString(),
                                    reader["Medico"].ToString(),
                                    valor,
                                    status
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Se der erro, tentar descobrir os nomes novamente
                DescobrirNomesColunas();
                
                // Tentar mais uma vez apenas se ainda não tentou
                try
                {
                    dgv.Rows.Clear();
                    
                    // Adicionar coluna oculta para IdConsulta se não existir
                    if (!dgv.Columns.Contains("IdConsulta"))
                    {
                        DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                        colId.Name = "IdConsulta";
                        colId.HeaderText = "IdConsulta";
                        colId.Visible = false;
                        dgv.Columns.Insert(0, colId);
                    }
                    
                    using (SqlConnection conn = ConexaoSQL.ObterConexao())
                    {
                        string query = $@"SELECT c.IdConsulta, c.[{colunaDataHora}] AS DataHora, p.Nome AS Paciente, m.Nome AS Medico, 
                                        c.[{colunaValor}] AS Valor, c.Status
                                        FROM Consulta c
                                        INNER JOIN Paciente p ON c.IdPaciente = p.IdPaciente
                                        INNER JOIN Medico m ON c.IdMedico = m.IdMedico
                                        ORDER BY c.[{colunaDataHora}] DESC";
                        
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int idConsulta = Convert.ToInt32(reader["IdConsulta"]);
                                    DateTime dataHora = Convert.ToDateTime(reader["DataHora"]);
                                    decimal valorDecimal = reader["Valor"] != DBNull.Value ? Convert.ToDecimal(reader["Valor"]) : 0;
                                    string valor = valorDecimal.ToString("F2").Replace(".", ",");
                                    string status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : "Agendado";
                                    
                                    dgv.Rows.Add(
                                        idConsulta,
                                        dataHora.ToString("dd/MM/yyyy HH:mm"),
                                        reader["Paciente"].ToString(),
                                        reader["Medico"].ToString(),
                                        valor,
                                        status
                                    );
                                }
                            }
                        }
                    }
                }
                catch (Exception ex2)
                {
                    MessageBox.Show("Erro ao carregar consultas: " + ex2.Message + "\n\nColuna DataHora: " + colunaDataHora + "\nColuna Valor: " + colunaValor + "\n\nVerifique se a tabela Consulta existe e possui as colunas necessárias.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                tentandoCarregar = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cmbMedico_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAgendar_Click(object sender, EventArgs e)
        {
            if (cmbPaciente.SelectedIndex == -1 || cmbMedico.SelectedIndex == -1 || string.IsNullOrWhiteSpace(mtbValor.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idPaciente = (int)cmbPaciente.SelectedValue;
                int idMedico = (int)cmbMedico.SelectedValue;
                DateTime dataHora = dtp.Value;
                
                // Converter valor do formato brasileiro (vírgula) para decimal
                string valorTexto = mtbValor.Text.Replace(",", ".");
                decimal valor = Convert.ToDecimal(valorTexto);

                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    string query = $@"INSERT INTO Consulta (IdPaciente, IdMedico, [{colunaDataHora}], [{colunaValor}], Status)
                                    VALUES (@IdPaciente, @IdMedico, @DataHora, @Valor, 'Agendado')";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdPaciente", idPaciente);
                        cmd.Parameters.AddWithValue("@IdMedico", idMedico);
                        cmd.Parameters.AddWithValue("@DataHora", dataHora);
                        cmd.Parameters.AddWithValue("@Valor", valor);
                        
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Consulta agendada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarConsultas();
                
                cmbPaciente.SelectedIndex = -1;
                cmbMedico.SelectedIndex = -1;
                mtbValor.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao agendar consulta: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione uma consulta para cancelar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgv.SelectedRows[0];
            
            // Obter IdConsulta da coluna oculta
            int idConsulta;
            if (dgv.Columns.Contains("IdConsulta") && selectedRow.Cells["IdConsulta"].Value != null)
            {
                idConsulta = Convert.ToInt32(selectedRow.Cells["IdConsulta"].Value);
            }
            else
            {
                MessageBox.Show("Erro ao identificar a consulta.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Deseja cancelar esta consulta?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    string query = "UPDATE Consulta SET Status = 'Cancelado' WHERE IdConsulta = @IdConsulta";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdConsulta", idConsulta);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Atualizar o status diretamente no DataGrid
                int statusColumnIndex = -1;
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    if (dgv.Columns[i].Name == "Status")
                    {
                        statusColumnIndex = i;
                        break;
                    }
                }
                
                if (statusColumnIndex >= 0)
                {
                    selectedRow.Cells[statusColumnIndex].Value = "Cancelado";
                }

                MessageBox.Show("Consulta cancelada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cancelar consulta: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReagendar_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione uma consulta para reagendar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgv.SelectedRows[0];
            
            // Obter IdConsulta da coluna oculta
            int idConsulta;
            if (dgv.Columns.Contains("IdConsulta") && selectedRow.Cells["IdConsulta"].Value != null)
            {
                idConsulta = Convert.ToInt32(selectedRow.Cells["IdConsulta"].Value);
            }
            else
            {
                MessageBox.Show("Erro ao identificar a consulta.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime novaDataHora = datetimepicker2.Value;

            try
            {
                using (SqlConnection conn = ConexaoSQL.ObterConexao())
                {
                    string query = $"UPDATE Consulta SET [{colunaDataHora}] = @DataHora, Status = 'Reagendado' WHERE IdConsulta = @IdConsulta";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@DataHora", novaDataHora);
                        cmd.Parameters.AddWithValue("@IdConsulta", idConsulta);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Atualizar a data/hora e status diretamente no DataGrid
                int dataHoraColumnIndex = -1;
                int statusColumnIndex = -1;
                
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    if (dgv.Columns[i].Name == "DataHora")
                    {
                        dataHoraColumnIndex = i;
                    }
                    else if (dgv.Columns[i].Name == "Status")
                    {
                        statusColumnIndex = i;
                    }
                }
                
                if (dataHoraColumnIndex >= 0)
                {
                    selectedRow.Cells[dataHoraColumnIndex].Value = novaDataHora.ToString("dd/MM/yyyy HH:mm");
                }
                
                if (statusColumnIndex >= 0)
                {
                    selectedRow.Cells[statusColumnIndex].Value = "Reagendado";
                }

                MessageBox.Show("Consulta reagendada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao reagendar consulta: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Tela3ACC_FormClosed(object sender, FormClosedEventArgs e)
        {
            Menu oFrm = (Menu)this.MdiParent;
            oFrm.MnuConsultas.Enabled = true;
            oFrm.MnuCConsultas.Enabled = true;
        }
    }
}
