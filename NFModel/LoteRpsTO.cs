using System;

namespace NFModel
{
    public class LoteRpsTO
    {
		public Int32? Id { get; set; }
		public Int32? NumeroLote { get; set; }
		public String CpfCnpj { get; set; }
		public String InscricaoMunicipal { get; set; }
		public Int32? Quantidade { get; set; }
    }
}