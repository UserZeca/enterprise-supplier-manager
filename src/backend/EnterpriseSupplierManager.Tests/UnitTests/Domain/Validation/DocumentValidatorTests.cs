using EnterpriseSupplierManager.Domain.Validation;
using Xunit;
using FluentAssertions;

namespace EnterpriseSupplierManager.Tests.UnitTests.Domain.Validation
{
    public class DocumentValidatorTests
    {
        // TESTES POSITIVOS
        [Theory]
        [InlineData("11222333000181")] // CNPJ Matemático real
        [InlineData("11.222.333/0001-81")] // Com máscara
        public void IsValidCnpj_ShouldReturnTrue_WhenCnpjIsCorrect(string cnpj)
        {
            DocumentValidator.IsValidCnpj(cnpj).Should().BeTrue();
        }

        [Theory]
        [InlineData("12345678909")] // CPF Matemático real
        public void IsValidCpf_ShouldReturnTrue_WhenCpfIsCorrect(string cpf)
        {
            DocumentValidator.IsValidCpf(cpf).Should().BeTrue();
        }

        // TESTES NEGATIVOS
        [Theory]
        [InlineData("45101655000100")] // O dado que falhou antes (inválido)
        [InlineData("00000000000000")] // Números repetidos
        [InlineData("12345678901234")] // Sequência inválida
        public void IsValidCnpj_ShouldReturnFalse_WhenCnpjIsInvalid(string cnpj)
        {

            DocumentValidator.IsValidCnpj(cnpj).Should().BeFalse();
        }

        [Theory]
        [InlineData("11111111111")] // CPF repetido
        [InlineData("12345678900")] // CPF inválido
        public void IsValidCpf_ShouldReturnFalse_WhenCpfIsInvalid(string cpf)
        {
            DocumentValidator.IsValidCpf(cpf).Should().BeFalse();
        }
    }
}
