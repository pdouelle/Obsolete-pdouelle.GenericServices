using System.Threading;
using System.Threading.Tasks;

namespace pdouelle.Edenred.CMS.API.Service
{
    public interface IGenericByIdService<TEntity, TRequest>
    {
        public Task<TEntity> GetByIdAsync(TRequest request, CancellationToken cancellationToken = new());
    }
}