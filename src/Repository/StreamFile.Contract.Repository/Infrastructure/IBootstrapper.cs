using System.Threading;
using System.Threading.Tasks;

namespace StreamFile.Contract.Repository.Infrastructure
{
    public interface IBootstrapper
    {
        Task InitialAsync(CancellationToken cancellationToken = default);

        Task RebuildAsync(CancellationToken cancellationToken = default);
    }
}