using PapelTrail.DTOs.Documents;

namespace PapelTrail.Services;

/// <summary>
/// Service contract for document operations.
/// </summary>
public interface IDocumentService
{
    /// <summary>
    /// Creates a new document.
    /// </summary>
    /// <param name="request">Document creation payload.</param>
    /// <param name="ownerId">Authenticated owner identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created document.</returns>
    Task<DocumentResponseDto> CreateAsync(
        CreateDocumentRequestDto request,
        Guid ownerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all non-deleted documents.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Document list.</returns>
    Task<IReadOnlyList<DocumentResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns one document by identifier.
    /// </summary>
    /// <param name="id">Document identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The document if found; otherwise <see langword="null" />.</returns>
    Task<DocumentResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft-deletes a document by identifier.
    /// </summary>
    /// <param name="id">Document identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see langword="true" /> when deleted; otherwise <see langword="false" />.</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
