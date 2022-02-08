using System.Threading;
using System.Threading.Tasks;

namespace StreamFile.Contract.Service.Base
{
    public interface IGetable<T, in TKey> where T : class, new()
    {
        Task<T> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    }
}
