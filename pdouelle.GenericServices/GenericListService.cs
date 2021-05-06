using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using pdouelle.QueryStringHelper;

namespace pdouelle.GenericServices
{
    public class GenericListService<TEntity, TRequest> : IGenericListService<TEntity, TRequest>
    {
        protected readonly HttpClient HttpClient;

        public GenericListService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
        
        public virtual async Task<IEnumerable<TEntity>> GetListAsync(TRequest request, CancellationToken cancellationToken = new())
        {
            var queryString = request.GetQueryString();

            HttpResponseMessage response = await HttpClient.GetAsync(queryString, cancellationToken);

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