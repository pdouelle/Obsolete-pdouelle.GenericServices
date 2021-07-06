using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using pdouelle.Entity;
using pdouelle.QueryStringHelper;

namespace pdouelle.GenericServices
{
    public class GenericByIdService<TEntity, TRequest> : IGenericByIdService<TEntity, TRequest> 
        where TRequest : IEntity 
        where TEntity : new()
    {
        protected readonly HttpClient HttpClient;
        private readonly ILogger<GenericByIdService<TEntity, TRequest>> _logger;

        public GenericByIdService(HttpClient httpClient, ILogger<GenericByIdService<TEntity, TRequest>> logger)
        {
            HttpClient = httpClient;
            _logger = logger;
        }

        public virtual async Task<TEntity> GetByIdAsync(TRequest request, CancellationToken cancellationToken = new())
        {
            try
            {
                var queryString = request.GetQueryString();

                HttpResponseMessage response = await HttpClient.GetAsync($"{request.Id}{queryString}", cancellationToken);

                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var jsonOptions = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
                    var jsonResponse = JsonSerializer.Deserialize<TEntity>(content, jsonOptions);
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