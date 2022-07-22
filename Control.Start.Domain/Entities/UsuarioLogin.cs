using Control.Facilites.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Control.Facilites.Domain.Entities
{

    public class UsuarioLogin
    {
        public string Login { get; set; }
        public string Senha { get; set; }

    }
}
