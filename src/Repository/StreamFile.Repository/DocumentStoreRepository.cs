using Invedia.Data.EF.Interfaces.DbContext;
using Invedia.DI.Attributes;
using Serilog;
using StreamFile.Contract.Repository.Interface;
using StreamFile.Contract.Repository.Models.DocumentStore;
using StreamFile.Repository.Infrastructure;

namespace StreamFile.Repository
{
    [ScopedDependency(ServiceType = typeof(IDocumentStoreRepository))]
    public class DocumentStoreRepository : Repository<DocumentStoreEntity>, IDocumentStoreRepository
    {
        private readonly ILogger _logger;
        public DocumentStoreRepository(IDbContext dbContext) : base(dbContext)
        {
            _logger = Log.Logger;
        }
    }
}
