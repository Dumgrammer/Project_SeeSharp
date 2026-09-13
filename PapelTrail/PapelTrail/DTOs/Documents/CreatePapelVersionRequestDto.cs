using System.ComponentModel.DataAnnotations;

namespace PapelTrail.DTOs.Documents
{
    public class CreatePapelVersionRequestDto
    {
        [Required, MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string ContentType { get; set; } = string.Empty;

        [Range(1, long.MaxValue)]
        public long FileSize { get; set; }

        [MaxLength(128)]
        public string? Checksum { get; set; }
    }
}
