using System.Threading;
using System.Threading.Tasks;

namespace pdouelle.GenericServices
{
    public interface IGenericByIdService<TEntity, TRequest>
    {
        public Task<TEntity> GetByIdAsync(TRequest request, CancellationToken cancellationToken = new());
    }
}