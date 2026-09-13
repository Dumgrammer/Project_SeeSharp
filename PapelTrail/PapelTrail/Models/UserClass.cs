using System.ComponentModel.DataAnnotations;

namespace PapelTrail.Models;

/// <summary>
/// Represents an authenticated PapelTrail user.
/// </summary>
public class UserClass
{
    /// <summary>
    /// Gets or sets the unique user identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hashed password.
    /// </summary>
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user role.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Role { get; set; } = "User";

    /// <summary>
    /// Gets or sets the UTC creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the documents owned by this user.
    /// </summary>
    public ICollection<PapelClass> OwnedPapels { get; set; } = new List<PapelClass>();

    /// <summary>
    /// Gets document versions uploaded by this user.
    /// </summary>
    public ICollection<PapelVersion> UploadedPapelVersions { get; set; } = new List<PapelVersion>();

    /// <summary>
    /// Gets document shares received by this user.
    /// </summary>
    public ICollection<PapelShareClass> SharedPapels { get; set; } = new List<PapelShareClass>();

    /// <summary>
    /// Gets the audit events attributed to this user.
    /// </summary>
    public ICollection<PapelAuditLogClass> AuditLogs { get; set; } = new List<PapelAuditLogClass>();
}
