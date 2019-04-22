using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using NFModel;

namespace NFDAL
{
    public class UsuarioDAL
    {


        public static bool ValidarUsuario(string login, string senha)
        {
            bool autenticado = false;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;

            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;

                StringBuilder queryGet = new StringBuilder(@"
				    SELECT 
                    Id,
                    Nome,
                    Login,
                    Senha,
                    DataCriacao,
                    Tipo
	                FROM Usuario
                    WHERE Login = @Login AND Senha = @Senha
                ");

                comm.Parameters.Add(new SqlParameter("Login", login));
                comm.Parameters.Add(new SqlParameter("Senha", senha));

                comm.CommandText = queryGet.ToString();

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();
                if (rd.HasRows)
                    autenticado = true;

                rd.Close();
            }
            catch (Exception ex)
            {
               // obj.Clear();
            }
            finally
            {
                con.Close();
            }

            return autenticado;
        }
    }
}