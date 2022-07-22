using Control.Facilites.Domain;
using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.AppServices.Dtos
{
    public class EmpresaFornecedorDto
    {
        public int Id { get; set; }
        public EmpresaDto Empresa { get; set; }
        public FornecedorDto Fornecedor { get; set; }
    }

}
