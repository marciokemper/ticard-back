using Control.Facilites.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Control.Facilites.Domain;

namespace Control.Facilites.AppServices.Dtos
{

    public class FornecedorDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string PontoVenda { get; set; }
    }
}
