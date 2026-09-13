using Microsoft.EntityFrameworkCore;
using PapelTrail.Data;
using PapelTrail.DTOs.Documents;
using PapelTrail.Models;

namespace PapelTrail.Services;

/// <summary>
/// Default implementation of <see cref="IDocumentService" />.
/// </summary>
/// <param name="dbContext">Application database context.</param>
/// <param name="logger">Document service logger.</param>
public class DocumentService(
    PapelTrailDbContext dbContext,
    ILogger<DocumentService> logger) : IDocumentService
{
    private readonly PapelTrailDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly ILogger<DocumentService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public async Task<DocumentResponseDto> CreateAsync(
        CreateDocumentRequestDto request,
        Guid ownerId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var now = DateTime.UtcNow;
        var entity = new PapelClass
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            ContentType = request.ContentType.Trim(),
            FileSize = request.FileSize,
            OwnerId = ownerId,
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false
        };

        await _dbContext.Papels.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Created document {DocumentId} for owner {OwnerId}", entity.Id, ownerId);

        return ToDto(entity);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<DocumentResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Papels
            .AsNoTracking()
            .Where(papel => !papel.IsDeleted)
            .OrderByDescending(papel => papel.CreatedAt)
            .Select(papel => ToDto(papel))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<DocumentResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Papels
            .AsNoTracking()
            .Where(papel => papel.Id == id && !papel.IsDeleted)
            .Select(papel => ToDto(papel))
            .SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Papels
            .SingleOrDefaultAsync(papel => papel.Id == id && !papel.IsDeleted, cancellationToken)
            .ConfigureAwait(false);

        if (entity is null)
        {
            return false;
        }

        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Soft deleted document {DocumentId}", id);

        return true;
    }

    private static DocumentResponseDto ToDto(PapelClass papel)
    {
        return new DocumentResponseDto
        {
            Id = papel.Id,
            Name = papel.Name,
            Description = papel.Description,
            ContentType = papel.ContentType,
            FileSize = papel.FileSize,
            OwnerId = papel.OwnerId,
            CreatedAt = papel.CreatedAt,
            UpdatedAt = papel.UpdatedAt
        };
    }
}
