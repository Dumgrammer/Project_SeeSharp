using System.ComponentModel.DataAnnotations;

namespace PapelTrail.DTOs.Documents;

/// <summary>
/// Request payload for creating a document.
/// </summary>
public class CreateDocumentRequestDto
{
    /// <summary>
    /// Gets or sets document name.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets document description.
    /// </summary>
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets MIME content type.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets file size in bytes.
    /// </summary>
    [Range(1, long.MaxValue)]
    public long FileSize { get; set; }
}
