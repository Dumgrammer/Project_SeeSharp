using System.ComponentModel.DataAnnotations;

namespace PapelTrail.Models;

/// <summary>
/// Represents an asynchronous processing lifecycle for a document version.
/// </summary>
public class ProcessingJob
{
    /// <summary>
    /// Gets or sets the processing job identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the document identifier.
    /// </summary>
    public Guid DocumentId { get; set; }

    /// <summary>
    /// Gets or sets the document version identifier.
    /// </summary>
    public Guid DocumentVersionId { get; set; }

    /// <summary>
    /// Gets or sets processing state.
    /// </summary>
    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Gets or sets an optional failure message.
    /// </summary>
    [MaxLength(2000)]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets creation timestamp in UTC.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets optional processing start timestamp.
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// Gets or sets optional processing completion timestamp.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets parent document.
    /// </summary>
    public Document Document { get; set; } = null!;

    /// <summary>
    /// Gets or sets document version.
    /// </summary>
    public DocumentVersion DocumentVersion { get; set; } = null!;
}
