using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NFDAL;
using NFModel;

namespace NFAppWeb.Controllers
{
    [Authorize]
    public class RpsController : Controller
    {
        public ActionResult Index() 
        {
            return View();
        }
            
        [HttpPost]
        public JsonResult Get()
        {
            int draw = Convert.ToInt32(Request.Form["draw"]);
            int start = Convert.ToInt32(Request.Form["start"]);
            int length = Convert.ToInt32(Request.Form["length"]);
            string textoFiltro = Request.Form["search[value]"];
            string sortColumn = Request.Form[string.Format("columns[{0}][name]", Request.Form["order[0][column]"])];
            string sortColumnDir = Request.Form["order[0][dir]"];

            int totRegistros = 0;
            int totRegistrosFiltro = 0;
            IList<RpsTO> dados = RpsDAL.Get(start, length, ref totRegistros, textoFiltro, ref totRegistrosFiltro, sortColumn, sortColumnDir);
            if (start > 0 && dados.Count == 0)
            {
                start -= length;
                dados = RpsDAL.Get(start, length, ref totRegistros, textoFiltro, ref totRegistrosFiltro, sortColumn, sortColumnDir);
                return Json(new { draw = draw, recordsFiltered = totRegistrosFiltro, recordsTotal = totRegistros, data = dados, voltarPagina = 'S' }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { draw = draw, recordsFiltered = totRegistrosFiltro, recordsTotal = totRegistros, data = dados }, JsonRequestBehavior.AllowGet);
        }
            
        [HttpPost]
        public JsonResult Insert(Int32? Id, Int32? IdLoteRps, Int32? Numero, String Serie, Int32? Tipo, String DataEmissao, Int32? Status, String Competencia, String ValorServico, String ISSRetido, String ItemListaServico, String CodigoCnae, String CodigoTributacaoMunicipio, String Discriminacao, String CodigoMunicipio, Int32? ExigibilidadeISS, String MunicipioIncidencia, String CpfCnpj_Prestador, String CpfCnpj_Tomador, Int32? OptanteSimplesNacional, Int32? IncentivoFiscal)
        {
            string auxMsgErro = string.Empty;
            string auxMsgSucesso = string.Empty;

            RpsTO obj = new RpsTO
            {
                Id = Id,
                IdLoteRps = IdLoteRps,
                Numero = Numero,
                Serie = Serie,
                Tipo = Tipo,
                DataEmissao = DataEmissao,
                Status = Status,
                Competencia = Competencia,
                ValorServico = ValorServico,
                ISSRetido = ISSRetido,
                ItemListaServico = ItemListaServico,
                CodigoCnae = CodigoCnae,
                CodigoTributacaoMunicipio = CodigoTributacaoMunicipio,
                Discriminacao = Discriminacao,
                CodigoMunicipio = CodigoMunicipio,
                ExigibilidadeISS = ExigibilidadeISS,
                MunicipioIncidencia = MunicipioIncidencia,
                CpfCnpj_Prestador = CpfCnpj_Prestador,
                CpfCnpj_Tomador = CpfCnpj_Tomador,
                OptanteSimplesNacional = OptanteSimplesNacional,
                IncentivoFiscal = IncentivoFiscal
            };

            if (RpsDAL.Insert(obj) == null)
            {
                auxMsgErro = "Falha ao tentar inserir o registro, favor tente novamente";
            }
            else
            {
                auxMsgSucesso = "Registro inserido com sucesso";
            }

            return Json(new { msgErro = auxMsgErro, msgSucesso = auxMsgSucesso });
        }
            
        [HttpPost]
        public JsonResult Update(Int32? IdLoteRps, Int32? Numero, String Serie, Int32? Tipo, String DataEmissao, Int32? Status, String Competencia, string ValorServico, string ISSRetido, String ItemListaServico, String CodigoCnae, String CodigoTributacaoMunicipio, String Discriminacao, String CodigoMunicipio, Int32? ExigibilidadeISS, String MunicipioIncidencia, String CpfCnpj_Prestador, String CpfCnpj_Tomador, Int32? OptanteSimplesNacional, Int32? IncentivoFiscal, Int32? Id)
        {
            string auxMsgErro = string.Empty;
            string auxMsgSucesso = string.Empty;

            RpsTO obj = new RpsTO
            {
                IdLoteRps = IdLoteRps,
                Numero = Numero,
                Serie = Serie,
                Tipo = Tipo,
                DataEmissao = DataEmissao,
                Status = Status,
                Competencia = Competencia,
                ValorServico = ValorServico,
                ISSRetido = ISSRetido,
                ItemListaServico = ItemListaServico,
                CodigoCnae = CodigoCnae,
                CodigoTributacaoMunicipio = CodigoTributacaoMunicipio,
                Discriminacao = Discriminacao,
                CodigoMunicipio = CodigoMunicipio,
                ExigibilidadeISS = ExigibilidadeISS,
                MunicipioIncidencia = MunicipioIncidencia,
                CpfCnpj_Prestador = CpfCnpj_Prestador,
                CpfCnpj_Tomador = CpfCnpj_Tomador,
                OptanteSimplesNacional = OptanteSimplesNacional,
                IncentivoFiscal = IncentivoFiscal,
                Id = Id
            };

            if (RpsDAL.Update(obj) == null)
            {
                auxMsgErro = "Falha ao tentar alterar o registro, favor tente novamente";
            }
            else
            {
                auxMsgSucesso = "Registro alterado com sucesso";
            }

            return Json(new { msgErro = auxMsgErro, msgSucesso = auxMsgSucesso });
        }
            
        [HttpPost]
        public JsonResult Delete(Int32? Id)
        {
            string auxMsgErro = string.Empty;
            string auxMsgSucesso = string.Empty;

            RpsTO obj = new RpsTO
            {
                Id = Id
            };

            if (RpsDAL.Delete(obj) == null)
            {
                auxMsgErro = "Falha ao tentar excluir o registro, favor tente novamente";
            }
            else
            {
                auxMsgSucesso = "Registro excluído com sucesso";
            }

            return Json(new { msgErro = auxMsgErro, msgSucesso = auxMsgSucesso });
        }
    }
}