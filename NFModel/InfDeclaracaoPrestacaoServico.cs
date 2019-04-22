using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFModel
{
    public class InfDeclaracaoPrestacaoServico
    {
        public InfDeclaracaoPrestacaoServico()
        {
            IdentificacaoRps = new IdentificacaoRps();
            Servico = new Servico();
            Prestador = new Prestador();
            Tomador = new Tomador();

        }


        public IdentificacaoRps IdentificacaoRps { get; set; }
        public string Competencia { get; set; }
        public string DataEmissao { get; set; }
        public string Status { get; set; }
        public Servico Servico { get; set; }

        public Prestador Prestador { get; set; }
        public Tomador Tomador { get; set; }
        public string OptanteSimplesNacional { get; set; }
        public string IncentivoFiscal { get; set; }


    }

}
