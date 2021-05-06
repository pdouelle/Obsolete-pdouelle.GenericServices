using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using pdouelle.Entity;
using pdouelle.QueryStringHelper;

namespace pdouelle.GenericServices
{
    public class GenericByIdService<TEntity, TRequest> : IGenericByIdService<TEntity, TRequest> where TRequest : IEntity
    {
        protected readonly HttpClient HttpClient;

        public GenericByIdService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public virtual async Task<TEntity> GetByIdAsync(TRequest request, CancellationToken cancellationToken = new())
        {
            var queryString = request.GetQueryString();

            HttpResponseMessage response =
                await HttpClient.GetAsync($"{request.Id}{queryString}", cancellationToken);

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