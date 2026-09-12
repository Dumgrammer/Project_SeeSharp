using System.ComponentModel.DataAnnotations;

namespace PapelTrail.Models;

/// <summary>
/// Represents one immutable stored version of a document.
/// </summary>
public class DocumentVersion
{
    /// <summary>
    /// Gets or sets the version identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the parent document identifier.
    /// </summary>
    public Guid DocumentId { get; set; }

    /// <summary>
    /// Gets or sets the version number (1-based).
    /// </summary>
    public int VersionNumber { get; set; }

    /// <summary>
    /// Gets or sets the file storage key.
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string StorageKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the original file name.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the MIME content type.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the version file size in bytes.
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Gets or sets an optional file checksum.
    /// </summary>
    [MaxLength(128)]
    public string? Checksum { get; set; }

    /// <summary>
    /// Gets or sets the uploader user identifier.
    /// </summary>
    public Guid UploadedById { get; set; }

    /// <summary>
    /// Gets or sets the UTC upload timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the parent document.
    /// </summary>
    public Document Document { get; set; } = null!;

    /// <summary>
    /// Gets or sets the uploader user.
    /// </summary>
    public User UploadedBy { get; set; } = null!;

    /// <summary>
    /// Gets processing jobs linked to this version.
    /// </summary>
    public ICollection<ProcessingJob> ProcessingJobs { get; set; } = new List<ProcessingJob>();
}

