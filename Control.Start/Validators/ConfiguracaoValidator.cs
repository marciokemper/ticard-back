using Control.Facilites.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Control.Facilites.Validators
{
    public class ConfiguracaoValidator : AbstractValidator<Configuracao>
    {
        public ConfiguracaoValidator()
        {
            //RuleFor(x => x.CaminhoDestino).NotNull().NotEmpty().WithMessage("Campo Caminho é obrigatório.");
            //RuleFor(x => x.PeriodicidadeExecucao).NotNull().NotEqual(0).WithMessage("Campo Periodicidade é obrigatório.");
            //RuleFor(x => x.VisibilidadeExecucao).NotNull().NotEmpty().WithMessage("Campo Visibilidade é obrigatório.");
        }
    }
}
