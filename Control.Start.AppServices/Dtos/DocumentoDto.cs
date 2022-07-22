using Control.Facilites.Domain.Enum;
using Control.Facilites.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Control.Facilites.Domain.Entities;

namespace Control.Facilites.AppServices.Dtos
{

    public class DocumentoDto
    {
        public int Id { get; set; }
        public EmpresaFornecedorDto EmpresaFornecedor { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public FluxoDocumento SituacaoDocumento { get; set; }
        public string CaminhoAnexo { get; set; }
    }

}
