using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using NFModel;

namespace NFDAL
{
    public class LoteRpsDAL
    {
        public static IList<LoteRpsTO> Get(int start, int pageSize, ref int totRegistros, string textoFiltro, ref int totRegistrosFiltro, string sortColumn, string sortColumnDir)
        {
            IList<LoteRpsTO> objs = new List<LoteRpsTO>();

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
                    NumeroLote,
                    CpfCnpj,
                    InscricaoMunicipal,
                    Quantidade,

                    (ROW_NUMBER() OVER (").Append(ordenacao).Append(@"))
                    AS 'numeroLinha', 

                    (SELECT COUNT(Id) FROM LoteRps) 
				    AS 'totRegistros', 

					(SELECT COUNT(Id) FROM LoteRps 
					    WHERE
                        Id like @textoFiltro
                        OR
                        NumeroLote like @textoFiltro
                        OR
                        CpfCnpj collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        InscricaoMunicipal collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Quantidade like @textoFiltro
                    ) 
					AS 'totRegistrosFiltro'

	                FROM LoteRps
						WHERE
                        Id like @textoFiltro
                        OR
                        NumeroLote like @textoFiltro
                        OR
                        CpfCnpj collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        InscricaoMunicipal collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Quantidade like @textoFiltro) 

				AS todasLinhas
                WHERE todasLinhas.numeroLinha > (@start)"); 

                comm.Parameters.Add(new SqlParameter("pageSize", pageSize));
                comm.Parameters.Add(new SqlParameter("start", start));
                comm.Parameters.Add(new SqlParameter("textoFiltro", string.Format("%{0}%", textoFiltro)));

                comm.CommandText = queryGet.ToString();

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();

                LoteRpsTO obj;

                if (rd.Read())
                {
                    totRegistros = rd.GetInt32(6);
                    totRegistrosFiltro = rd.GetInt32(7);

                    obj = new LoteRpsTO
                    {
                        Id = rd.GetInt32(0),
                        NumeroLote = rd.IsDBNull(1) ? (Int32?)null : rd.GetInt32(1),
                        CpfCnpj = rd.IsDBNull(2) ? null : rd.GetString(2),
                        InscricaoMunicipal = rd.IsDBNull(3) ? null : rd.GetString(3),
                        Quantidade = rd.IsDBNull(4) ? (Int32?)null : rd.GetInt32(4)
                    };
                    objs.Add(obj);
                }
                while (rd.Read())
                {
                    obj = new LoteRpsTO
                    {
                        Id = rd.GetInt32(0),
                        NumeroLote = rd.IsDBNull(1) ? (Int32?)null : rd.GetInt32(1),
                        CpfCnpj = rd.IsDBNull(2) ? null : rd.GetString(2),
                        InscricaoMunicipal = rd.IsDBNull(3) ? null : rd.GetString(3),
                        Quantidade = rd.IsDBNull(4) ? (Int32?)null : rd.GetInt32(4)
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
                        chaveRegistro = rd.GetString(0),
                        valorRegistro = rd.GetString(1)
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
                
        public static int? Update(LoteRpsTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                UPDATE LoteRps 
                SET NumeroLote = @NumeroLote,
                CpfCnpj = @CpfCnpj,
                InscricaoMunicipal = @InscricaoMunicipal,
                Quantidade = @Quantidade
                WHERE Id = @Id
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
                
        public static int? Delete(LoteRpsTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                DELETE LoteRps 
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

        public static int GetUltimoRPS()
        {

            int ultimoRPS;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                SELECT ISNULL(MAX(Numero)+1, 1) as UltimoRps FROM Rps
                ";

                con.Open();
                ultimoRPS = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch (Exception ex)
            {
                ultimoRPS = 0;
            }
            finally
            {
                con.Close();
            }
            return ultimoRPS;

        }
    }
}