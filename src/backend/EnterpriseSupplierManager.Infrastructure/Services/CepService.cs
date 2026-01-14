using System.Net.Http.Json;
using EnterpriseSupplierManager.Application.DTOs.Common;
using EnterpriseSupplierManager.Application.Interfaces;

namespace EnterpriseSupplierManager.Infrastructure.Services;

public class CepService : ICepService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CepService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<PostalCodeResponseDTO?> GetAddressByCepAsync(string cep)
    {
        var cleanCep = new string(cep.Where(char.IsDigit).ToArray());

        var client = _httpClientFactory.CreateClient("CepApi");

        try
        {
            var response = await client.GetFromJsonAsync<ViaCepResponse>($"{cleanCep}/json/");

            if (response.Erro != null)
                return null;

            return new PostalCodeResponseDTO
            {
                PostalCode = response.Cep,
                Street = response.Logradouro,
                Neighborhood = response.Bairro,
                City = response.Localidade,
                State = response.Uf
            };
        }
        catch (Exception)
        {
            // To do: Adicionar log
            return null;
        }
    }

    public class ViaCepResponse
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Localidade { get; set; }
        public string Bairro { get; set; }
        public string Uf { get; set; }
        public string? Erro { get; set; }
    }
}