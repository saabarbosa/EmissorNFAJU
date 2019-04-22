using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NFDAL;
using NFModel;


namespace NFAppWeb.Controllers
{
    [Authorize]
    public class DadosEnvioController : Controller
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
            IList<DadosEnvioTO> dados = DadosEnvioDAL.Get(start, length, ref totRegistros, textoFiltro, ref totRegistrosFiltro, sortColumn, sortColumnDir);
            if (start > 0 && dados.Count == 0)
            {
                start -= length;
                dados = DadosEnvioDAL.Get(start, length, ref totRegistros, textoFiltro, ref totRegistrosFiltro, sortColumn, sortColumnDir);
                return Json(new { draw = draw, recordsFiltered = totRegistrosFiltro, recordsTotal = totRegistros, data = dados, voltarPagina = 'S' }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { draw = draw, recordsFiltered = totRegistrosFiltro, recordsTotal = totRegistros, data = dados }, JsonRequestBehavior.AllowGet);
        }
            
        [HttpPost]
        public JsonResult Insert(String NumeroLoteRps, string Data, String ArquivoImportacao, String ConteudoArquivoImportacao, String ArquivoRemessa, String XMLRemessa, String ArquivoRetorno, String XMLRetorno)
        {
            string auxMsgErro = string.Empty;
            string auxMsgSucesso = string.Empty;

            DadosEnvioTO obj = new DadosEnvioTO
            {
                NumeroLoteRps = NumeroLoteRps,
                Data = Data,
                ArquivoImportacao = ArquivoImportacao,
                ConteudoArquivoImportacao = ConteudoArquivoImportacao,
                ArquivoRemessa = ArquivoRemessa,
                XMLRemessa = XMLRemessa,
                ArquivoRetorno = ArquivoRetorno,
                XMLRetorno = XMLRetorno
            };

            if (DadosEnvioDAL.Insert(obj) == null)
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
        public JsonResult Update(String NumeroLoteRps, string Data, String ArquivoImportacao, String ConteudoArquivoImportacao, String ArquivoRemessa, String XMLRemessa, String ArquivoRetorno, String XMLRetorno, Int32 Id)
        {
            string auxMsgErro = string.Empty;
            string auxMsgSucesso = string.Empty;

            DadosEnvioTO obj = new DadosEnvioTO
            {
                NumeroLoteRps = NumeroLoteRps,
                Data = Data,
                ArquivoImportacao = ArquivoImportacao,
                ConteudoArquivoImportacao = ConteudoArquivoImportacao,
                ArquivoRemessa = ArquivoRemessa,
                XMLRemessa = XMLRemessa,
                ArquivoRetorno = ArquivoRetorno,
                XMLRetorno = XMLRetorno,
                Id = Id
            };

            if (DadosEnvioDAL.Update(obj) == null)
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
        public JsonResult Delete(Int32 Id)
        {
            string auxMsgErro = string.Empty;
            string auxMsgSucesso = string.Empty;

            DadosEnvioTO obj = new DadosEnvioTO
            {
                Id = Id
            };

            if (DadosEnvioDAL.Delete(obj) == null)
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