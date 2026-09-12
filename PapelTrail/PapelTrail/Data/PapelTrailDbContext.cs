using Microsoft.EntityFrameworkCore;
using PapelTrail.Models;

namespace PapelTrail.Data;

/// <summary>
/// Entity Framework database context for PapelTrail.
/// </summary>
/// <param name="options">Database context options.</param>
public class PapelTrailDbContext(DbContextOptions<PapelTrailDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets users.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Gets or sets documents.
    /// </summary>
    public DbSet<Document> Documents => Set<Document>();

    /// <summary>
    /// Gets or sets document versions.
    /// </summary>
    public DbSet<DocumentVersion> DocumentVersions => Set<DocumentVersion>();

    /// <summary>
    /// Gets or sets document shares.
    /// </summary>
    public DbSet<DocumentShare> DocumentShares => Set<DocumentShare>();

    /// <summary>
    /// Gets or sets processing jobs.
    /// </summary>
    public DbSet<ProcessingJob> ProcessingJobs => Set<ProcessingJob>();

    /// <summary>
    /// Gets or sets audit logs.
    /// </summary>
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(user => user.Id);
            builder.HasIndex(user => user.Email).IsUnique();
            builder.HasIndex(user => user.Username).IsUnique();
        });

        modelBuilder.Entity<Document>(builder =>
        {
            builder.HasKey(document => document.Id);
            builder.HasIndex(document => new { document.OwnerId, document.CreatedAt });

            builder.HasOne(document => document.Owner)
                .WithMany(user => user.OwnedDocuments)
                .HasForeignKey(document => document.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DocumentVersion>(builder =>
        {
            builder.HasKey(version => version.Id);
            builder.HasIndex(version => new { version.DocumentId, version.VersionNumber }).IsUnique();

            builder.HasOne(version => version.Document)
                .WithMany(document => document.Versions)
                .HasForeignKey(version => version.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(version => version.UploadedBy)
                .WithMany(user => user.UploadedDocumentVersions)
                .HasForeignKey(version => version.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DocumentShare>(builder =>
        {
            builder.HasKey(share => share.Id);
            builder.HasIndex(share => new { share.DocumentId, share.SharedWithUserId }).IsUnique();

            builder.HasOne(share => share.Document)
                .WithMany(document => document.Shares)
                .HasForeignKey(share => share.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(share => share.SharedWithUser)
                .WithMany(user => user.SharedDocuments)
                .HasForeignKey(share => share.SharedWithUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProcessingJob>(builder =>
        {
            builder.HasKey(job => job.Id);
            builder.HasIndex(job => new { job.DocumentId, job.CreatedAt });
            builder.HasIndex(job => new { job.DocumentVersionId, job.Status });

            builder.HasOne(job => job.Document)
                .WithMany(document => document.ProcessingJobs)
                .HasForeignKey(job => job.DocumentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(job => job.DocumentVersion)
                .WithMany(version => version.ProcessingJobs)
                .HasForeignKey(job => job.DocumentVersionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AuditLog>(builder =>
        {
            builder.HasKey(log => log.Id);
            builder.HasIndex(log => new { log.EntityType, log.EntityId });
            builder.HasIndex(log => log.CreatedAt);

            builder.HasOne(log => log.User)
                .WithMany(user => user.AuditLogs)
                .HasForeignKey(log => log.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
