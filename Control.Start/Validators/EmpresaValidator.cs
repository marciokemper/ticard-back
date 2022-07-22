using Control.Facilites.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Control.Facilites.Validators
{
    public class EmpresaValidator : AbstractValidator<Empresa>
    {
        public EmpresaValidator()
        {
            RuleFor(x => x.Nome).NotNull().NotEmpty().WithMessage("Campo Nome é obrigatório.");
        }
    }
}


