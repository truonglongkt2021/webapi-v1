using StreamFile.Contract.Repository.Models.DocumentStore;
using StreamFile.Core.Models.Documents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamFile.Contract.Service
{
    public interface IDocumentStoreService
    {
        void UploadDocument(AddDocumentModel request);
        public DocumentStoresEntity GetByDocumnetId(string documentId);
        DocInfoModel DownloadFile(string documentId);
        public  void TestSignalR();
    }
}
