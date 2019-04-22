using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFAppWeb
{
    public class LogOnModel
    {
        public int Id { get; set; }

        public int Celular { get; set; }

        public int Email { get; set; }

        public int TipoUsuario { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
