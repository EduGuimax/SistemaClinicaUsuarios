using System.Data.SqlClient;
using System.Configuration;

namespace SqlClinicaUser
{
    public static class ConexaoSQL
    {
        public static SqlConnection ObterConexao()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicaConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            return conn;
        }
    }
}

