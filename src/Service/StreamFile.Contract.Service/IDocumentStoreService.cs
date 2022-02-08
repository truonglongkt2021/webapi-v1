using StreamFile.Contract.Repository.Models.DocumentStore;
using StreamFile.Core.Models.Documents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamFile.Contract.Service
{
    public interface IDocumentStoreService : 
        Base.ICreateable<DocumentStoreEntity, string>
    {
        public Task<string> UploadDocument(AddDocumentModel request,
                                        CancellationToken cancellationToken = default);
        //public Task<string> CreateAsync(DocumentStoreEntity model, CancellationToken cancellationToken = default);
        public DocumentStoreEntity GetByDocumnetId(string documentId);

    }
}
