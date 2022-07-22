using Control.Facilites.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Control.Facilites.Domain.Entities
{

    public class Documento 
    {
        public int Id { get; set; }
        public EmpresaFornecedor EmpresaFornecedor { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public FluxoDocumento SituacaoDocumento { get; set; }
        public string CaminhoAnexo { get; set; }      


    }
}
