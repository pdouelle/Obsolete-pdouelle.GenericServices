using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using pdouelle.QueryStringHelper;

namespace pdouelle.GenericServices
{
    public class GenericListService<TEntity, TRequest> : IGenericListService<TEntity, TRequest>
    {
        protected readonly HttpClient HttpClient;
        private readonly ILogger<GenericListService<TEntity, TRequest>> _logger;

        public GenericListService(HttpClient httpClient, ILogger<GenericListService<TEntity, TRequest>> logger)
        {
            HttpClient = httpClient;
            _logger = logger;
        }

        public virtual async Task<IEnumerable<TEntity>> GetListAsync(TRequest request,
            CancellationToken cancellationToken = new())
        {
            try
            {
                var queryString = request.GetQueryString();

                HttpResponseMessage response = await HttpClient.GetAsync(queryString, cancellationToken);

                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var jsonOptions = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
                    var jsonResponse = JsonSerializer.Deserialize<IEnumerable<TEntity>>(content, jsonOptions);
                    return jsonResponse;
                }

                var error = $"Http Response Error: {response} Response Body: {content}";
                _logger.LogError(error);
                throw new Exception(error);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}