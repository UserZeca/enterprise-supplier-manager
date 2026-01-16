using EnterpriseSupplierManager.Application.Interfaces;
using EnterpriseSupplierManager.Application.Services;
using EnterpriseSupplierManager.Domain.Entities;
using EnterpriseSupplierManager.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EnterpriseSupplierManager.Tests.UnitTests.Application.Services
{
    public class SupplierServiceTests
    {
        private readonly Mock<ISupplierRepository> _supplierRepoMock = new();
        private readonly Mock<ICompanyRepository> _companyRepoMock = new();
        private readonly SupplierService _service;

        public SupplierServiceTests()
        {
            _service = new SupplierService(
                _supplierRepoMock.Object,
                _companyRepoMock.Object,
                Mock.Of<ILogger<SupplierService>>(),
                Mock.Of<ICepService>()
            );
        }

        [Fact]
        public async Task AssociateToCompany_Should_Block_Underage_In_Parana()
        {
            // Arrange: Fornecedor de 17 anos
            var supplier = new Supplier
            {
                Document = "12345678901",
                BirthDate = DateTime.Today.AddYears(-17)
            };
            var company = new Company { Uf = "PR" };

            _supplierRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(supplier);
            _companyRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(company);

            // Act
            var act = () => _service.AssociateToCompanyAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*não pode atender empresas no Paraná*");
        }
    }
}
