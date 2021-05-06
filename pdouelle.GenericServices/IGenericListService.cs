using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace pdouelle.Edenred.CMS.API.Service
{
    public interface IGenericListService<TEntity, TRequest>
    {
        public Task<IEnumerable<TEntity>> GetListAsync(TRequest request, CancellationToken cancellationToken = new());
    }
}