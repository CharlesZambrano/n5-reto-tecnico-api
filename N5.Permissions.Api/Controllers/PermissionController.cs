// *? n5-reto-tecnico-api/N5.Permissions.Api/Controllers/PermissionController.cs

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N5.Permissions.Application.Commands.PermissionCommand;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Application.Queries.PermissionQuerie;

namespace N5.Permissions.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "User,Administrator")]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all permissions.
        /// </summary>
        /// <returns>List of permissions.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetPermissions()
        {
            var result = await _mediator.Send(new GetPermissionsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get a permission by ID.
        /// </summary>
        /// <param name="id">Permission ID. </param>
        /// <returns>Permission found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PermissionDto>> GetPermissionById(int id)
        {
            var result = await _mediator.Send(new GetPermissionByIdQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Create a new permission.
        /// </summary>
        /// <param name="command">Command with the new permission data.</param>
        /// <returns>The permission created.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PermissionDto>> CreatePermission([FromBody] CreatePermissionCommand command)
        {
            var dto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPermissionById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Modify an existing permission.
        /// </summary>
        /// <param name="id">ID of the permission to modify.</param>
        /// <param name="command">Updated permission data.</param>
        /// <returns>No content if successful; NotFound otherwise.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] UpdatePermissionCommand command)
        {
            command.Id = id;
            var success = await _mediator.Send(command);
            if (!success)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Search for permissions using a query term.
        /// </summary>
        /// <param name="query">Text to search for.</param>
        /// <returns>List of permissions that match the search.</returns>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> SearchPermissions([FromQuery] string query)
        {
            var result = await _mediator.Send(new SearchPermissionsQuery(query));
            return Ok(result);
        }

        /// <summary>
        /// Reindex permissions in Elasticsearch.
        /// </summary>
        /// <returns>No Content on success; Error 500 on failure.</returns>
        [HttpPost("reindex")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReindexPermissions()
        {
            var success = await _mediator.Send(new ReindexPermissionsCommand());
            return success ? NoContent() : StatusCode(500, "Error en la reindexación");
        }
    }
}
