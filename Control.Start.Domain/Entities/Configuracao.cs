using Control.Facilites.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Control.Facilites.Domain.Entities
{

    public class Configuracao
    {
        public int Id { get; set; }
        public string CaminhoNexxera { get; set; }

    }
}
