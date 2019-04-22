using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using NFDAL;
using NFModel;
using System.Text.RegularExpressions;

namespace NFBusiness
{
    public class Importacao
    {
        private static string[] lines = { };

        public static bool FileIsValid(FileInfo file)
        {
            bool result = false;
            try
            {
                if ((file.Extension.ToUpper().Equals(".TXT")) || (file.Extension.ToUpper().Equals(".CSV")))
                {
                    lines = System.IO.File.ReadAllLines(file.FullName);
                    if (lines.Length != 0)
                    {
                        string[] linhaCab = lines[0].Split('|');
                        // Verifica se o arquivo possui o cabeçalho SISPEC
                        if ((lines[0].IndexOf("000|") == 0) || (linhaCab.Length == 17))
                        {
                            result = true;
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public static string[,] GetValues()
        {
            string[,] values = { };
            try
            {
                int i = 0;
                string[] columns = new string[50];
                values = new string[lines.Length, columns.Length];
                // Preenche a matriz values com os valores dos campos
                foreach (string line in lines)
                {
                    columns = line.Split('|');
                    int j = 0;
                    foreach (string field in columns)
                    {
                        values[i, j] = field;
                        j++;
                    }
                    i++;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return values;
        }

        public static List<ArquivoRps> SetValues()
        {
            List<ArquivoRps> arquivosRpsCol = new List<ArquivoRps>();
            try
            {
                string pathOrigemImportacao = ParametroDAL.GetValor("OrigemImportacaoPath");
                DirectoryInfo diretorio = new DirectoryInfo(@pathOrigemImportacao);

                FileInfo[] arquivos = diretorio.GetFiles();
                foreach (FileInfo file in arquivos)
                {
                    if (FileIsValid(file))
                    {
                        string[,] values = GetValues();
                        ArquivoRps arquivoRps = new ArquivoRps();

                        arquivoRps.NomeArquivo = file.Name;
                        arquivoRps.NomeArquivoREM = file.Name.Replace(".txt", ".REM");
                        arquivoRps.NomeArquivoRET = file.Name.Replace(".txt", ".RET");

                        string[] DadosNFOrigem = ParametroDAL.GetValor("DadosNFOrigem").Split(';');

                        // Primeira linha --> Dados do Prestador
                        if ((lines[1].IndexOf("001|") == 0))
                        {

                            arquivoRps.Rps.NumeroLote = RpsDAL.GetUltimoLote();
                            arquivoRps.Rps.CpfCnpj = DadosNFOrigem[0];
                            arquivoRps.Rps.InscricaoMunicipal = DadosNFOrigem[1];
                            arquivoRps.Rps.Quantidade = "1"; // Analisar o arquivo se será 1 ou N lotes por arquivo

                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Numero = Convert.ToString(LoteRpsDAL.GetUltimoRPS()); //Sequencial => Controle do banco --> Só precisa ser gerado sequencialmente quando o lote for processado com sucesso
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Serie = DadosNFOrigem[2];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Tipo = DadosNFOrigem[3];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.DataEmissao = DateTime.Now.ToString("yyyy-MM-dd");
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Status = DadosNFOrigem[4];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Competencia = arquivoRps.Rps.InfDeclaracaoPrestacaoServico.DataEmissao;

                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Prestador.CpfCnpj = arquivoRps.Rps.CpfCnpj;
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Prestador.InscricaoMunicipal = arquivoRps.Rps.InscricaoMunicipal;

                        }
                        // Segunda linha --> Dados do Tomador
                        if ((lines[2].IndexOf("002|") == 0))
                        {
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Tomador.CpfCnpj = values[2, 1];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Tomador.RazaoSocial = values[2, 3];

                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Logradouro = values[2, 11];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Numero = values[2, 12];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Bairro = values[2, 8];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.CodigoMunicipio = values[2, 6];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Uf = values[2, 7];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Cep = values[2, 10];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Tomador.Contato.Telefone = values[2, 5];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Tomador.Contato.Email = values[2, 4].ToLower().TrimEnd();
                        }
                        // Terceira linha --> Dados do Serviço
                        if ((lines[3].IndexOf("005|") == 0))
                        {
                            decimal valorServico = Convert.ToDecimal(values[3, 1]) / 100;
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.Valores.ValorServicos = Convert.ToString(valorServico).Replace(",", ".");
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.IssRetido = DadosNFOrigem[5];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.ItemListaServico = DadosNFOrigem[6];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.CodigoCnae = DadosNFOrigem[7];

                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.CodigoTributacaoMunicipio = arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.ItemListaServico;


                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.Discriminacao = values[3, 18];
                            // Ir na tabela Regra verificar se há alguma regra estabelecida
                            RegraTO regraGeral = RegraDAL.GetTodos("*");
                            if (regraGeral.Regra != null)
                            {
                                //string discriminacao = regraGeral.Discriminacao;
                                arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.Discriminacao = regraGeral.Discriminacao;
                            }

                            RegraTO regra = RegraDAL.GetPorCpfCnpjPrestador(arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Tomador.CpfCnpj);
                            if (regra.CpfCnpj_Prestador != null)
                            {
                                string discriminacao = regra.Discriminacao;
                                string[] srt_discriminacao = discriminacao.Split('|');

                                foreach (string valor in srt_discriminacao)
                                {
                                    switch (valor)
                                    {
                                        case "DataEmissao":
                                            // Campo crítico
                                            string dtEmissao = GetExpressaoComData(arquivoRps.Rps.InfDeclaracaoPrestacaoServico.DataEmissao, regra.Expressoes);
                                            discriminacao = discriminacao.Replace(valor, dtEmissao);
                                            break;
                                        case "ValorServicos":
                                            discriminacao = discriminacao.Replace(valor, arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.Valores.ValorServicos);
                                            break;
                                        case "IssRetido":
                                            discriminacao = discriminacao.Replace(valor, arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.IssRetido);
                                            break;
                                        case "ItemListaServico":
                                            discriminacao = discriminacao.Replace(valor, arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.ItemListaServico);
                                            break;
                                        case "CodigoCnae":
                                            discriminacao = discriminacao.Replace(valor, arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.CodigoCnae);
                                            break;
                                    }
                                }

                                arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.Discriminacao = discriminacao.Replace("|", "");
                            }

                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.CodigoMunicipio = DadosNFOrigem[8];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.ExigibilidadeISS = DadosNFOrigem[9];
                            arquivoRps.Rps.InfDeclaracaoPrestacaoServico.Servico.MunicipioIncidencia = DadosNFOrigem[10];
                        }

                        arquivoRps.Rps.InfDeclaracaoPrestacaoServico.OptanteSimplesNacional = DadosNFOrigem[11];
                        arquivoRps.Rps.InfDeclaracaoPrestacaoServico.IncentivoFiscal = DadosNFOrigem[12];

                        arquivosRpsCol.Add(arquivoRps);

                    }

                }

            }
            catch (Exception)
            {

                throw;
            }
            return arquivosRpsCol;
        }

        public static void GerarArquivo(string nome, string tipo, string conteudo)
        {

            try
            {
                string path = "";
                string pathOrigemRemessa = ParametroDAL.GetValor("OrigemRemessaPath");
                string pathOrigemRetorno = ParametroDAL.GetValor("OrigemRetornoPath");
                if (tipo.Equals("REM"))
                    path = @pathOrigemRemessa + nome;
                else
                    path = @pathOrigemRetorno + nome;

                if (!File.Exists(path))
                {

                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(conteudo);
                    }
                }
            }
            catch (Exception e)
            {

                System.Console.WriteLine(e.Message);

            }

        }

        public static string LerArquivoImportacao(string pathDestinoImportacao)
        {
            StringBuilder sb = new StringBuilder();
            if (File.Exists(pathDestinoImportacao))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(pathDestinoImportacao))
                    {
                        String linha;
                        while ((linha = sr.ReadLine()) != null)
                        {
                            sb.AppendLine(linha);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return sb.ToString();

        }

        public static string GetExpressaoComData(string dtEmissao, string expressao)
        {
            string dataEmissao = Convert.ToDateTime(dtEmissao).ToString("dd-MM-yyyy");
            if (!String.IsNullOrEmpty(expressao))
            {
                string[] srt_expressao = expressao.Split('|');
                if (expressao.Contains("DataEmissao"))
                {
                    Regex r = new Regex(@"\{[^\}]+?\}");
                    Match m = r.Match(expressao);

                    string dias = m.Value.Split(';')[0];
                    if (m.Value.Contains("+"))
                    {
                        dias = m.Value.Split('+')[1].Split('}')[0];
                        dataEmissao = Convert.ToDateTime(dataEmissao).AddDays(Convert.ToDouble(dias)).ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        dias = m.Value.Split('-')[1].Split('}')[0];
                        dataEmissao = Convert.ToDateTime(dataEmissao).AddDays(-(Convert.ToDouble(dias))).ToString("dd-MM-yyyy");
                    }
                }
            }
            return dataEmissao;
        }
    }
}
