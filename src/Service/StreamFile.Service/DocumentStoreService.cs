using AutoMapper;
using Invedia.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StreamFile.Contract.Repository.Interface;
using StreamFile.Contract.Repository.Models.DocumentStore;
using StreamFile.Contract.Service;
using StreamFile.Core.Models.Documents;
using System;
using System.Threading;
using System.Threading.Tasks;

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

        public Task<string> UploadDocument(AddDocumentModel request,
                                                CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<DocumentStoreEntity>(request);
            CreateAsync(entity, cancellationToken);
            return Task.FromResult(entity.Id);
        }

        public Task<string> CreateAsync(DocumentStoreEntity model, CancellationToken cancellationToken = default)
        {
            //set default value

            _documentStoreRepository.Add(model);
            UnitOfWork.SaveChanges();
            return Task.FromResult(model.Id);
        }

        public DocumentStoreEntity GetByDocumnetId(string documentId)
        {
           //var  Connection = new HubConnectionBuilder()
           // .WithUrl("https://telesys.amazingtech.vn/signalr")
           // .Build();
            var result = _documentStoreRepository.GetSingle(w => w.DocumentId == documentId);
            return result;
        }
    }
}
