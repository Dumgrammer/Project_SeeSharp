using System.ComponentModel.DataAnnotations;

namespace PapelTrail.Models;

/// <summary>
/// Represents a document share entry for another user.
/// </summary>
public class DocumentShare
{
    /// <summary>
    /// Gets or sets the share identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the shared document identifier.
    /// </summary>
    public Guid DocumentId { get; set; }

    /// <summary>
    /// Gets or sets the target user identifier.
    /// </summary>
    public Guid SharedWithUserId { get; set; }

    /// <summary>
    /// Gets or sets granted permission.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Permission { get; set; } = "View";

    /// <summary>
    /// Gets or sets the UTC share creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the UTC expiration timestamp if applicable.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the shared document.
    /// </summary>
    public Document Document { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user with whom the document is shared.
    /// </summary>
    public User SharedWithUser { get; set; } = null!;
}
