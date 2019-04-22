using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFBusiness.NfseWSService;

using System.Xml;
using NFDAL;
using NFModel;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace NFBusiness
{
    public class NotaFiscal
    {

        // GerarNotaFiscal
        public string Gerar()
        {
            string msg = "Não encontrou arquivo para importação.";
            try
            {
                // Retornar uma coleção de RPs
                //List<Rps> lotesRps = Importacao.ObterDadosEnvio();
                List<ArquivoRps> lotesRps = Importacao.SetValues();
                if (lotesRps.Count > 0)
                {
                    msg = "Encontrou " + lotesRps.Count.ToString() + " arquivo(s) para importação.";
                    int i = 0;
                    foreach (ArquivoRps dados in lotesRps)
                    {

                        string xmlCabecalho = GerarXMLCabecalho();

                        // Recepcionando o arquivo XML assinado
                        string xmlDados = GerarXML(dados);
                        xmlDados = AssinarXML(xmlDados, "Rps");
                        xmlDados = AssinarXML(xmlDados, "EnviarLoteRpsEnvio");

                        NfseWSServiceSoapClient servico = new NfseWSServiceSoapClient();
                        string xmlRetorno  = servico.RecepcionarLoteRps(xmlCabecalho, xmlDados);

                        //string consultLote = servico.ConsultarLoteRps(xmlCabecalho, xmlDados);

                        // Gerando arquivos REM/RET nos respectivos diretorios
                        Importacao.GerarArquivo(dados.NomeArquivoREM, "REM", xmlDados);
                        Importacao.GerarArquivo(dados.NomeArquivoRET, "RET", xmlRetorno);

                        string pathOrigemImportacao = ParametroDAL.GetValor("OrigemImportacaoPath");
                        string pathOrigemRemessa    = ParametroDAL.GetValor("OrigemRemessaPath");
                        string pathOrigemRetorno    = ParametroDAL.GetValor("OrigemRetornoPath");
                        string pathDestinoImportacao= ParametroDAL.GetValor("DestinoImportacaoPath");
                        string pathDestinoRemessa   = ParametroDAL.GetValor("DestinoRemessaPath");
                        string pathDestinoRetorno   = ParametroDAL.GetValor("DestinoRetornoPath");

                        string arquivoImportacao = @pathDestinoImportacao + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + dados.NomeArquivo;
                        // Movendo arquivo de importacao da origem para destino com outro nome
                        File.Move(@pathOrigemImportacao + dados.NomeArquivo, arquivoImportacao);

                        string arquivoRemessa    = @pathDestinoRemessa + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + dados.NomeArquivoREM;
                        // Movendo arquivo de remessa da origem para destino com outro nome
                        File.Move(@pathOrigemRemessa + dados.NomeArquivoREM, arquivoRemessa);

                        string arquivoRetorno    = @pathDestinoRetorno + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + dados.NomeArquivoRET;
                        // Movendo arquivo de retorno da origem para destino com outro nome
                        File.Move(@pathOrigemRetorno + dados.NomeArquivoRET, arquivoRetorno);

                        // Lendo arquivo da pasta Backup
                        string conteudoImportacao = Importacao.LerArquivoImportacao(arquivoImportacao);

                         // Gravando na base o lote
                        SalvarLote(dados.Rps, lotesRps.Count, i);
                        // Gravando na base o resultado do envio
                        SalvarDadosEnvio(dados.Rps, xmlDados, xmlRetorno, arquivoImportacao, conteudoImportacao, arquivoRemessa, arquivoRetorno);

                        i++;
                        System.Threading.Thread.Sleep(5000); //Delay de 5 segundos para chamada sincrona 
                    }

                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;

            }
            return msg;

        }

        private void SalvarLote(Rps dados, int count, int seq)
        {
            try
            {
                // Inserindo informacoes na tabela LoteRPS
                LoteRpsTO loteRpsTo = new LoteRpsTO();
                loteRpsTo.NumeroLote = dados.NumeroLote;
                loteRpsTo.CpfCnpj = dados.CpfCnpj;
                loteRpsTo.InscricaoMunicipal = dados.InscricaoMunicipal;
                loteRpsTo.Quantidade = count;
                int? IdLote = LoteRpsDAL.Insert(loteRpsTo);

                RpsTO rps = new RpsTO();
                rps.IdLoteRps = IdLote;
                rps.Numero = Convert.ToInt32(dados.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Numero);
                rps.Serie = dados.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Serie;
                rps.Tipo = Convert.ToInt32(dados.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Tipo);
                rps.DataEmissao = dados.InfDeclaracaoPrestacaoServico.DataEmissao;
                rps.Status = 1; //Identificar
                rps.Competencia = rps.DataEmissao;
                rps.ValorServico = dados.InfDeclaracaoPrestacaoServico.Servico.Valores.ValorServicos.Replace(".", ",");
                rps.ISSRetido = dados.InfDeclaracaoPrestacaoServico.Servico.IssRetido;
                rps.ItemListaServico = dados.InfDeclaracaoPrestacaoServico.Servico.ItemListaServico;
                rps.CodigoCnae = dados.InfDeclaracaoPrestacaoServico.Servico.CodigoCnae;
                rps.CodigoTributacaoMunicipio = dados.InfDeclaracaoPrestacaoServico.Servico.CodigoTributacaoMunicipio;
                rps.Discriminacao = dados.InfDeclaracaoPrestacaoServico.Servico.Discriminacao;
                rps.CodigoMunicipio = dados.InfDeclaracaoPrestacaoServico.Servico.CodigoMunicipio;
                rps.ExigibilidadeISS = Convert.ToInt16(dados.InfDeclaracaoPrestacaoServico.Servico.ExigibilidadeISS);
                rps.MunicipioIncidencia = dados.InfDeclaracaoPrestacaoServico.Servico.MunicipioIncidencia;

                rps.CpfCnpj_Prestador = dados.InfDeclaracaoPrestacaoServico.Prestador.CpfCnpj;

                /*
                //Obter CNPJ do prestador, verificar na tabela se existe
                PrestadorTO prestador = new PrestadorTO();
                prestador.CpfCnpj_Prestador = dados.InfDeclaracaoPrestacaoServico.Prestador.CpfCnpj;
                prestador.InscricaoMunicipal = dados.InfDeclaracaoPrestacaoServico.Prestador.InscricaoMunicipal;

                //Obter CNPJ do tomador, verificar na tabela se existe
                TomadorTO tomador = new TomadorTO();
                tomador.CpfCnpj_Tomador = dados.InfDeclaracaoPrestacaoServico.Tomador.CpfCnpj;
                tomador.RazaoSocial = dados.InfDeclaracaoPrestacaoServico.Tomador.RazaoSocial;
                tomador.Endereco = dados.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Logradouro;
                tomador.Numero = dados.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Numero;
                tomador.Bairro = dados.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Bairro;
                tomador.CodigoMunicipio = dados.InfDeclaracaoPrestacaoServico.Tomador.Endereco.CodigoMunicipio;
                tomador.Uf = dados.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Uf;
                tomador.Cep = dados.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Cep;
                tomador.Telefone = dados.InfDeclaracaoPrestacaoServico.Tomador.Contato.Telefone;
                tomador.Email = dados.InfDeclaracaoPrestacaoServico.Tomador.Contato.Email;
                */
                rps.CpfCnpj_Tomador = dados.InfDeclaracaoPrestacaoServico.Tomador.CpfCnpj;

                rps.OptanteSimplesNacional = Convert.ToInt16(dados.InfDeclaracaoPrestacaoServico.OptanteSimplesNacional);
                rps.IncentivoFiscal = Convert.ToInt16(dados.InfDeclaracaoPrestacaoServico.IncentivoFiscal);

                RpsDAL.Insert(rps);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void SalvarDadosEnvio(Rps dados, string xmlDados, string xmlRetorno, string arquivoImportacao, string conteudoImportacao, string arquivoRemessa, string arquivoRetorno)
        {
            try
            {
                DadosEnvioTO dadosEnvio = new DadosEnvioTO();
                dadosEnvio.NumeroLoteRps = Convert.ToString(dados.NumeroLote);

                dadosEnvio.Data = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                dadosEnvio.XMLRemessa = xmlDados;
                dadosEnvio.XMLRetorno = xmlRetorno;

                dadosEnvio.ArquivoImportacao = arquivoImportacao;
                dadosEnvio.ArquivoRemessa = arquivoRemessa;
                dadosEnvio.ArquivoRetorno = arquivoRetorno;
                dadosEnvio.ConteudoArquivoImportacao = conteudoImportacao;

                DadosEnvioDAL.Insert(dadosEnvio);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void SalvarDados(Rps dados, int count, int seq, string xmlDados, string xmlRetorno, string arquivoImportacao, string conteudoImportacao, string arquivoRemessa, string arquivoRetorno)
        {
            try
            {
                // Inserindo informacoes na tabela LoteRPS
                LoteRpsTO loteRps = new LoteRpsTO();
                loteRps.NumeroLote = dados.NumeroLote;
                loteRps.CpfCnpj = dados.CpfCnpj;
                loteRps.InscricaoMunicipal = dados.InscricaoMunicipal;
                loteRps.Quantidade = count;
                int? IdLote = LoteRpsDAL.Insert(loteRps);

                RpsTO rps = new RpsTO();
                rps.IdLoteRps = IdLote;
                rps.Numero = Convert.ToInt32(dados.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Numero);
                rps.Serie = dados.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Serie;
                rps.Tipo = Convert.ToInt32(dados.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Tipo);
                rps.DataEmissao = dados.InfDeclaracaoPrestacaoServico.DataEmissao;
                rps.Status = 1; //Identificar
                rps.Competencia = rps.DataEmissao;
                rps.ValorServico = dados.InfDeclaracaoPrestacaoServico.Servico.Valores.ValorServicos.Replace(".", ",");
                rps.ISSRetido = dados.InfDeclaracaoPrestacaoServico.Servico.IssRetido;
                rps.ItemListaServico = dados.InfDeclaracaoPrestacaoServico.Servico.ItemListaServico;
                rps.CodigoCnae = dados.InfDeclaracaoPrestacaoServico.Servico.CodigoCnae;
                rps.CodigoTributacaoMunicipio = dados.InfDeclaracaoPrestacaoServico.Servico.CodigoTributacaoMunicipio;
                rps.Discriminacao = dados.InfDeclaracaoPrestacaoServico.Servico.Discriminacao;
                rps.CodigoMunicipio = dados.InfDeclaracaoPrestacaoServico.Servico.CodigoMunicipio;
                rps.ExigibilidadeISS = Convert.ToInt16(dados.InfDeclaracaoPrestacaoServico.Servico.ExigibilidadeISS);
                rps.MunicipioIncidencia = dados.InfDeclaracaoPrestacaoServico.Servico.MunicipioIncidencia;

                rps.CpfCnpj_Prestador = dados.InfDeclaracaoPrestacaoServico.Prestador.CpfCnpj;

                /*
                //Obter CNPJ do prestador, verificar na tabela se existe
                PrestadorTO prestador = new PrestadorTO();
                prestador.CpfCnpj_Prestador = dados.InfDeclaracaoPrestacaoServico.Prestador.CpfCnpj;
                prestador.InscricaoMunicipal = dados.InfDeclaracaoPrestacaoServico.Prestador.InscricaoMunicipal;

                //Obter CNPJ do tomador, verificar na tabela se existe
                TomadorTO tomador = new TomadorTO();
                tomador.CpfCnpj_Tomador = dados.InfDeclaracaoPrestacaoServico.Tomador.CpfCnpj;
                tomador.RazaoSocial = dados.InfDeclaracaoPrestacaoServico.Tomador.RazaoSocial;
                tomador.Endereco = dados.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Logradouro;
                tomador.Numero = dados.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Numero;
                tomador.Bairro = dados.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Bairro;
                tomador.CodigoMunicipio = dados.InfDeclaracaoPrestacaoServico.Tomador.Endereco.CodigoMunicipio;
                tomador.Uf = dados.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Uf;
                tomador.Cep = dados.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Cep;
                tomador.Telefone = dados.InfDeclaracaoPrestacaoServico.Tomador.Contato.Telefone;
                tomador.Email = dados.InfDeclaracaoPrestacaoServico.Tomador.Contato.Email;
                */
                rps.CpfCnpj_Tomador = dados.InfDeclaracaoPrestacaoServico.Tomador.CpfCnpj;

                rps.OptanteSimplesNacional = Convert.ToInt16(dados.InfDeclaracaoPrestacaoServico.OptanteSimplesNacional);
                rps.IncentivoFiscal = Convert.ToInt16(dados.InfDeclaracaoPrestacaoServico.IncentivoFiscal);

                DadosEnvioTO dadosEnvio = new DadosEnvioTO();
                dadosEnvio.NumeroLoteRps = Convert.ToString(dados.NumeroLote);

                dadosEnvio.XMLRemessa = xmlDados;
                dadosEnvio.XMLRetorno = xmlRetorno;

                dadosEnvio.ArquivoImportacao = arquivoImportacao;
                dadosEnvio.ArquivoRemessa = arquivoRemessa;
                dadosEnvio.ArquivoRetorno = arquivoRetorno;
                dadosEnvio.ConteudoArquivoImportacao = conteudoImportacao;

                //RpsDAL.InsertTransaction(loteRps, rps, dadosEnvio);
                RpsDAL.Insert(rps);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private X509Certificate2 ObterCertificado()
        {
            try
            {
                string pathCertPath = ParametroDAL.GetValor("CertPath");
                string certPath = @pathCertPath;
                string certPass = ParametroDAL.GetValor("CertPass");
                X509Certificate2 myCert = new X509Certificate2(certPath, certPass); //parametro adicionado no final
                return myCert;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string AssinarXML(string xml, string node)
        {
            AssinadorDeXML assinador = new AssinadorDeXML();
            return assinador.AssinarXML(xml, node, ObterCertificado());
        }

        private string GerarXMLCabecalho()
        {
            return System.IO.File.ReadAllText(@ParametroDAL.GetValor("XmlPathCabecalho"));
        }

        private string GerarXML(ArquivoRps envio)
        {
            //string xmlString = System.IO.File.ReadAllText(@"C:\i9ti\www\SISPEC\GerarNfseEnvio_Exemplo.xml");
            string xmlString = System.IO.File.ReadAllText(@ParametroDAL.GetValor("XmlPathEnviarLote"));

            
            xmlString = xmlString.Replace("{NumeroLote}", envio.Rps.NumeroLote.ToString());
            xmlString = xmlString.Replace("{CpfCnpj}", envio.Rps.CpfCnpj);
            xmlString = xmlString.Replace("{InscricaoMunicipal}", envio.Rps.InscricaoMunicipal);
            xmlString = xmlString.Replace("{QuantidadeRps}", envio.Rps.Quantidade);


            xmlString = xmlString.Replace("{Numero}", envio.Rps.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Numero);
            xmlString = xmlString.Replace("{Serie}", envio.Rps.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Serie);
            xmlString = xmlString.Replace("{Tipo}", envio.Rps.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Tipo);
            xmlString = xmlString.Replace("{Tipo}", envio.Rps.InfDeclaracaoPrestacaoServico.IdentificacaoRps.Tipo);
            xmlString = xmlString.Replace("{DataEmissao}", envio.Rps.InfDeclaracaoPrestacaoServico.DataEmissao);
            xmlString = xmlString.Replace("{Status}", envio.Rps.InfDeclaracaoPrestacaoServico.Status);
            xmlString = xmlString.Replace("{Competencia}", envio.Rps.InfDeclaracaoPrestacaoServico.Competencia);

            xmlString = xmlString.Replace("{ValorServicos}", envio.Rps.InfDeclaracaoPrestacaoServico.Servico.Valores.ValorServicos);
            xmlString = xmlString.Replace("{IssRetido}", envio.Rps.InfDeclaracaoPrestacaoServico.Servico.IssRetido);
            xmlString = xmlString.Replace("{ItemListaServico}", envio.Rps.InfDeclaracaoPrestacaoServico.Servico.ItemListaServico);
            xmlString = xmlString.Replace("{CodigoCnae}", envio.Rps.InfDeclaracaoPrestacaoServico.Servico.CodigoCnae);
            xmlString = xmlString.Replace("{CodigoTributacaoMunicipio}", envio.Rps.InfDeclaracaoPrestacaoServico.Servico.CodigoTributacaoMunicipio);
            xmlString = xmlString.Replace("{Discriminacao}", envio.Rps.InfDeclaracaoPrestacaoServico.Servico.Discriminacao);
            xmlString = xmlString.Replace("{CodigoMunicipio}", envio.Rps.InfDeclaracaoPrestacaoServico.Servico.CodigoMunicipio);
            xmlString = xmlString.Replace("{ExigibilidadeISS}", envio.Rps.InfDeclaracaoPrestacaoServico.Servico.ExigibilidadeISS);
            xmlString = xmlString.Replace("{MunicipioIncidencia}", envio.Rps.InfDeclaracaoPrestacaoServico.Servico.MunicipioIncidencia);

            xmlString = xmlString.Replace("{CpfCnpjPrestador}", envio.Rps.InfDeclaracaoPrestacaoServico.Prestador.CpfCnpj);
            xmlString = xmlString.Replace("{InscricaoMunicipalPrestador}", envio.Rps.InfDeclaracaoPrestacaoServico.Prestador.InscricaoMunicipal);

            xmlString = xmlString.Replace("{CpfCnpjTomador}", envio.Rps.InfDeclaracaoPrestacaoServico.Tomador.CpfCnpj);
            xmlString = xmlString.Replace("{RazaoSocialTomador}", envio.Rps.InfDeclaracaoPrestacaoServico.Tomador.RazaoSocial);
            xmlString = xmlString.Replace("{EnderecoTomador}", envio.Rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Logradouro);
            xmlString = xmlString.Replace("{NumeroTomador}", envio.Rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Numero);
            xmlString = xmlString.Replace("{BairroTomador}", envio.Rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Bairro);
            xmlString = xmlString.Replace("{CodigoMunicipioTomador}", envio.Rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.CodigoMunicipio);
            xmlString = xmlString.Replace("{UfTomador}", envio.Rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Uf);
            xmlString = xmlString.Replace("{CepTomador}", envio.Rps.InfDeclaracaoPrestacaoServico.Tomador.Endereco.Cep);
            xmlString = xmlString.Replace("{TelefoneTomador}", envio.Rps.InfDeclaracaoPrestacaoServico.Tomador.Contato.Telefone);
            xmlString = xmlString.Replace("{EmailTomador}", envio.Rps.InfDeclaracaoPrestacaoServico.Tomador.Contato.Email);

            xmlString = xmlString.Replace("{OptanteSimplesNacional}", envio.Rps.InfDeclaracaoPrestacaoServico.OptanteSimplesNacional);
            xmlString = xmlString.Replace("{IncentivoFiscal}", envio.Rps.InfDeclaracaoPrestacaoServico.IncentivoFiscal);

            return xmlString;
        }

    }
}
