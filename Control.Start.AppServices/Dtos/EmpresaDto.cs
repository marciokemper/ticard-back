using Control.Facilites.Domain;
using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.AppServices.Dtos
{
    public class EmpresaDto 
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Logo { get; set; }
        public EmpresaDto EmpresaPai { get; set; }
        public string EmpresaPaiId { get; set; }
        public string Cnpj { get; set; }
        public string PontoVenda { get; set; }
    }

}
