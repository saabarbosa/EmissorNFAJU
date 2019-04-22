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
    public class old_Importacao
    {

        private static string nomeArquivo;

        public static List<Rps> ObterDadosEnvio()
        {
            List<Rps> rpsCollection = new List<Rps>(); 
            Rps rps = null;
            try {
                string pathOrigemImportacao = ParametroDAL.GetValor("OrigemImportacaoPath");
                DirectoryInfo diretorio = new DirectoryInfo(@pathOrigemImportacao);
                FileInfo[] arquivos = diretorio.GetFiles();
                if (arquivos.Length != 0)
                {
                    // Busca arquivos do diretorio
                    foreach (FileInfo file in arquivos)
                    {

                        // Somente arquivos TXT/CSV
                        if ((file.Extension.ToUpper().Equals(".TXT")) || (file.Extension.ToUpper().Equals(".CSV")))
                        {
                            string[] lines = System.IO.File.ReadAllLines(file.FullName);
                            if (lines.Length != 0)
                            {
                                string[] linhaCab = lines[0].Split('|');
                                string[] columns = new string[50];
                                string[,] values = new string[lines.Length, columns.Length];

                                // Verifica se o arquivo possui o cabeçalho SISPEC
                                if ((lines[0].IndexOf("000|") == 0) || (linhaCab.Length == 17))
                                {
                                    int i = 0;

                                    // Preenche a matriz values com os valores dos campos
                                    foreach (string line in lines)
                                    {
                                        columns = line.Split('|');
                                        int j = 0;
                                        foreach (string field in columns)
                                        {
                                            values[i, j] = field;
                                            //Console.WriteLine("\t Line[" + i + "] - Field[" + j + "]: " + field);
                                            j++;
                                        }
                                        i++;
                                    }
                                    rps = new Rps();
                                    //string[] DadosNFPlamedAju = ParametroDAL.GetValor("DadosNFPlamedAju").Split(';');
                                    string[] DadosNFOrigem = ParametroDAL.GetValor("DadosNFOrigem").Split(';');

                                    // Primeira linha --> Dados do Prestador
                                    if ((lines[1].IndexOf("001|") == 0))
                                    {

                                        rps.NumeroLote = RpsDAL.GetUltimoLote();
                                        rps.CpfCnpj = DadosNFOrigem[0];
                                        rps.InscricaoMunicipal = DadosNFOrigem[1];
                                        rps.Quantidade = "1"; // Analisar o arquivo se será 1 ou N lotes por arquivo



                                        rps.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Numero = Convert.ToString(LoteRpsDAL.GetUltimoRPS()); //Sequencial => Controle do banco --> Só precisa ser gerado sequencialmente quando o lote for processado com sucesso
                                        rps.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Serie = DadosNFOrigem[2];
                                        rps.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Tipo = DadosNFOrigem[3];
                                        rps.InfDeclaracaoPrestacaoServico.DataEmissao = DateTime.Now.ToString("yyyy-MM-dd");
                                        rps.InfDeclaracaoPrestacaoServico.Status = DadosNFOrigem[4];
                                        rps.InfDeclaracaoPrestacaoServico.Competencia = rps.InfDeclaracaoPrestacaoServico.DataEmissao;

                                        rps.InfDeclaracaoPrestacaoServico.Prestador.CpfCnpj = rps.CpfCnpj;
                                        rps.InfDeclaracaoPrestacaoServico.Prestador.InscricaoMunicipal = rps.InscricaoMunicipal;

                                    }
                                    // Segunda linha --> Dados do Tomador
                                    if ((lines[2].IndexOf("002|") == 0))
                                    {
                                        rps.InfDeclaracaoPrestacaoServico.Tomador.CpfCnpj = values[2, 1];
                                        rps.InfDeclaracaoPrestacaoServico.Tomador.RazaoSocial = values[2, 3];

                                        rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Logradouro = values[2, 11];
                                        rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Numero = values[2, 12];
                                        rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Bairro = values[2, 8];
                                        rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.CodigoMunicipio = values[2, 6];
                                        rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Uf = values[2, 7];
                                        rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Cep = values[2, 10];
                                        rps.InfDeclaracaoPrestacaoServico.Tomador.Contato.Telefone = values[2, 5];
                                        rps.InfDeclaracaoPrestacaoServico.Tomador.Contato.Email = values[2, 4].ToLower().TrimEnd();
                                    }
                                    // Terceira linha --> Dados do Serviço
                                    if ((lines[3].IndexOf("005|") == 0))
                                    {
                                        decimal valorServico = Convert.ToDecimal(values[3, 1]) / 100;
                                        rps.InfDeclaracaoPrestacaoServico.Servico.Valores.ValorServicos = Convert.ToString(valorServico).Replace(",", ".");
                                        rps.InfDeclaracaoPrestacaoServico.Servico.IssRetido = DadosNFOrigem[5];
                                        rps.InfDeclaracaoPrestacaoServico.Servico.ItemListaServico = DadosNFOrigem[6];
                                        rps.InfDeclaracaoPrestacaoServico.Servico.CodigoCnae = DadosNFOrigem[7];

                                        rps.InfDeclaracaoPrestacaoServico.Servico.CodigoTributacaoMunicipio = rps.InfDeclaracaoPrestacaoServico.Servico.ItemListaServico;


                                        rps.InfDeclaracaoPrestacaoServico.Servico.Discriminacao = values[3, 18];
                                        // Ir na tabela Regra verificar se há alguma regra estabelecida
                                        RegraTO regraGeral = RegraDAL.GetTodos("*");
                                        if (regraGeral.Regra != null)
                                        {
                                            //string discriminacao = regraGeral.Discriminacao;
                                            rps.InfDeclaracaoPrestacaoServico.Servico.Discriminacao = regraGeral.Discriminacao;
                                        }
                                        
                                        RegraTO regra = RegraDAL.GetPorCpfCnpjPrestador(rps.InfDeclaracaoPrestacaoServico.Tomador.CpfCnpj);
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
                                                        string dtEmissao = GetExpressaoComData(rps.InfDeclaracaoPrestacaoServico.DataEmissao, regra.Expressoes);
                                                        discriminacao = discriminacao.Replace(valor, dtEmissao);
                                                        break;
                                                    case "ValorServicos":
                                                        discriminacao = discriminacao.Replace(valor, rps.InfDeclaracaoPrestacaoServico.Servico.Valores.ValorServicos);
                                                        break;
                                                    case "IssRetido":
                                                        discriminacao = discriminacao.Replace(valor, rps.InfDeclaracaoPrestacaoServico.Servico.IssRetido);
                                                        break;
                                                    case "ItemListaServico":
                                                        discriminacao = discriminacao.Replace(valor, rps.InfDeclaracaoPrestacaoServico.Servico.ItemListaServico);
                                                        break;
                                                    case "CodigoCnae":
                                                        discriminacao = discriminacao.Replace(valor, rps.InfDeclaracaoPrestacaoServico.Servico.CodigoCnae);
                                                        break;
                                                }
                                            }
                                            
                                            rps.InfDeclaracaoPrestacaoServico.Servico.Discriminacao = discriminacao.Replace("|", "");
                                        }

                                        rps.InfDeclaracaoPrestacaoServico.Servico.CodigoMunicipio = DadosNFOrigem[8];
                                        rps.InfDeclaracaoPrestacaoServico.Servico.ExigibilidadeISS = DadosNFOrigem[9];
                                        rps.InfDeclaracaoPrestacaoServico.Servico.MunicipioIncidencia = DadosNFOrigem[10];
                                    }

                                    rps.InfDeclaracaoPrestacaoServico.OptanteSimplesNacional = DadosNFOrigem[11];
                                    rps.InfDeclaracaoPrestacaoServico.IncentivoFiscal = DadosNFOrigem[12];

                                    rpsCollection.Add(rps);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Conteúdo de arquivo no formato inválido (layout inválido).");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Arquivo no formato inválido (tipo inválido).");
                        }

                        nomeArquivo = file.Name.Split('.')[0];
                    }

                }
                else
                {
                    Console.WriteLine("Nenhum arquivo para importação.");
                }
             } catch (Exception e){
                    Console.WriteLine(e.Message);
            }
            return rpsCollection;
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

        public static void GerarArquivo(string nome, string tipo, string conteudo)
        {

            try
            {
                string path = "";
                string pathOrigemRemessa = ParametroDAL.GetValor("OrigemRemessaPath");
                string pathOrigemRetorno = ParametroDAL.GetValor("OrigemRetornoPath");
                if (tipo.Equals("REM"))
                    path = @pathOrigemRemessa + nomeArquivo;
                else
                    path = @pathOrigemRetorno + nomeArquivo;

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


        public static void MoverArquivos(string arquivo)
        {
            string pathOrigemImportacao = ParametroDAL.GetValor("OrigemImportacaoPath");
            string pathDestinoImportacao = ParametroDAL.GetValor("DestinoImportacaoPath");
            DirectoryInfo dirOrigemImportacao = new DirectoryInfo(@pathOrigemImportacao);
            string dirDestinoImportacao = @pathDestinoImportacao;

            foreach (FileInfo f in dirOrigemImportacao.GetFiles("*.txt"))
            {
                if(f.Name.Equals(arquivo + ".txt"))
                    File.Move(f.FullName, dirDestinoImportacao + f.Name);
            }

            string pathOrigemRemessa = ParametroDAL.GetValor("OrigemRemessaPath");
            string pathDestinoRemessa = ParametroDAL.GetValor("DestinoRemessaPath");
            DirectoryInfo dirOrigemRemessa = new DirectoryInfo(@pathOrigemRemessa);
            string dirDestinoRemessa = @pathDestinoRemessa;


            // reavaliar essa rotina, pois não deixe mover um arquivo caso exista outro com mesmo nome na mesma pasta
            foreach (FileInfo f in dirOrigemRemessa.GetFiles("*.REM"))
            {
                if (f.Name.Equals(arquivo + ".REM"))
                    File.Move(f.FullName, dirDestinoRemessa + f.Name);
            }

            string pathOrigemRetorno = ParametroDAL.GetValor("OrigemRetornoPath");
            string pathDestinoRetorno = ParametroDAL.GetValor("DestinoRetornoPath");

            DirectoryInfo dirOrigemRetorno = new DirectoryInfo(@pathOrigemRetorno);
            string dirDestinoRetorno = @pathDestinoRetorno;

            foreach (FileInfo f in dirOrigemRetorno.GetFiles("*.RET"))
            {
                if (f.Name.Equals(arquivo + ".RET"))
                    File.Move(f.FullName, dirDestinoRetorno + f.Name);
            }

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
