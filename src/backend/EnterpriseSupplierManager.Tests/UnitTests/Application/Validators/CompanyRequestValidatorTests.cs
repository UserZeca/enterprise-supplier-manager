using EnterpriseSupplierManager.Application.DTOs.Company;
using EnterpriseSupplierManager.Application.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace EnterpriseSupplierManager.Tests.UnitTests.Application.Validators
{
    public class CompanyRequestValidatorTests
    {
        private readonly CompanyRequestValidator _validator = new();

        [Theory]
        [InlineData("")]          // Vazio
        [InlineData("Ab")]        // Curto demais
        public void TradeName_ShouldHaveError_WhenInvalid(string name)
        {
            var model = new CompanyRequestDTO { TradeName = name };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.TradeName);
        }

        [Theory]
        [InlineData("1234567")]   // Curto (7 dígitos)
        [InlineData("123456789")] // Longo (9 dígitos)
        [InlineData("12345-678")] // Com máscara
        public void Cep_ShouldHaveError_WhenFormatIsInvalid(string cep)
        {
            var model = new CompanyRequestDTO { Cep = cep };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Cep);
        }

        [Fact]
        public void Uf_ShouldHaveError_WhenNotExactlyTwoCharacters()
        {
            var model = new CompanyRequestDTO { Uf = "MG1" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Uf);
        }
    }
}
