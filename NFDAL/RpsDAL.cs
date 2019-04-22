using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using NFModel;

namespace NFDAL
{
    public class RpsDAL
    {
        public static IList<RpsTO> Get(int start, int pageSize, ref int totRegistros, string textoFiltro, ref int totRegistrosFiltro, string sortColumn, string sortColumnDir)
        {
            IList<RpsTO> objs = new List<RpsTO>();

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
                    IdLoteRps,
                    Numero,
                    Serie,
                    Tipo,
                    DataEmissao,
                    Status,
                    Competencia,
                    ValorServico,
                    ISSRetido,
                    ItemListaServico,
                    CodigoCnae,
                    CodigoTributacaoMunicipio,
                    Discriminacao,
                    CodigoMunicipio,
                    ExigibilidadeISS,
                    MunicipioIncidencia,
                    CpfCnpj_Prestador,
                    CpfCnpj_Tomador,
                    OptanteSimplesNacional,
                    IncentivoFiscal,

                    (ROW_NUMBER() OVER (").Append(ordenacao).Append(@"))
                    AS 'numeroLinha', 

                    (SELECT COUNT(Id) FROM Rps) 
				    AS 'totRegistros', 

					(SELECT COUNT(Id) FROM Rps 
					    WHERE
                        Id like @textoFiltro
                        OR
                        IdLoteRps like @textoFiltro
                        OR
                        Numero like @textoFiltro
                        OR
                        Serie collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Tipo like @textoFiltro
                        OR
                        DataEmissao like @textoFiltro
                        OR
                        Status like @textoFiltro
                        OR
                        Competencia like @textoFiltro
                        OR
                        ValorServico collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        ISSRetido collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        ItemListaServico collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        CodigoCnae collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        CodigoTributacaoMunicipio collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Discriminacao collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        CodigoMunicipio collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        ExigibilidadeISS like @textoFiltro
                        OR
                        MunicipioIncidencia collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        CpfCnpj_Prestador collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        CpfCnpj_Tomador collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        OptanteSimplesNacional like @textoFiltro
                        OR
                        IncentivoFiscal like @textoFiltro
                    ) 
					AS 'totRegistrosFiltro'

	                FROM Rps
						WHERE
                        Id like @textoFiltro
                        OR
                        IdLoteRps like @textoFiltro
                        OR
                        Numero like @textoFiltro
                        OR
                        Serie collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Tipo like @textoFiltro
                        OR
                        DataEmissao like @textoFiltro
                        OR
                        Status like @textoFiltro
                        OR
                        Competencia like @textoFiltro
                        OR
                        ValorServico collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        ISSRetido collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        ItemListaServico collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        CodigoCnae collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        CodigoTributacaoMunicipio collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        Discriminacao collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        CodigoMunicipio collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        ExigibilidadeISS like @textoFiltro
                        OR
                        MunicipioIncidencia collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        CpfCnpj_Prestador collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        CpfCnpj_Tomador collate Latin1_General_CI_AI like @textoFiltro
                        OR
                        OptanteSimplesNacional like @textoFiltro
                        OR
                        IncentivoFiscal like @textoFiltro) 

				AS todasLinhas
                WHERE todasLinhas.numeroLinha > (@start)");

                comm.Parameters.Add(new SqlParameter("pageSize", pageSize));
                comm.Parameters.Add(new SqlParameter("start", start));
                comm.Parameters.Add(new SqlParameter("textoFiltro", string.Format("%{0}%", textoFiltro)));

                comm.CommandText = queryGet.ToString();

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();

                RpsTO obj;

                if (rd.Read())
                {
                    totRegistros = rd.GetInt32(22);
                    totRegistrosFiltro = rd.GetInt32(23);

                    obj = new RpsTO
                    {
                        Id = rd.GetInt32(0),
                        IdLoteRps = rd.GetInt32(1),
                        Numero = rd.IsDBNull(2) ? (Int32?)null : rd.GetInt32(2),
                        Serie = rd.IsDBNull(3) ? null : rd.GetString(3),
                        Tipo = rd.IsDBNull(4) ? (Int32?)null : rd.GetInt32(4),
                        DataEmissao = rd.IsDBNull(5) ? null : rd.GetDateTime(5).ToShortDateString(),
                        Status = rd.IsDBNull(6) ? (Int32?)null : rd.GetInt32(6),
                        Competencia = rd.IsDBNull(7) ? null : rd.GetDateTime(7).ToShortDateString(),
                        ValorServico = rd.IsDBNull(8) ? null : rd.GetString(8),
                        ISSRetido = rd.IsDBNull(9) ? null : rd.GetString(9),
                        ItemListaServico = rd.IsDBNull(10) ? null : rd.GetString(10),
                        CodigoCnae = rd.IsDBNull(11) ? null : rd.GetString(11),
                        CodigoTributacaoMunicipio = rd.IsDBNull(12) ? null : rd.GetString(12),
                        Discriminacao = rd.IsDBNull(13) ? null : rd.GetString(13).Replace("\n", ""),
                        CodigoMunicipio = rd.IsDBNull(14) ? null : rd.GetString(14),
                        ExigibilidadeISS = rd.IsDBNull(15) ? (Int32?)null : rd.GetInt32(15),
                        MunicipioIncidencia = rd.IsDBNull(16) ? null : rd.GetString(16),
                        CpfCnpj_Prestador = rd.IsDBNull(17) ? null : rd.GetString(17),
                        CpfCnpj_Tomador = rd.IsDBNull(18) ? null : rd.GetString(18),
                        OptanteSimplesNacional = rd.IsDBNull(19) ? (Int32?)null : rd.GetInt32(19),
                        IncentivoFiscal = rd.IsDBNull(20) ? (Int32?)null : rd.GetInt32(20)
                    };
                    objs.Add(obj);
                }
                while (rd.Read())
                {
                    obj = new RpsTO
                    {
                        Id = rd.GetInt32(0),
                        IdLoteRps = rd.GetInt32(1),
                        Numero = rd.IsDBNull(2) ? (Int32?)null : rd.GetInt32(2),
                        Serie = rd.IsDBNull(3) ? null : rd.GetString(3),
                        Tipo = rd.IsDBNull(4) ? (Int32?)null : rd.GetInt32(4),
                        DataEmissao = rd.IsDBNull(5) ? null : rd.GetDateTime(5).ToShortDateString(),
                        Status = rd.IsDBNull(6) ? (Int32?)null : rd.GetInt32(6),
                        Competencia = rd.IsDBNull(7) ? null : rd.GetDateTime(7).ToShortDateString(),
                        ValorServico = rd.IsDBNull(8) ? null : rd.GetString(8),
                        ISSRetido = rd.IsDBNull(9) ? null : rd.GetString(9),
                        ItemListaServico = rd.IsDBNull(10) ? null : rd.GetString(10),
                        CodigoCnae = rd.IsDBNull(11) ? null : rd.GetString(11),
                        CodigoTributacaoMunicipio = rd.IsDBNull(12) ? null : rd.GetString(12),
                        Discriminacao = rd.IsDBNull(13) ? null : rd.GetString(13).Replace("\n", ""),
                        CodigoMunicipio = rd.IsDBNull(14) ? null : rd.GetString(14),
                        ExigibilidadeISS = rd.IsDBNull(15) ? (Int32?)null : rd.GetInt32(15),
                        MunicipioIncidencia = rd.IsDBNull(16) ? null : rd.GetString(16),
                        CpfCnpj_Prestador = rd.IsDBNull(17) ? null : rd.GetString(17),
                        CpfCnpj_Tomador = rd.IsDBNull(18) ? null : rd.GetString(18),
                        OptanteSimplesNacional = rd.IsDBNull(19) ? (Int32?)null : rd.GetInt32(19),
                        IncentivoFiscal = rd.IsDBNull(20) ? (Int32?)null : rd.GetInt32(20)
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
        
        public static int? Insert(RpsTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                INSERT INTO Rps 
                (IdLoteRps, Numero, Serie, Tipo, DataEmissao, Status, Competencia, ValorServico, ISSRetido, ItemListaServico, CodigoCnae, CodigoTributacaoMunicipio, Discriminacao, CodigoMunicipio, ExigibilidadeISS, MunicipioIncidencia, CpfCnpj_Prestador, CpfCnpj_Tomador, OptanteSimplesNacional, IncentivoFiscal) VALUES 
                (@IdLoteRps, @Numero, @Serie, @Tipo, @DataEmissao, @Status, @Competencia, @ValorServico, @ISSRetido, @ItemListaServico, @CodigoCnae, @CodigoTributacaoMunicipio, @Discriminacao, @CodigoMunicipio, @ExigibilidadeISS, @MunicipioIncidencia, @CpfCnpj_Prestador, @CpfCnpj_Tomador, @OptanteSimplesNacional, @IncentivoFiscal)
                ";

                con.Open();

                object Numero = DBNull.Value;
                if (null != obj.Numero)
                {
                    Numero = obj.Numero;
                }

                object Serie = DBNull.Value;
                if (null != obj.Serie)
                {
                    Serie = obj.Serie;
                }

                object Tipo = DBNull.Value;
                if (null != obj.Tipo)
                {
                    Tipo = obj.Tipo;
                }

                object DataEmissao = DBNull.Value;
                if (null != obj.DataEmissao)
                {
                    DataEmissao = obj.DataEmissao;
                }

                object Status = DBNull.Value;
                if (null != obj.Status)
                {
                    Status = obj.Status;
                }

                object Competencia = DBNull.Value;
                if (null != obj.Competencia)
                {
                    Competencia = obj.Competencia;
                }

                object ValorServico = DBNull.Value;
                if (null != obj.ValorServico)
                {
                    ValorServico = obj.ValorServico;
                }

                object ISSRetido = DBNull.Value;
                if (null != obj.ISSRetido)
                {
                    ISSRetido = obj.ISSRetido;
                }

                object ItemListaServico = DBNull.Value;
                if (null != obj.ItemListaServico)
                {
                    ItemListaServico = obj.ItemListaServico;
                }

                object CodigoCnae = DBNull.Value;
                if (null != obj.CodigoCnae)
                {
                    CodigoCnae = obj.CodigoCnae;
                }

                object CodigoTributacaoMunicipio = DBNull.Value;
                if (null != obj.CodigoTributacaoMunicipio)
                {
                    CodigoTributacaoMunicipio = obj.CodigoTributacaoMunicipio;
                }

                object Discriminacao = DBNull.Value;
                if (null != obj.Discriminacao)
                {
                    Discriminacao = obj.Discriminacao;
                }

                object CodigoMunicipio = DBNull.Value;
                if (null != obj.CodigoMunicipio)
                {
                    CodigoMunicipio = obj.CodigoMunicipio;
                }

                object ExigibilidadeISS = DBNull.Value;
                if (null != obj.ExigibilidadeISS)
                {
                    ExigibilidadeISS = obj.ExigibilidadeISS;
                }

                object MunicipioIncidencia = DBNull.Value;
                if (null != obj.MunicipioIncidencia)
                {
                    MunicipioIncidencia = obj.MunicipioIncidencia;
                }

                object CpfCnpj_Prestador = DBNull.Value;
                if (null != obj.CpfCnpj_Prestador)
                {
                    CpfCnpj_Prestador = obj.CpfCnpj_Prestador;
                }

                object CpfCnpj_Tomador = DBNull.Value;
                if (null != obj.CpfCnpj_Tomador)
                {
                    CpfCnpj_Tomador = obj.CpfCnpj_Tomador;
                }

                object OptanteSimplesNacional = DBNull.Value;
                if (null != obj.OptanteSimplesNacional)
                {
                    OptanteSimplesNacional = obj.OptanteSimplesNacional;
                }

                object IncentivoFiscal = DBNull.Value;
                if (null != obj.IncentivoFiscal)
                {
                    IncentivoFiscal = obj.IncentivoFiscal;
                }


                comm.Parameters.Add(new SqlParameter("IdLoteRps", obj.IdLoteRps));
                comm.Parameters.Add(new SqlParameter("Numero", Numero));
                comm.Parameters.Add(new SqlParameter("Serie", Serie));
                comm.Parameters.Add(new SqlParameter("Tipo", Tipo));
                comm.Parameters.Add(new SqlParameter("DataEmissao", DataEmissao));
                comm.Parameters.Add(new SqlParameter("Status", Status));
                comm.Parameters.Add(new SqlParameter("Competencia", Competencia));
                comm.Parameters.Add(new SqlParameter("ValorServico", ValorServico));
                comm.Parameters.Add(new SqlParameter("ISSRetido", ISSRetido));
                comm.Parameters.Add(new SqlParameter("ItemListaServico", ItemListaServico));
                comm.Parameters.Add(new SqlParameter("CodigoCnae", CodigoCnae));
                comm.Parameters.Add(new SqlParameter("CodigoTributacaoMunicipio", CodigoTributacaoMunicipio));
                comm.Parameters.Add(new SqlParameter("Discriminacao", Discriminacao));
                comm.Parameters.Add(new SqlParameter("CodigoMunicipio", CodigoMunicipio));
                comm.Parameters.Add(new SqlParameter("ExigibilidadeISS", ExigibilidadeISS));
                comm.Parameters.Add(new SqlParameter("MunicipioIncidencia", MunicipioIncidencia));
                comm.Parameters.Add(new SqlParameter("CpfCnpj_Prestador", CpfCnpj_Prestador));
                comm.Parameters.Add(new SqlParameter("CpfCnpj_Tomador", CpfCnpj_Tomador));
                comm.Parameters.Add(new SqlParameter("OptanteSimplesNacional", OptanteSimplesNacional));
                comm.Parameters.Add(new SqlParameter("IncentivoFiscal", IncentivoFiscal));

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

        public static int? Update(RpsTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                UPDATE Rps 
                SET IdLoteRps = @IdLoteRps,
                Numero = @Numero,
                Serie = @Serie,
                Tipo = @Tipo,
                DataEmissao = @DataEmissao,
                Status = @Status,
                Competencia = @Competencia,
                ValorServico = @ValorServico,
                ISSRetido = @ISSRetido,
                ItemListaServico = @ItemListaServico,
                CodigoCnae = @CodigoCnae,
                CodigoTributacaoMunicipio = @CodigoTributacaoMunicipio,
                Discriminacao = @Discriminacao,
                CodigoMunicipio = @CodigoMunicipio,
                ExigibilidadeISS = @ExigibilidadeISS,
                MunicipioIncidencia = @MunicipioIncidencia,
                CpfCnpj_Prestador = @CpfCnpj_Prestador,
                CpfCnpj_Tomador = @CpfCnpj_Tomador,
                OptanteSimplesNacional = @OptanteSimplesNacional,
                IncentivoFiscal = @IncentivoFiscal
                WHERE Id = @Id
                ";

                con.Open();

                object Numero = DBNull.Value;
                if (null != obj.Numero)
                {
                    Numero = obj.Numero;
                }

                object Serie = DBNull.Value;
                if (null != obj.Serie)
                {
                    Serie = obj.Serie;
                }

                object Tipo = DBNull.Value;
                if (null != obj.Tipo)
                {
                    Tipo = obj.Tipo;
                }

                object DataEmissao = DBNull.Value;
                if (null != obj.DataEmissao)
                {
                    DataEmissao = obj.DataEmissao;
                }

                object Status = DBNull.Value;
                if (null != obj.Status)
                {
                    Status = obj.Status;
                }

                object Competencia = DBNull.Value;
                if (null != obj.Competencia)
                {
                    Competencia = obj.Competencia;
                }

                object ValorServico = DBNull.Value;
                if (null != obj.ValorServico)
                {
                    ValorServico = obj.ValorServico;
                }

                object ISSRetido = DBNull.Value;
                if (null != obj.ISSRetido)
                {
                    ISSRetido = obj.ISSRetido;
                }

                object ItemListaServico = DBNull.Value;
                if (null != obj.ItemListaServico)
                {
                    ItemListaServico = obj.ItemListaServico;
                }

                object CodigoCnae = DBNull.Value;
                if (null != obj.CodigoCnae)
                {
                    CodigoCnae = obj.CodigoCnae;
                }

                object CodigoTributacaoMunicipio = DBNull.Value;
                if (null != obj.CodigoTributacaoMunicipio)
                {
                    CodigoTributacaoMunicipio = obj.CodigoTributacaoMunicipio;
                }

                object Discriminacao = DBNull.Value;
                if (null != obj.Discriminacao)
                {
                    Discriminacao = obj.Discriminacao;
                }

                object CodigoMunicipio = DBNull.Value;
                if (null != obj.CodigoMunicipio)
                {
                    CodigoMunicipio = obj.CodigoMunicipio;
                }

                object ExigibilidadeISS = DBNull.Value;
                if (null != obj.ExigibilidadeISS)
                {
                    ExigibilidadeISS = obj.ExigibilidadeISS;
                }

                object MunicipioIncidencia = DBNull.Value;
                if (null != obj.MunicipioIncidencia)
                {
                    MunicipioIncidencia = obj.MunicipioIncidencia;
                }

                object CpfCnpj_Prestador = DBNull.Value;
                if (null != obj.CpfCnpj_Prestador)
                {
                    CpfCnpj_Prestador = obj.CpfCnpj_Prestador;
                }

                object CpfCnpj_Tomador = DBNull.Value;
                if (null != obj.CpfCnpj_Tomador)
                {
                    CpfCnpj_Tomador = obj.CpfCnpj_Tomador;
                }

                object OptanteSimplesNacional = DBNull.Value;
                if (null != obj.OptanteSimplesNacional)
                {
                    OptanteSimplesNacional = obj.OptanteSimplesNacional;
                }

                object IncentivoFiscal = DBNull.Value;
                if (null != obj.IncentivoFiscal)
                {
                    IncentivoFiscal = obj.IncentivoFiscal;
                }

                comm.Parameters.Add(new SqlParameter("IdLoteRps", obj.IdLoteRps));
                comm.Parameters.Add(new SqlParameter("Numero", Numero));
                comm.Parameters.Add(new SqlParameter("Serie", Serie));
                comm.Parameters.Add(new SqlParameter("Tipo", Tipo));
                comm.Parameters.Add(new SqlParameter("DataEmissao", DataEmissao));
                comm.Parameters.Add(new SqlParameter("Status", Status));
                comm.Parameters.Add(new SqlParameter("Competencia", Competencia));
                comm.Parameters.Add(new SqlParameter("ValorServico", ValorServico));
                comm.Parameters.Add(new SqlParameter("ISSRetido", ISSRetido));
                comm.Parameters.Add(new SqlParameter("ItemListaServico", ItemListaServico));
                comm.Parameters.Add(new SqlParameter("CodigoCnae", CodigoCnae));
                comm.Parameters.Add(new SqlParameter("CodigoTributacaoMunicipio", CodigoTributacaoMunicipio));
                comm.Parameters.Add(new SqlParameter("Discriminacao", Discriminacao));
                comm.Parameters.Add(new SqlParameter("CodigoMunicipio", CodigoMunicipio));
                comm.Parameters.Add(new SqlParameter("ExigibilidadeISS", ExigibilidadeISS));
                comm.Parameters.Add(new SqlParameter("MunicipioIncidencia", MunicipioIncidencia));
                comm.Parameters.Add(new SqlParameter("CpfCnpj_Prestador", CpfCnpj_Prestador));
                comm.Parameters.Add(new SqlParameter("CpfCnpj_Tomador", CpfCnpj_Tomador));
                comm.Parameters.Add(new SqlParameter("OptanteSimplesNacional", OptanteSimplesNacional));
                comm.Parameters.Add(new SqlParameter("IncentivoFiscal", IncentivoFiscal));
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

        public static int? Delete(RpsTO obj)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                DELETE Rps 
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

        public static int GetUltimoLote()
        {

            int ultimoLote;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = @"
                SELECT ISNULL(MAX(NumeroLote)+1, 1) as UltimoNumeroLote FROM LoteRps
                ";

                con.Open();
                ultimoLote = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch (Exception ex)
            {
                ultimoLote = 0;
            }
            finally
            {
                con.Close();
            }
            return ultimoLote;

        }

        public static int? InsertTransaction(LoteRpsTO obj0, RpsTO obj1, DadosEnvioTO obj2)
        {
            int? nrLinhas;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;

            SqlCommand comm0 = new SqlCommand();
            comm0.Connection = con;
            comm0.CommandText = @"
                INSERT INTO LoteRps 
                (NumeroLote, CpfCnpj, InscricaoMunicipal, Quantidade) OUTPUT Inserted.Id VALUES 
                (@NumeroLote, @CpfCnpj, @InscricaoMunicipal, @Quantidade)
                ";

            SqlCommand comm1 = new SqlCommand();
            comm1.Connection = con;
            comm1.CommandText = @"
            INSERT INTO Rps 
            (IdLoteRps, Numero, Serie, Tipo, DataEmissao, Status, Competencia, ValorServico, ISSRetido, ItemListaServico, CodigoCnae, CodigoTributacaoMunicipio, Discriminacao, CodigoMunicipio, ExigibilidadeISS, MunicipioIncidencia, CpfCnpj_Prestador, CpfCnpj_Tomador, OptanteSimplesNacional, IncentivoFiscal) VALUES 
            (@IdLoteRps, @Numero, @Serie, @Tipo, @DataEmissao, @Status, @Competencia, @ValorServico, @ISSRetido, @ItemListaServico, @CodigoCnae, @CodigoTributacaoMunicipio, @Discriminacao, @CodigoMunicipio, @ExigibilidadeISS, @MunicipioIncidencia, @CpfCnpj_Prestador, @CpfCnpj_Tomador, @OptanteSimplesNacional, @IncentivoFiscal)
            ";

            SqlCommand comm2 = new SqlCommand();
            comm2.Connection = con;
            comm2.CommandText = @"
            INSERT INTO DadosEnvio 
            (NumeroLoteRps, Data, ArquivoImportacao, ConteudoArquivoImportacao, ArquivoRemessa, XMLRemessa, ArquivoRetorno, XMLRetorno) VALUES 
            (@NumeroLoteRps, @Data, @ArquivoImportacao, @ConteudoArquivoImportacao, @ArquivoRemessa, @XMLRemessa, @ArquivoRetorno, @XMLRetorno)
            ";

            object NumeroLote = DBNull.Value;
            if (null != obj0.NumeroLote)
            {
                NumeroLote = obj0.NumeroLote;
            }

            object CpfCnpj = DBNull.Value;
            if (null != obj0.CpfCnpj)
            {
                CpfCnpj = obj0.CpfCnpj;
            }

            object InscricaoMunicipal = DBNull.Value;
            if (null != obj0.InscricaoMunicipal)
            {
                InscricaoMunicipal = obj0.InscricaoMunicipal;
            }

            object Quantidade = DBNull.Value;
            if (null != obj0.Quantidade)
            {
                Quantidade = obj0.Quantidade;
            }


            object Numero = DBNull.Value;
            if (null != obj1.Numero)
            {
                Numero = obj1.Numero;
            }

            object Serie = DBNull.Value;
            if (null != obj1.Serie)
            {
                Serie = obj1.Serie;
            }

            object Tipo = DBNull.Value;
            if (null != obj1.Tipo)
            {
                Tipo = obj1.Tipo;
            }

            object DataEmissao = DBNull.Value;
            if (null != obj1.DataEmissao)
            {
                DataEmissao = obj1.DataEmissao;
            }

            object Status = DBNull.Value;
            if (null != obj1.Status)
            {
                Status = obj1.Status;
            }

            object Competencia = DBNull.Value;
            if (null != obj1.Competencia)
            {
                Competencia = obj1.Competencia;
            }

            object ValorServico = DBNull.Value;
            if (null != obj1.ValorServico)
            {
                ValorServico = obj1.ValorServico;
            }

            object ISSRetido = DBNull.Value;
            if (null != obj1.ISSRetido)
            {
                ISSRetido = obj1.ISSRetido;
            }

            object ItemListaServico = DBNull.Value;
            if (null != obj1.ItemListaServico)
            {
                ItemListaServico = obj1.ItemListaServico;
            }

            object CodigoCnae = DBNull.Value;
            if (null != obj1.CodigoCnae)
            {
                CodigoCnae = obj1.CodigoCnae;
            }

            object CodigoTributacaoMunicipio = DBNull.Value;
            if (null != obj1.CodigoTributacaoMunicipio)
            {
                CodigoTributacaoMunicipio = obj1.CodigoTributacaoMunicipio;
            }

            object Discriminacao = DBNull.Value;
            if (null != obj1.Discriminacao)
            {
                Discriminacao = obj1.Discriminacao;
            }

            object CodigoMunicipio = DBNull.Value;
            if (null != obj1.CodigoMunicipio)
            {
                CodigoMunicipio = obj1.CodigoMunicipio;
            }

            object ExigibilidadeISS = DBNull.Value;
            if (null != obj1.ExigibilidadeISS)
            {
                ExigibilidadeISS = obj1.ExigibilidadeISS;
            }

            object MunicipioIncidencia = DBNull.Value;
            if (null != obj1.MunicipioIncidencia)
            {
                MunicipioIncidencia = obj1.MunicipioIncidencia;
            }

            object CpfCnpj_Prestador = DBNull.Value;
            if (null != obj1.CpfCnpj_Prestador)
            {
                CpfCnpj_Prestador = obj1.CpfCnpj_Prestador;
            }

            object CpfCnpj_Tomador = DBNull.Value;
            if (null != obj1.CpfCnpj_Tomador)
            {
                CpfCnpj_Tomador = obj1.CpfCnpj_Tomador;
            }

            object OptanteSimplesNacional = DBNull.Value;
            if (null != obj1.OptanteSimplesNacional)
            {
                OptanteSimplesNacional = obj1.OptanteSimplesNacional;
            }

            object IncentivoFiscal = DBNull.Value;
            if (null != obj1.IncentivoFiscal)
            {
                IncentivoFiscal = obj1.IncentivoFiscal;
            }

            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            try
            {
                comm0.Transaction = tran;
                comm0.Parameters.Add(new SqlParameter("NumeroLote", NumeroLote));
                comm0.Parameters.Add(new SqlParameter("CpfCnpj", CpfCnpj));
                comm0.Parameters.Add(new SqlParameter("InscricaoMunicipal", InscricaoMunicipal));
                comm0.Parameters.Add(new SqlParameter("Quantidade", Quantidade));
                nrLinhas = comm0.ExecuteNonQuery();

                comm1.Transaction = tran;
                //comm1.Parameters.Add(new SqlParameter("IdLoteRps", obj1.IdLoteRps));
                comm1.Parameters.Add(new SqlParameter("IdLoteRps", nrLinhas));
                comm1.Parameters.Add(new SqlParameter("Numero", Numero));
                comm1.Parameters.Add(new SqlParameter("Serie", Serie));
                comm1.Parameters.Add(new SqlParameter("Tipo", Tipo));
                comm1.Parameters.Add(new SqlParameter("DataEmissao", DataEmissao));
                comm1.Parameters.Add(new SqlParameter("Status", Status));
                comm1.Parameters.Add(new SqlParameter("Competencia", Competencia));
                comm1.Parameters.Add(new SqlParameter("ValorServico", ValorServico));
                comm1.Parameters.Add(new SqlParameter("ISSRetido", ISSRetido));
                comm1.Parameters.Add(new SqlParameter("ItemListaServico", ItemListaServico));
                comm1.Parameters.Add(new SqlParameter("CodigoCnae", CodigoCnae));
                comm1.Parameters.Add(new SqlParameter("CodigoTributacaoMunicipio", CodigoTributacaoMunicipio));
                comm1.Parameters.Add(new SqlParameter("Discriminacao", Discriminacao));
                comm1.Parameters.Add(new SqlParameter("CodigoMunicipio", CodigoMunicipio));
                comm1.Parameters.Add(new SqlParameter("ExigibilidadeISS", ExigibilidadeISS));
                comm1.Parameters.Add(new SqlParameter("MunicipioIncidencia", MunicipioIncidencia));
                comm1.Parameters.Add(new SqlParameter("CpfCnpj_Prestador", CpfCnpj_Prestador));
                comm1.Parameters.Add(new SqlParameter("CpfCnpj_Tomador", CpfCnpj_Tomador));
                comm1.Parameters.Add(new SqlParameter("OptanteSimplesNacional", OptanteSimplesNacional));
                comm1.Parameters.Add(new SqlParameter("IncentivoFiscal", IncentivoFiscal));
                nrLinhas = comm1.ExecuteNonQuery();

                comm2.Transaction = tran;
                comm2.Parameters.Add(new SqlParameter("NumeroLoteRps", obj2.NumeroLoteRps));
                comm2.Parameters.Add(new SqlParameter("Data", obj2.Data));
                comm2.Parameters.Add(new SqlParameter("ArquivoImportacao", obj2.ArquivoImportacao));
                comm2.Parameters.Add(new SqlParameter("ConteudoArquivoImportacao", obj2.ConteudoArquivoImportacao));
                comm2.Parameters.Add(new SqlParameter("ArquivoRemessa", obj2.ArquivoRemessa));
                comm2.Parameters.Add(new SqlParameter("XMLRemessa", obj2.XMLRemessa));
                comm2.Parameters.Add(new SqlParameter("ArquivoRetorno", obj2.ArquivoRetorno));
                comm2.Parameters.Add(new SqlParameter("XMLRetorno", obj2.XMLRetorno));
                nrLinhas = comm2.ExecuteNonQuery();

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
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