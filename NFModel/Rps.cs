using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFModel
{
    public class Rps
    {
        public Rps()
        {
            InfDeclaracaoPrestacaoServico = new InfDeclaracaoPrestacaoServico();
        }
        public int NumeroLote { get; set; }
        public string CpfCnpj { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string Quantidade { get; set; }
        public InfDeclaracaoPrestacaoServico InfDeclaracaoPrestacaoServico { get; set; }


    }
}
