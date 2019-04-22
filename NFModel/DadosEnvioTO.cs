using System;

namespace NFModel
{
    public class DadosEnvioTO
    {
		public Int32 Id { get; set; }
		public String NumeroLoteRps { get; set; }
		public String Data { get; set; }
        public String ArquivoImportacao { get; set; }
		public String ConteudoArquivoImportacao { get; set; }
		public String ArquivoRemessa { get; set; }
		public String XMLRemessa { get; set; }
		public String ArquivoRetorno { get; set; }
		public String XMLRetorno { get; set; }
    }
}