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
        try
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            // TODO: replace with authenticated user id from JWT claims once auth is enabled.

            if (!Request.Headers.TryGetValue("X-User-Id", out var userIdHeader)
                 || !Guid.TryParse(userIdHeader.ToString(), out var ownerId)
                 || ownerId == Guid.Empty)
            {
                return BadRequest("Header X-User-Id is required and must be a valid user GUID.");
            }
            var createdDocument = await _documentService.CreateAsync(request, ownerId, cancellationToken).ConfigureAwait(false);

            return CreatedAtAction("GetById", new { id = createdDocument.Id }, createdDocument);

        }
        catch (Exception ex)
        {
            // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
            // For this example, we'll just write to the console.
            Console.WriteLine($"An error occurred while creating the document: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the document.");
        }

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
        try
        {
            var documents = await _documentService.GetAllAsync(cancellationToken).ConfigureAwait(false);
            return Ok(documents);
        }
        catch (Exception ex)
        {
            // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
            // For this example, we'll just write to the console.
            Console.WriteLine($"An error occurred while retrieving documents: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving documents.");
        }
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
        try
        {
            var document = await _documentService.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (document is null)
            {
                return NotFound();
            }

            return Ok(document);
        }
        catch (Exception ex)
        {
            // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
            // For this example, we'll just write to the console.
            Console.WriteLine($"An error occurred while retrieving the document: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the document.");
        }
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
        try
        {
            var wasDeleted = await _documentService.DeleteAsync(id, cancellationToken).ConfigureAwait(false);
            if (!wasDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
            // For this example, we'll just write to the console.
            Console.WriteLine($"An error occurred while deleting the document: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the document.");
        }
    }
}
