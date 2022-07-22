using Control.Facilites.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Control.Facilites.Validators
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(x => x.Nome).NotNull().NotEmpty().WithMessage("Campo Nome é obrigatório.");
            RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Campo Email é obrigatório.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email informado formatado errado.");
            RuleFor(x => x.Senha).NotNull().NotEmpty().WithMessage("Campo Senha é obrigatório.");
            RuleFor(x => x.ConfirmaSenha).NotNull().NotEmpty().WithMessage("Campo Confirmar senha é obrigatório.");
            RuleFor(x => x.Senha).Equal(x => x.ConfirmaSenha).When(x => !String.IsNullOrWhiteSpace(x.Senha)).WithMessage("Senhas informadas diferentes.");

        }
    }
}
