namespace StreamFile.Core.Models.Documents
{
    public class AddDocumentModel
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }
        public string DocumentId { get; set; }

    }
}
