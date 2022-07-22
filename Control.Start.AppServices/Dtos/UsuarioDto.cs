using Control.Facilites.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Control.Facilites.Domain;
using Control.Facilites.Domain.Entities;

namespace Control.Facilites.AppServices.Dtos
{

    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string ConfirmaSenha { get; set; }
        public Boolean Ativo { get; set; }
        public EmpresaFornecedorDto EmpresaFornecedor { get; set; }

    }
}
