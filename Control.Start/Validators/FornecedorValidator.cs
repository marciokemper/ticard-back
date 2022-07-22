using Control.Facilites.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Control.Facilites.Validators
{
    public class FornecedorValidator : AbstractValidator<Fornecedor>
    {
        public FornecedorValidator()
        {
            RuleFor(x => x.Codigo).NotNull().NotEmpty().WithMessage("Campo Código é obrigatório.");
            RuleFor(x => x.Nome).NotNull().NotEmpty().WithMessage("Campo Nome é obrigatório.");
           // RuleFor(x => x.Cnpj).NotNull().NotEmpty().WithMessage("Campo CNPJ é obrigatório.");
           // RuleFor(x => x.EnderecoLogradouro).NotNull().NotEmpty().WithMessage("Campo Endereço é obrigatório.");
            //RuleFor(x => x.EnderecoCep).NotNull().NotEmpty().WithMessage("Campo Cep é obrigatório.");
        }
    }
}


