using System;

namespace NFModel
{
    public class RegraTO
    {
		public Int32? Id { get; set; }
		public String Regra { get; set; }
		public String CpfCnpj_Prestador { get; set; }
		public String Discriminacao { get; set; }
		public String Expressoes { get; set; }
    }
}