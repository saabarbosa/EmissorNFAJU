using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFBusiness;

namespace TesteApp
{
    class Program
    {
        static void Main(string[] args)
        {
 
            NotaFiscal nf = new NotaFiscal();
            string msg = nf.Gerar();
            Console.WriteLine("Retorno do método: " + msg);
            Console.ReadKey();

        }
    }
}
