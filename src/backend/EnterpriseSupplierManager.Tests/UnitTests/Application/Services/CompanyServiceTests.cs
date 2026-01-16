using EnterpriseSupplierManager.Application.DTOs.Company;
using EnterpriseSupplierManager.Application.Interfaces;
using EnterpriseSupplierManager.Application.Services;
using EnterpriseSupplierManager.Domain.Entities;
using EnterpriseSupplierManager.Domain.Exceptions;
using EnterpriseSupplierManager.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EnterpriseSupplierManager.Tests.UnitTests.Application.Services
{
    public class CompanyServiceTests
    {
        private readonly Mock<ICompanyRepository> _companyRepoMock = new();
        private readonly Mock<ICepService> _cepServiceMock = new();
        private readonly CompanyService _service;

        public CompanyServiceTests()
        {
            _service = new CompanyService(
                _companyRepoMock.Object,
                _cepServiceMock.Object,
                Mock.Of<ILogger<CompanyService>>()
            );
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowDuplicateEntryException_WhenCnpjAlreadyExists()
        {
            // Arrange
            var request = new CompanyRequestDTO { Cnpj = "11222333000181" };
            var existingCompany = new Company { Cnpj = "11222333000181" };

            _companyRepoMock.Setup(x => x.GetByCnpjAsync(request.Cnpj))
                             .ReturnsAsync(existingCompany);

            // Act
            var act = () => _service.CreateAsync(request);

            // Assert
            await act.Should().ThrowAsync<DuplicateEntryException>()
                .WithMessage("*Já existe uma empresa cadastrada com este CNPJ*");
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallCepService_OnlyWhenCepIsChanged()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var request = new CompanyRequestDTO { Cep = "31270901" };
            var existingCompany = new Company { Cep = "30110000" };

            // --- REFLECTION ---
            var idProperty = typeof(Company).GetProperty("Id");

            // Setamos o valor no objeto 'existingCompany'
            idProperty?.SetValue(existingCompany, companyId);
            // -------------------------

            _companyRepoMock.Setup(x => x.GetByIdAsync(companyId)).ReturnsAsync(existingCompany);

            // Act
            await _service.UpdateAsync(companyId, request);

            // Assert
            _cepServiceMock.Verify(x => x.EnsureValidCepAsync(request.Cep), Times.Once);
        }
    }
}
