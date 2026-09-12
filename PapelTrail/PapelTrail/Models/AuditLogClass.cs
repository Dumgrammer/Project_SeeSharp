using System.ComponentModel.DataAnnotations;

namespace PapelTrail.Models;

/// <summary>
/// Tracks auditable actions in the system.
/// </summary>
public class AuditLog
{
    /// <summary>
    /// Gets or sets the audit record identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the related user identifier.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Gets or sets action key (for example DOCUMENT_CREATED).
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets entity type name.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target entity identifier.
    /// </summary>
    public Guid? EntityId { get; set; }

    /// <summary>
    /// Gets or sets optional extra details.
    /// </summary>
    [MaxLength(4000)]
    public string? Details { get; set; }

    /// <summary>
    /// Gets or sets creation timestamp in UTC.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets related user navigation.
    /// </summary>
    public User? User { get; set; }
}
