using System.ComponentModel.DataAnnotations;

namespace PapelTrail.Models;

/// <summary>
/// Represents an asynchronous processing lifecycle for a Papel version.
/// </summary>
public class PapelProcessingJobClass
{
    /// <summary>
    /// Gets or sets the processing job identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the Papel identifier.
    /// </summary>
    public Guid PapelId { get; set; }

    /// <summary>
    /// Gets or sets the Papel version identifier.
    /// </summary>
    public Guid PapelVersionId { get; set; }

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
    /// Gets or sets parent Papel document.
    /// </summary>
    public PapelClass Papel { get; set; } = null!;

    /// <summary>
    /// Gets or sets Papel version.
    /// </summary>
    public PapelVersion PapelVersion { get; set; } = null!;
}
