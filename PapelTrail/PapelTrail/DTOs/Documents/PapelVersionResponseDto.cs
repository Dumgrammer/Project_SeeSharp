namespace PapelTrail.DTOs.Documents
{
    public class PapelVersionResponseDto
    {
        public Guid Id { get; set; }
        public Guid PapelId { get; set; }
        public string VersionNumber { get; set; } = string.Empty;
        public string StoraageKey { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string? Checksum { get; set; }
        public Guid Uploadedby { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
