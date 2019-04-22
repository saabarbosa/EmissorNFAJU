using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFModel
{
    public class Servico
    {
        public Servico()
        {
            Valores = new Valores();
        }

        public Valores Valores { get; set; }
        public string IssRetido { get; set; }
        public string ItemListaServico { get; set; }
        public string CodigoCnae { get; set; }
        public string CodigoTributacaoMunicipio { get; set; }
        public string Discriminacao { get; set; }
        public string CodigoMunicipio { get; set; }
        public string ExigibilidadeISS { get; set; }
        public string MunicipioIncidencia { get; set; }
    }
}
