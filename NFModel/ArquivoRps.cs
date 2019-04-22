using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFModel
{
    public class ArquivoRps
    {

        public ArquivoRps()
        {
            Rps = new Rps();
        }

        public string NomeArquivo { get; set; }
        public string NomeArquivoREM { get; set; }
        public string NomeArquivoRET { get; set; }

        public Rps Rps { get; set; }
    }
}
