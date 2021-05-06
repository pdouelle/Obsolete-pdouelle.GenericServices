using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using pdouelle.QueryStringHelper;

namespace pdouelle.Edenred.CMS.API.Service
{
    public class GenericListService<TEntity, TRequest> : IGenericListService<TEntity, TRequest>
    {
        private readonly HttpClient _httpClient;

        public GenericListService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<IEnumerable<TEntity>> GetListAsync(TRequest request, CancellationToken cancellationToken = new())
        {
            var queryString = request.GetQueryString();

            HttpResponseMessage response = await _httpClient.GetAsync(queryString, cancellationToken);

            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<IEnumerable<TEntity>>(
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            throw new Exception((int) response.StatusCode + "-" + response.StatusCode);
        }
    }
}