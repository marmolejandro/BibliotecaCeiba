using FluentValidation;
using PruebaIngresoBibliotecario.Domain.Entities;
using PruebaIngresoBibliotecario.Domain.Models;
using System;

namespace PruebaIngresoBibliotecario.Api.Validators
{
    public class LoanPreValidator : AbstractValidator<CreateLoanInDto>
    {
        public LoanPreValidator() 
        {
            RuleFor(r => r.Isbn).NotNull().WithMessage("{PropertyName} no puede ser null")
                                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio");

            RuleFor(r => r.IdentificacionUsuario).NotNull().WithMessage("{PropertyName} no puede ser null")
                                                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                                                .MaximumLength(10).WithMessage("{PropertyName} no puede superar los 10 caracteres")
                                                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("{propertyName} debe ser alfanumerico");


            RuleFor(p => (int)p.TipoUsuario).NotNull().WithMessage("{PropertyName} no puede ser null")
                                            .NotEmpty().WithMessage("{PropertyName} no puede ser vacio");
        }
    }
}
