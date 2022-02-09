using StreamFile.Contract.Repository.Infrastructure;
using StreamFile.Contract.Repository.Models.DocumentStore;

namespace StreamFile.Contract.Repository.Interface
{
    public interface IDocumentStoreRepository : IRepository<DocumentStoreEntity>
    {
        bool Edit(DocumentStoreEntity entity);
        DocumentStoreEntity GetDocByDocumnetId(string documentId);
    }
}
