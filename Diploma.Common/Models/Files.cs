namespace Diploma.Common.Models;

public class Files : BaseEntity<int>
{
    public string? FileId { get; set; }
    public byte[] FileData { get; set; }
    public string FileName { get; set; }
}