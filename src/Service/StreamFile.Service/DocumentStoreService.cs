using AutoMapper;
using Invedia.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StreamFile.Contract.Repository.Interface;
using StreamFile.Contract.Repository.Models.DocumentStore;
using StreamFile.Contract.Service;
using StreamFile.Core.Exceptions;
using StreamFile.Core.Models.Documents;
using System;
using System.IO;

namespace StreamFile.Service
{
    [ScopedDependency(ServiceType = typeof(IDocumentStoreService))]
    public class DocumentStoreService : Base.Service, IDocumentStoreService
    {
        private readonly IDocumentStoreRepository _documentStoreRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public DocumentStoreService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _documentStoreRepository = serviceProvider.GetRequiredService<IDocumentStoreRepository>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _logger = Log.Logger;
        }

        public void UploadDocument(AddDocumentModel request)
        {
            var entity = _mapper.Map<DocumentStoreEntity>(request);
            _documentStoreRepository.Insert(entity);
        }

        public DocumentStoreEntity GetByDocumnetId(string documentId)
        {
            var result = _documentStoreRepository.GetDocByDocumnetId(documentId);
            return result;
        }

        public DocInfoModel DownloadFile(string documentId)
        {
            var document = _documentStoreRepository.GetDocByDocumnetId(documentId);
            if (document == null) throw new AppException($"Document with ID: {documentId} is not found");

            var files = Path.Combine(document.FilePath);
            if (!System.IO.File.Exists(files)) throw new AppException($"Can not found find!");

            return new DocInfoModel { FileName = document.FileName, FilePath = files};
        }


        public async void TestSignalR()
        {
            //using (var hubConnection = new HubConnection("https://telesys.amazingtech.vn/signalr"))
            //{
            //    IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("ChatHub");
            //    //stockTickerHubProxy.On<Stock>("UpdateStockPrice", stock => Console.WriteLine("Stock update for {0} new price {1}", stock.Symbol, stock.Price));
            //    await hubConnection.Start();
            //    await stockTickerHubProxy.Invoke("Send", "moa", "cai quan que");
            //}
        }
    }
}
