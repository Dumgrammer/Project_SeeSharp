using Microsoft.AspNetCore.Mvc;
using PapelTrail.DTOs.Documents;
using PapelTrail.Services;

namespace PapelTrail.Controllers;

/// <summary>
/// Exposes REST endpoints for document CRUD operations.
/// </summary>
/// <param name="documentService">Document business service.</param>
[ApiController]
[Route("api/[controller]")]
public class DocumentsController(IDocumentService documentService) : ControllerBase
{
    private readonly IDocumentService _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));

    /// <summary>
    /// Creates a new document.
    /// </summary>
    /// <param name="request">Document creation payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created document.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(DocumentResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DocumentResponseDto>> CreateAsync(
        [FromBody] CreateDocumentRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        // TODO: replace with authenticated user id from JWT claims once auth is enabled.
        var ownerId = Guid.Empty;
        var createdDocument = await _documentService.CreateAsync(request, ownerId, cancellationToken).ConfigureAwait(false);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = createdDocument.Id }, createdDocument);
    }

    /// <summary>
    /// Gets all documents.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Document list.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DocumentResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<DocumentResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var documents = await _documentService.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return Ok(documents);
    }

    /// <summary>
    /// Gets one document by id.
    /// </summary>
    /// <param name="id">Document identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The document when found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DocumentResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DocumentResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var document = await _documentService.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (document is null)
        {
            return NotFound();
        }

        return Ok(document);
    }

    /// <summary>
    /// Soft-deletes one document by id.
    /// </summary>
    /// <param name="id">Document identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content when deleted.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var wasDeleted = await _documentService.DeleteAsync(id, cancellationToken).ConfigureAwait(false);
        if (!wasDeleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
