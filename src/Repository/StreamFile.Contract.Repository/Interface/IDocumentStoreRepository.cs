using StreamFile.Contract.Repository.Infrastructure;
using StreamFile.Contract.Repository.Models.DocumentStore;

namespace StreamFile.Contract.Repository.Interface
{
    public interface IDocumentStoreRepository : IRepository<DocumentStoresEntity>
    {
        bool Edit(DocumentStoresEntity entity);
        DocumentStoresEntity GetDocByDocumnetId(string documentId);
    }
}
