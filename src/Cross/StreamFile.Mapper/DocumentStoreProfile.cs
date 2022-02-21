using AutoMapper;
using StreamFile.Contract.Repository.Models.DocumentStore;
using StreamFile.Core.Models.Documents;

namespace StreamFile.Mapper
{
    public class DocumentsStoreProfile : Profile
    {
        public DocumentsStoreProfile()
        {
            CreateMap<AddDocumentModel, DocumentStoresEntity>().ReverseMap();
        }
    }
}
