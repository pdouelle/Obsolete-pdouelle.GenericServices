using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using pdouelle.Entity;
using pdouelle.QueryStringHelper;

namespace pdouelle.Edenred.CMS.API.Service
{
    public class GenericByIdService<TEntity, TRequest> : IGenericByIdService<TEntity, TRequest> where TRequest : IEntity

    {
    private readonly HttpClient _httpClient;

    public GenericByIdService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TEntity> GetByIdAsync(TRequest request, CancellationToken cancellationToken = new())
    {
        var queryString = request.GetQueryString();

        HttpResponseMessage response =
            await _httpClient.GetAsync($"{request.Id}{queryString}", cancellationToken);

        if (response.IsSuccessStatusCode)
            return JsonSerializer.Deserialize<TEntity>(
                await response.Content.ReadAsStringAsync(cancellationToken),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        throw new Exception((int) response.StatusCode + "-" + response.StatusCode);
    }
    }
}