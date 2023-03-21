using FileToSql.Infrastructure.Enums;

namespace FileToSql.Infrastructure.Entities;

public class UploadedFile
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public FileContentType ContentType { get; set; }
    public long Size { get; set; }
    public string UploadedBy { get; set; } = null!; //user.UserName
    public DateTime UploadedOn { get; set; } = DateTime.Now;
}
