using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using NFModel;

namespace NFDAL
{
    public class RegraDAL
    {
        public static IList<RegraTO> Get(int start, int pageSize, ref int totRegistros, string textoFiltro, ref int totRegistrosFiltro, string sortColumn, string sortColumnDir)
        {
            IList<RegraTO> objs = new List<RegraTO>();

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
                    Regra,
                    CpfCnpj_Prestador,
                    Discriminacao,
                    Expressoes,

                    (ROW_NUMBER() OVER (").Append(ordenacao).Append(@"))
                    AS 'numeroLinha', 

                    (SELECT COUNT(Id) FROM Regra) 
				    AS 'totRegistros', 

					(SELECT COUNT(Id) FROM Regra 
					    WHERE
                        Id like @textoFiltro
                        OR
                        Regra collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        CpfCnpj_Prestador collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Discriminacao collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Expressoes collate Latin1_General_CI_AI like @textoFiltro
                    ) 
					AS 'totRegistrosFiltro'

	                FROM Regra
						WHERE
                        Id like @textoFiltro
                        OR
                        Regra collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        CpfCnpj_Prestador collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Discriminacao collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Expressoes collate Latin1_General_CI_AI like @textoFiltro)

				AS todasLinhas
                WHERE todasLinhas.numeroLinha > (@start)"); 


                comm.Parameters.Add(new SqlParameter("pageSize", pageSize));
                comm.Parameters.Add(new SqlParameter("start", start));
                comm.Parameters.Add(new SqlParameter("textoFiltro", string.Format("%{0}%", textoFiltro)));

                comm.CommandText = queryGet.ToString();

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();

                RegraTO obj;

                if (rd.Read())
                {
                    totRegistros = rd.GetInt32(6);
                    totRegistrosFiltro = rd.GetInt32(7);

                    obj = new RegraTO
                    {
                        Id = rd.GetInt32(0),
                        Regra = rd.IsDBNull(1) ? null : rd.GetString(1),
                        CpfCnpj_Prestador = rd.IsDBNull(2) ? null : rd.GetString(2),
                        Discriminacao = rd.IsDBNull(3) ? null : rd.GetString(3),
                        Expressoes = rd.IsDBNull(4) ? null : rd.GetString(4)
                    };
                    objs.Add(obj);
                }
                while (rd.Read())
                {
                    obj = new RegraTO
                    {
                        Id = rd.GetInt32(0),
                        Regra = rd.IsDBNull(1) ? null : rd.GetString(1),
                        CpfCnpj_Prestador = rd.IsDBNull(2) ? null : rd.GetString(2),
                        Discriminacao = rd.IsDBNull(3) ? null : rd.GetString(3),
                        Expressoes = rd.IsDBNull(4) ? null : rd.GetString(4)
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
        
        public static IList<object> GetParaChaveEstrangeira()
        {
            IList<object> objs = new List<object>();

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;

            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;

                StringBuilder queryGet = new StringBuilder(@"
                SELECT
                chaveRegistro = CONVERT(VARCHAR, Id),
                valorRegistro = CONVERT(VARCHAR, Id)

                FROM LoteRps

                ORDER BY valorRegistro");

                comm.CommandText = queryGet.ToString();

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();

                object obj;

                while (rd.Read())
                {
                    obj = new 
                    {
                        Id = rd.GetInt32(0),
                        Regra = rd.IsDBNull(1) ? null : rd.GetString(1),
                        CpfCnpj_Prestador = rd.IsDBNull(2) ? null : rd.GetString(2),
                        Discriminacao = rd.IsDBNull(3) ? null : rd.GetString(3),
                        Expressoes = rd.IsDBNull(4) ? null : rd.GetString(4)
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
     
        /*
        public static int? Insert(LoteRpsTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                INSERT INTO LoteRps 
                (NumeroLote, CpfCnpj, InscricaoMunicipal, Quantidade) OUTPUT Inserted.Id VALUES 
                (@NumeroLote, @CpfCnpj, @InscricaoMunicipal, @Quantidade)
                ";

                con.Open();

                object NumeroLote = DBNull.Value;
                if (null != obj.NumeroLote)
                {
                    NumeroLote = obj.NumeroLote;
                }

                object CpfCnpj = DBNull.Value;
                if (null != obj.CpfCnpj)
                {
                    CpfCnpj = obj.CpfCnpj;
                }

                object InscricaoMunicipal = DBNull.Value;
                if (null != obj.InscricaoMunicipal)
                {
                    InscricaoMunicipal = obj.InscricaoMunicipal;
                }

                object Quantidade = DBNull.Value;
                if (null != obj.Quantidade)
                {
                    Quantidade = obj.Quantidade;
                }

                comm.Parameters.Add(new SqlParameter("NumeroLote", NumeroLote));
                comm.Parameters.Add(new SqlParameter("CpfCnpj", CpfCnpj));
                comm.Parameters.Add(new SqlParameter("InscricaoMunicipal", InscricaoMunicipal));
                comm.Parameters.Add(new SqlParameter("Quantidade", Quantidade));

                //nrLinhas = comm.ExecuteNonQuery();
                nrLinhas = Convert.ToInt32(comm.ExecuteScalar());
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
        */
        public static int? Update(RegraTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                UPDATE Regra 
                SET Regra = @Regra,
                CpfCnpj_Prestador = @CpfCnpj_Prestador,
                Discriminacao = @Discriminacao,
                Expressoes = @Expressoes
                WHERE Id = @Id
                ";

                con.Open();

                object Regra = DBNull.Value;
                if (null != obj.Regra)
                {
                    Regra = obj.Regra;
                }

                object CpfCnpj_Prestador = DBNull.Value;
                if (null != obj.CpfCnpj_Prestador)
                {
                    CpfCnpj_Prestador = obj.CpfCnpj_Prestador;
                }

                object Discriminacao = DBNull.Value;
                if (null != obj.Discriminacao)
                {
                    Discriminacao = obj.Discriminacao;
                }

                object Expressoes = DBNull.Value;
                if (null != obj.Expressoes)
                {
                    Expressoes = obj.Expressoes;
                }


                comm.Parameters.Add(new SqlParameter("Regra", Regra));
                comm.Parameters.Add(new SqlParameter("CpfCnpj_Prestador", CpfCnpj_Prestador));
                comm.Parameters.Add(new SqlParameter("Discriminacao", Discriminacao));
                comm.Parameters.Add(new SqlParameter("Expressoes", Expressoes));
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

        
        public static int? Delete(RegraTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                DELETE Regra 
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

        public static RegraTO GetPorCpfCnpjPrestador(string CpfCnpj_Prestador)
        {
            RegraTO obj = new RegraTO();

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;

            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;

                StringBuilder queryGet = new StringBuilder(@"
                SELECT
                *
                FROM Regra 

                WHERE CpfCnpj_Prestador = (@CpfCnpj_Prestador)"); 

                comm.Parameters.Add(new SqlParameter("CpfCnpj_Prestador", CpfCnpj_Prestador));


                comm.CommandText = queryGet.ToString();

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();


                while (rd.Read())
                {
                    obj = new RegraTO
                    {
                        Id = rd.GetInt32(0),
                        Regra = rd.IsDBNull(1) ? null : rd.GetString(1),
                        CpfCnpj_Prestador = rd.IsDBNull(2) ? null : rd.GetString(2),
                        Discriminacao = rd.IsDBNull(3) ? null : rd.GetString(3),
                        Expressoes = rd.IsDBNull(4) ? null : rd.GetString(4)
                    };
                    
                }
                rd.Close();
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                con.Close();
            }

            return obj;
        }

        public static RegraTO GetTodos(string Regra)
        {
            RegraTO obj = new RegraTO();

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;

            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;

                StringBuilder queryGet = new StringBuilder(@"
                SELECT
                *
                FROM Regra 

                WHERE Regra = (@Regra)");

                comm.Parameters.Add(new SqlParameter("Regra", Regra));


                comm.CommandText = queryGet.ToString();

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();


                while (rd.Read())
                {
                    obj = new RegraTO
                    {
                        Id = rd.GetInt32(0),
                        Regra = rd.IsDBNull(1) ? null : rd.GetString(1),
                        CpfCnpj_Prestador = rd.IsDBNull(2) ? null : rd.GetString(2),
                        Discriminacao = rd.IsDBNull(3) ? null : rd.GetString(3),
                        Expressoes = rd.IsDBNull(4) ? null : rd.GetString(4)
                    };

                }
                rd.Close();
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                con.Close();
            }

            return obj;
        }

    }
}