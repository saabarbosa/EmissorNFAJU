using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using NFModel;

namespace NFDAL
{
    public class ParametroDAL
    {
        public static IList<ParametroTO> Get(int start, int pageSize, ref int totRegistros, string textoFiltro, ref int totRegistrosFiltro, string sortColumn, string sortColumnDir)
        {
            IList<ParametroTO> objs = new List<ParametroTO>();

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;

            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;

                string ordenacao;
                if (string.IsNullOrEmpty(sortColumn))
                {
                    ordenacao = "ORDER BY Id";
                }
                else
                {
                    ordenacao = string.Format("ORDER BY {0} {1}", sortColumn, sortColumnDir);
                }
                StringBuilder queryGet = new StringBuilder(@"
                SELECT TOP (@pageSize) *
                FROM (
				    SELECT 
                    Id,
                    Chave,
                    Valor,

                    (ROW_NUMBER() OVER (").Append(ordenacao).Append(@"))
                    AS 'numeroLinha', 

                    (SELECT COUNT(Id) FROM Parametro) 
				    AS 'totRegistros', 

					(SELECT COUNT(Id) FROM Parametro 
					    WHERE
                        Id like @textoFiltro
                        OR
                        Chave collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Valor collate Latin1_General_CI_AI like @textoFiltro
                    ) 
					AS 'totRegistrosFiltro'

	                FROM Parametro
						WHERE
                        Id like @textoFiltro
                        OR
                        Chave collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Valor collate Latin1_General_CI_AI like @textoFiltro) 

				AS todasLinhas
                WHERE todasLinhas.numeroLinha > (@start)"); 

                comm.Parameters.Add(new SqlParameter("pageSize", pageSize));
                comm.Parameters.Add(new SqlParameter("start", start));
                comm.Parameters.Add(new SqlParameter("textoFiltro", string.Format("%{0}%", textoFiltro)));
                comm.CommandText = queryGet.ToString(); 

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();

                ParametroTO obj;

                if (rd.Read())
                {
                    totRegistros = rd.GetInt32(4);
                    totRegistrosFiltro = rd.GetInt32(5);

                    obj = new ParametroTO
                    {
                        Id = rd.GetInt32(0),
                        Chave = rd.GetString(1),
                        Valor = rd.GetString(2).Replace("\\", "\\\\")
                };
                    objs.Add(obj);
                }
                while (rd.Read())
                {
                    obj = new ParametroTO
                    {
                        Id = rd.GetInt32(0),
                        Chave = rd.GetString(1),
                        Valor = rd.GetString(2).Replace("\\", "\\\\")
                    };
                    objs.Add(obj);
                }
                rd.Close();
            }
            catch (Exception ex)
            {
                objs.Clear();
            }
            finally
            {
                con.Close();
            }

            return objs;
        }
        
        public static int? Insert(ParametroTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                INSERT INTO Parametro 
                (Chave, Valor) VALUES 
                (@Chave, @Valor)
                ";

                con.Open();

                comm.Parameters.Add(new SqlParameter("Chave", obj.Chave));
                comm.Parameters.Add(new SqlParameter("Valor", obj.Valor));

                nrLinhas = comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                nrLinhas = null;
            }
            finally
            {
                con.Close();
            }
            return nrLinhas;
        }
                
        public static int? Update(ParametroTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                UPDATE Parametro 
                SET Chave = @Chave,
                Valor = @Valor
                WHERE Id = @Id
                ";

                con.Open();

                comm.Parameters.Add(new SqlParameter("Chave", obj.Chave));
                comm.Parameters.Add(new SqlParameter("Valor", obj.Valor));
                comm.Parameters.Add(new SqlParameter("Id", obj.Id));

                nrLinhas = comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                nrLinhas = null;
            }
            finally
            {
                con.Close();
            }
            return nrLinhas;
        }
                
        public static int? Delete(ParametroTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                DELETE Parametro 
                WHERE Id = @Id
                ";

                con.Open();
        
                comm.Parameters.Add(new SqlParameter("Id", obj.Id));

                nrLinhas = comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                nrLinhas = null;
            }
            finally
            {
                con.Close();
            }
            return nrLinhas;
        }

        public static string GetValor(string chave)
        {
            ParametroTO obj = new ParametroTO();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;

            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;

                StringBuilder queryGet = new StringBuilder(@"
				    SELECT 
                    Id,
                    Chave,
                    Valor
	                FROM Parametro
                    WHERE Chave = @Chave
                ");

                comm.Parameters.Add(new SqlParameter("Chave", chave));

                comm.CommandText = queryGet.ToString();

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();


                if (rd.Read())
                {

                    obj = new ParametroTO
                    {
                        Id = rd.GetInt32(0),
                        Chave = rd.GetString(1),
                        Valor = rd.GetString(2)
                    };
   
                }

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

            return obj.Valor;
        }
    }
}