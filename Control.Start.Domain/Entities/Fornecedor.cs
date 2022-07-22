using Control.Facilites.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Control.Facilites.Domain.Entities
{

    public class Fornecedor 
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string PontoVenda { get; set; }

    }
}
