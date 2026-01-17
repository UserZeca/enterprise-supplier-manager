using EnterpriseSupplierManager.Application.DTOs.Common;
using EnterpriseSupplierManager.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace EnterpriseSupplierManager.Infrastructure.Services;

public class CepService : ICepService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CepService> _logger;

    public CepService(IHttpClientFactory httpClientFactory, ILogger<CepService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task EnsureValidCepAsync(string cep)
    {
        var address = await GetAddressByCepAsync(cep);
        if (address == null)
        {
            _logger.LogWarning("Validação falhou para o CEP: {Cep}", cep);
            throw new ArgumentException("O CEP informado é inválido ou não foi encontrado.");
        }
    }

    public async Task<CepResponseDTO?> GetAddressByCepAsync(string cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
        {
            _logger.LogWarning("Tentativa de consulta com CEP vazio ou nulo.");
            return null;
        }

        var cleanCep = new string(cep.Where(char.IsDigit).ToArray());
        var client = _httpClientFactory.CreateClient("CepApi");

        try
        {
            _logger.LogInformation("Iniciando consulta de CEP: {Cep}", cleanCep);

            var response = await client.GetFromJsonAsync<ViaCepResponse>($"{cleanCep}/json/");

            if (response == null || response.Erro == "true")
            {
                _logger.LogWarning("CEP {Cep} não encontrado na base de dados externa.", cleanCep);
                return null;
            }

            return new CepResponseDTO
            {
                Cep = response.Cep,
                Street = response.Logradouro,
                Neighborhood = response.Bairro,
                City = response.Localidade,
                State = response.Uf
            };
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "Erro de conectividade ao consultar o CEP {Cep}. Status: {Status}", cleanCep, httpEx.StatusCode);
            return null;
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "Erro na deserialização da resposta do CEP {Cep}. O formato do JSON mudou ou é inválido.", cleanCep);
            return null;
        }
        catch (TaskCanceledException timeoutEx)
        {
            _logger.LogError(timeoutEx, "Tempo limite (timeout) excedido ao consultar o CEP {Cep}.", cleanCep);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Ocorreu um erro inesperado ao processar o CEP {Cep}.", cleanCep);
            throw;
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