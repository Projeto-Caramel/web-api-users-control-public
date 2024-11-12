using Caramel.Pattern.Services.Domain.Entities.Models.Adopters;
using Caramel.Pattern.Services.Domain.Enums.Adopters;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Validators
{
    [ExcludeFromCodeCoverage]
    public class AdopterValidator : AbstractValidator<Adopter>
    {
        public AdopterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O Nome é obrigatório.");
            RuleFor(x => x.Birthday)
                .NotNull().WithMessage("Idade Inválida.");

            RuleFor(x => x.ResidencyType)
                .IsInEnum().NotEqual(ResidencyType.Blank).WithMessage("Opção de Tipo de Residência Inválida.");
            RuleFor(x => x.Lifestyle)
                .IsInEnum().NotEqual(Lifestyle.Blank).WithMessage("Opção de Estilo de Vida Inválida.");
            RuleFor(x => x.PetExperience)
                .IsInEnum().NotEqual(PetExperience.Blank).WithMessage("Opção de Experiência com Pet Inválida.");
            RuleFor(x => x.HasChildren)
                .IsInEnum().NotEqual(HasChildren.Blank).WithMessage("Opção de Crianças em Casa Inválida.");
            RuleFor(x => x.FinancialSituation)
                .IsInEnum().NotEqual(FinancialSituation.Blank).WithMessage("Opção de Situação Financeira Inválida.");
            RuleFor(x => x.FreeTime)
                .IsInEnum().NotEqual(FreeTime.Blank).WithMessage("Opção de Tempo Livre Inválida.");
        }
    }
}
