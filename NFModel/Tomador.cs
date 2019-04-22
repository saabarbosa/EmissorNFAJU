using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFModel
{
    public class Tomador
    {
        public Tomador()
        {
            Endereco = new Endereco();
            Contato = new Contato();
        }

        public string CpfCnpj { get; set; }
        public string RazaoSocial { get; set; }
        public Endereco Endereco { get; set; }
        public Contato Contato { get; set; }
    }
}
