using EnterpriseSupplierManager.Application.DTOs.Company;
using EnterpriseSupplierManager.Domain.Validation;
using FluentValidation;

namespace EnterpriseSupplierManager.Application.Validators
{
    public class CompanyRequestValidator : AbstractValidator<CompanyRequestDTO>
    {
        public CompanyRequestValidator()
        {
            RuleFor(x => x.TradeName)
                .NotEmpty().WithMessage("O nome da empresa é obrigatório.")
                .Length(3, 150).WithMessage("O nome deve ter entre 3 e 150 caracteres.");

            RuleFor(x => x.Cnpj)
                .NotEmpty().WithMessage("O CNPJ é obrigatório.")
                .Must(DocumentValidator.IsValidCnpj).WithMessage("O CNPJ informado é inválido.")
                .Matches(@"^\d{14}$").WithMessage("O CNPJ deve conter apenas 14 números.");

            RuleFor(x => x.Uf)
                .NotEmpty().WithMessage("A UF é obrigatória.")
                .Length(2).WithMessage("A UF deve ter exatamente 2 caracteres.");

            RuleFor(x => x.Cep)
                .NotEmpty().WithMessage("O CEP é obrigatório.")
                .Matches(@"^\d{8}$").WithMessage("O CEP deve conter 8 números.");
        }
    }
}
