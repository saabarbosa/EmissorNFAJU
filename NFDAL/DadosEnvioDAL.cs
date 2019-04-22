using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using NFModel;

namespace NFDAL
{
    public class DadosEnvioDAL
    {
        public static IList<DadosEnvioTO> Get(int start, int pageSize, ref int totRegistros, string textoFiltro, ref int totRegistrosFiltro, string sortColumn, string sortColumnDir)
        {
            IList<DadosEnvioTO> objs = new List<DadosEnvioTO>();

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
                    NumeroLoteRps,
                    Data,
                    ArquivoImportacao,
                    ConteudoArquivoImportacao,
                    ArquivoRemessa,
                    XMLRemessa,
                    ArquivoRetorno,
                    XMLRetorno,

                    (ROW_NUMBER() OVER (").Append(ordenacao).Append(@"))
                    AS 'numeroLinha', 

                    (SELECT COUNT(Id) FROM DadosEnvio) 
				    AS 'totRegistros', 

					(SELECT COUNT(Id) FROM DadosEnvio 
					    WHERE
                        Id like @textoFiltro
                        OR
                        NumeroLoteRps collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Data like @textoFiltro
                        OR
                        ArquivoImportacao collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        ConteudoArquivoImportacao collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        ArquivoRemessa collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        XMLRemessa collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        ArquivoRetorno collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        XMLRetorno collate Latin1_General_CI_AI like @textoFiltro
                    ) 
					AS 'totRegistrosFiltro'

	                FROM DadosEnvio
						WHERE
                        Id like @textoFiltro
                        OR
                        NumeroLoteRps collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Data like @textoFiltro
                        OR
                        ArquivoImportacao collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        ConteudoArquivoImportacao collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        ArquivoRemessa collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        XMLRemessa collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        ArquivoRetorno collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        XMLRetorno collate Latin1_General_CI_AI like @textoFiltro) 

				AS todasLinhas
                WHERE todasLinhas.numeroLinha > (@start)"); 

                comm.Parameters.Add(new SqlParameter("pageSize", pageSize));
                comm.Parameters.Add(new SqlParameter("start", start));
                comm.Parameters.Add(new SqlParameter("textoFiltro", string.Format("%{0}%", textoFiltro)));

                comm.CommandText = queryGet.ToString();

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();

                DadosEnvioTO obj;

                if (rd.Read())
                {
                    totRegistros = rd.GetInt32(10);
                    totRegistrosFiltro = rd.GetInt32(11);

                    obj = new DadosEnvioTO
                    {
                        Id = rd.GetInt32(0),
                        NumeroLoteRps = rd.GetString(1),
                        Data = rd.GetDateTime(2).ToString("dd/MM/yyyy hh:mm:ss"),
                        ArquivoImportacao = rd.GetString(3).Replace("\\", "\\\\"),
                        ConteudoArquivoImportacao = rd.GetString(4),
                        ArquivoRemessa = rd.GetString(5).Replace("\\", "\\\\"),
                        XMLRemessa = rd.GetString(6).Replace("\"", ""),
                        ArquivoRetorno = rd.GetString(7).Replace("\\", "\\\\"),
                        XMLRetorno = rd.GetString(8).Replace("\"", "")
                    };
                    objs.Add(obj);
                }
                while (rd.Read())
                {
                    obj = new DadosEnvioTO
                    {
                        Id = rd.GetInt32(0),
                        NumeroLoteRps = rd.GetString(1),
                        Data = rd.GetDateTime(2).ToString("dd/MM/yyyy hh:mm:ss"),
                        ArquivoImportacao = rd.GetString(3).Replace("\\", "\\\\"),
                        ConteudoArquivoImportacao = rd.GetString(4).Replace("\\", "\\\\"),
                        ArquivoRemessa = rd.GetString(5).Replace("\\", "\\\\"),
                        XMLRemessa = rd.GetString(6).Replace("\"", ""),
                        ArquivoRetorno = rd.GetString(7).Replace("\\", "\\\\"),
                        XMLRetorno = rd.GetString(8).Replace("\"", "")
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
        
        public static int? Insert(DadosEnvioTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                INSERT INTO DadosEnvio 
                (NumeroLoteRps, Data, ArquivoImportacao, ConteudoArquivoImportacao, ArquivoRemessa, XMLRemessa, ArquivoRetorno, XMLRetorno) VALUES 
                (@NumeroLoteRps, @Data, @ArquivoImportacao, @ConteudoArquivoImportacao, @ArquivoRemessa, @XMLRemessa, @ArquivoRetorno, @XMLRetorno)
                ";

                con.Open();

                comm.Parameters.Add(new SqlParameter("NumeroLoteRps", obj.NumeroLoteRps));
                comm.Parameters.Add(new SqlParameter("Data", DateTime.Now));
                comm.Parameters.Add(new SqlParameter("ArquivoImportacao", obj.ArquivoImportacao));
                comm.Parameters.Add(new SqlParameter("ConteudoArquivoImportacao", obj.ConteudoArquivoImportacao));
                comm.Parameters.Add(new SqlParameter("ArquivoRemessa", obj.ArquivoRemessa));
                comm.Parameters.Add(new SqlParameter("XMLRemessa", obj.XMLRemessa));
                comm.Parameters.Add(new SqlParameter("ArquivoRetorno", obj.ArquivoRetorno));
                comm.Parameters.Add(new SqlParameter("XMLRetorno", obj.XMLRetorno));

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
                
        public static int? Update(DadosEnvioTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                UPDATE DadosEnvio 
                SET NumeroLoteRps = @NumeroLoteRps,
                Data = @Data,
                ArquivoImportacao = @ArquivoImportacao,
                ConteudoArquivoImportacao = @ConteudoArquivoImportacao,
                ArquivoRemessa = @ArquivoRemessa,
                XMLRemessa = @XMLRemessa,
                ArquivoRetorno = @ArquivoRetorno,
                XMLRetorno = @XMLRetorno
                WHERE Id = @Id
                ";

                con.Open();

                comm.Parameters.Add(new SqlParameter("NumeroLoteRps", obj.NumeroLoteRps));
                comm.Parameters.Add(new SqlParameter("Data", DateTime.Now));
                comm.Parameters.Add(new SqlParameter("ArquivoImportacao", obj.ArquivoImportacao));
                comm.Parameters.Add(new SqlParameter("ConteudoArquivoImportacao", obj.ConteudoArquivoImportacao));
                comm.Parameters.Add(new SqlParameter("ArquivoRemessa", obj.ArquivoRemessa));
                comm.Parameters.Add(new SqlParameter("XMLRemessa", obj.XMLRemessa));
                comm.Parameters.Add(new SqlParameter("ArquivoRetorno", obj.ArquivoRetorno));
                comm.Parameters.Add(new SqlParameter("XMLRetorno", obj.XMLRetorno));
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
                
        public static int? Delete(DadosEnvioTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                DELETE DadosEnvio 
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
    }
}