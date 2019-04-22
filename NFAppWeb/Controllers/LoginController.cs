using System;
using System.Web.Mvc;
using NFDAL;
using NFModel;
using System.Web.Security;
using System.Globalization;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace NFAppWeb.Controllers
{

    public class LoginController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Validar(String Username, String Password)
        {
        
            string auxMsgErro = "";

            Usuario obj = new Usuario
            {
                Login = Username,
                Senha = Password
            };

            if (obj == null)
            {
                auxMsgErro = "Falha ao tentar efetuar o login, favor tente novamente";
            }
            else
            {
                if (UsuarioDAL.ValidarUsuario(Username.ToLower(), GerarHashMd5(Password)))
                {
                    obj = SetupFormsAuthTicket(obj.Login, false);

                }
                else
                {
                    auxMsgErro = "Usuário ou senha inválido(s).";
                }
                
            }
            //return View("AcessoNegado");
            return Json(new { msgErro = auxMsgErro });
        }

        private Usuario SetupFormsAuthTicket(string userName, bool persistanceFlag)
        {
            Usuario usuario = new Usuario();
            usuario.Login = userName;

            var userId = usuario.Id;
            string userData = userId.ToString(CultureInfo.InvariantCulture);
            var authTicket = new FormsAuthenticationTicket(1, //version
                                userName, // user name
                                DateTime.Now,             //creation
                                DateTime.Now.AddMinutes(30), //Expiration
                                persistanceFlag, //Persistent
                                userData);

            var encTicket = FormsAuthentication.Encrypt(authTicket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
            return usuario;
        }
        [AllowAnonymous]
        public ActionResult NotFound()
        {
            return HttpNotFound();
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
      
        public ActionResult AcessoNegado()
        {
            return View();
        }

        public static string GerarHashMd5(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i=0;i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}