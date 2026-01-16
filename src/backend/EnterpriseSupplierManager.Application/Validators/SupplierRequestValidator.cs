using EnterpriseSupplierManager.Application.DTOs.Supplier;
using EnterpriseSupplierManager.Domain.Validation;
using FluentValidation;

namespace EnterpriseSupplierManager.Application.Validators
{
    public class SupplierRequestValidator : AbstractValidator<SupplierRequestDTO>
    {
        public SupplierRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                // Valida o formato 'usuario@dominio.extensao'
                // Garante: (1) Início sem @ ou espaços, (2) Uma arroba, (3) Nome do servidor, 
                // (4) Ponto obrigatório e (5) Sufixo do domínio no final.
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            RuleFor(x => x.Cep)
                .NotEmpty().WithMessage("O CEP é obrigatório.")
                // Garante que existem 8 números
                .Matches(@"^\d{8}$").WithMessage("O CEP deve conter exatamente 8 números.");

            RuleFor(x => x.Document)
                .NotEmpty().WithMessage("O documento (CPF/CNPJ) é obrigatório.")
                .Custom((document, context) =>
                {
                    string digits = new string(document.Where(char.IsDigit).ToArray());

                    if (digits.Length == 11)
                    {
                        if (!DocumentValidator.IsValidCpf(digits))
                            context.AddFailure("O CPF informado é inválido.");
                    }
                    else if (digits.Length == 14)
                    {
                        if (!DocumentValidator.IsValidCnpj(digits))
                            context.AddFailure("O CNPJ informado é inválido.");
                    }
                    else
                    {
                        context.AddFailure("O documento deve ter 11 (CPF) ou 14 (CNPJ) dígitos.");
                    }
                });

            When(x => IsPhysicalPerson(x.Document), () =>
            {
                RuleFor(x => x.Rg)
                    .NotEmpty().WithMessage("O RG é obrigatório para pessoa física.")
                    // Valida RG: Permite de 5 a 14 dígitos iniciais e um dígito verificador opcional (número ou 'X').
                    .Matches(@"^[0-9]{5,14}[0-9xX]?$").WithMessage("O formato do RG é inválido.");

                RuleFor(x => x.BirthDate)
                    .NotEmpty().WithMessage("A data de nascimento é obrigatória para pessoa física.")
                    .LessThan(DateTime.Now).WithMessage("A data de nascimento não pode ser no futuro.");
            });
        }

        private bool IsPhysicalPerson(string document)
        {
            if (string.IsNullOrWhiteSpace(document)) return false;
            string digits = new string(document.Where(char.IsDigit).ToArray());
            return digits.Length == 11;
        }
    }
}
