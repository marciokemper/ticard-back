using Control.Facilites.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Control.Facilites.Validators
{
    public class DocumentoValidator : AbstractValidator<Documento>
    {
        public DocumentoValidator()
        {
            //RuleFor(x => x.Vencimento).NotNull().WithMessage("Campo Vencimento é obrigatório.");
        }
    }
}
