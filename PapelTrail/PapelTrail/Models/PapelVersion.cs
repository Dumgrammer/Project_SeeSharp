using System.ComponentModel.DataAnnotations;

namespace PapelTrail.Models;

/// <summary>
/// Represents one immutable stored version of a Papel document.
/// </summary>
public class PapelVersion
{
    /// <summary>
    /// Gets or sets the version identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the parent Papel identifier.
    /// </summary>
    public Guid PapelId { get; set; }

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
    /// Gets or sets the parent Papel document.
    /// </summary>
    public PapelClass Papel { get; set; } = null!;

    /// <summary>
    /// Gets or sets the uploader user.
    /// </summary>
    public UserClass UploadedBy { get; set; } = null!;

    /// <summary>
    /// Gets processing jobs linked to this version.
    /// </summary>
    public ICollection<PapelProcessingJobClass> ProcessingJobs { get; set; } = new List<PapelProcessingJobClass>();
}
