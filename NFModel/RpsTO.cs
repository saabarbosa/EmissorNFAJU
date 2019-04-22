using System;

namespace NFModel
{
    public class RpsTO
    {
		public Int32? Id { get; set; }
		public Int32? IdLoteRps { get; set; }
		public Int32? Numero { get; set; }
		public String Serie { get; set; }
		public Int32? Tipo { get; set; }
		public String DataEmissao { get; set; }
		public Int32? Status { get; set; }
		public String Competencia { get; set; }
		public String ValorServico { get; set; }
		public String ISSRetido { get; set; }
		public String ItemListaServico { get; set; }
		public String CodigoCnae { get; set; }
		public String CodigoTributacaoMunicipio { get; set; }
		public String Discriminacao { get; set; }
		public String CodigoMunicipio { get; set; }
		public Int32? ExigibilidadeISS { get; set; }
		public String MunicipioIncidencia { get; set; }
		public String CpfCnpj_Prestador { get; set; }
		public String CpfCnpj_Tomador { get; set; }
		public Int32? OptanteSimplesNacional { get; set; }
		public Int32? IncentivoFiscal { get; set; }
    }
}