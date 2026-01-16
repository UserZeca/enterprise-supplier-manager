using EnterpriseSupplierManager.Application.DTOs.Supplier;
using EnterpriseSupplierManager.Application.Validators;
using FluentValidation.TestHelper;


namespace EnterpriseSupplierManager.Tests.UnitTests.Application.Validators
{
    public class SupplierRequestValidatorTests
    {
        private readonly SupplierRequestValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Email_Has_No_Domain()
        {
            var model = new SupplierRequestDTO { Email = "teste@c" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("12345")]    // Curto
        [InlineData("12345678X")] // Com X
        public void Should_Accept_Valid_RG_Formats(string rg)
        {
            var model = new SupplierRequestDTO
            {
                Document = "12345678901",
                Rg = rg
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Rg);
        }
    }
}
