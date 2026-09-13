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
    public DbSet<UserClass> Users => Set<UserClass>();

    /// <summary>
    /// Gets or sets Papel documents.
    /// </summary>
    public DbSet<PapelClass> Papels => Set<PapelClass>();

    /// <summary>
    /// Gets or sets Papel versions.
    /// </summary>
    public DbSet<PapelVersion> PapelVersions => Set<PapelVersion>();

    /// <summary>
    /// Gets or sets Papel shares.
    /// </summary>
    public DbSet<PapelShareClass> PapelShares => Set<PapelShareClass>();

    /// <summary>
    /// Gets or sets Papel processing jobs.
    /// </summary>
    public DbSet<PapelProcessingJobClass> PapelProcessingJobs => Set<PapelProcessingJobClass>();

    /// <summary>
    /// Gets or sets Papel audit logs.
    /// </summary>
    public DbSet<PapelAuditLogClass> PapelAuditLogs => Set<PapelAuditLogClass>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserClass>(builder =>
        {
            builder.ToTable("Users");
            builder.HasKey(user => user.Id);
            builder.HasIndex(user => user.Email).IsUnique();
            builder.HasIndex(user => user.Username).IsUnique();
        });

        modelBuilder.Entity<PapelClass>(builder =>
        {
            builder.ToTable("Papels");
            builder.HasKey(papel => papel.Id);
            builder.HasIndex(papel => new { papel.OwnerId, papel.CreatedAt });

            builder.HasOne(papel => papel.Owner)
                .WithMany(user => user.OwnedPapels)
                .HasForeignKey(papel => papel.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PapelVersion>(builder =>
        {
            builder.ToTable("PapelVersions");
            builder.HasKey(version => version.Id);
            builder.HasIndex(version => new { version.PapelId, version.VersionNumber }).IsUnique();

            builder.HasOne(version => version.Papel)
                .WithMany(papel => papel.Versions)
                .HasForeignKey(version => version.PapelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(version => version.UploadedBy)
                .WithMany(user => user.UploadedPapelVersions)
                .HasForeignKey(version => version.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PapelShareClass>(builder =>
        {
            builder.ToTable("PapelShares");
            builder.HasKey(share => share.Id);
            builder.HasIndex(share => new { share.PapelId, share.SharedWithUserId }).IsUnique();

            builder.HasOne(share => share.Papel)
                .WithMany(papel => papel.Shares)
                .HasForeignKey(share => share.PapelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(share => share.SharedWithUser)
                .WithMany(user => user.SharedPapels)
                .HasForeignKey(share => share.SharedWithUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PapelProcessingJobClass>(builder =>
        {
            builder.ToTable("PapelProcessingJobs");
            builder.HasKey(job => job.Id);
            builder.HasIndex(job => new { job.PapelId, job.CreatedAt });
            builder.HasIndex(job => new { job.PapelVersionId, job.Status });

            builder.HasOne(job => job.Papel)
                .WithMany(papel => papel.ProcessingJobs)
                .HasForeignKey(job => job.PapelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(job => job.PapelVersion)
                .WithMany(version => version.ProcessingJobs)
                .HasForeignKey(job => job.PapelVersionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PapelAuditLogClass>(builder =>
        {
            builder.ToTable("PapelAuditLogs");
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
