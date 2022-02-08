using StreamFile.Core.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamFile.Contract.Repository.Models.DocumentStore
{
    [Table("DocumentStore")]
    public class DocumentStoreEntity : Entity
    {
        public DocumentStoreEntity()
        {
            ContentType = ApplicationConstant.FileContentType;
        }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long? FileSize { get; set; }
        public string FilePath { get; set; }
        public string DocumentId { get; set; }


    }
}
