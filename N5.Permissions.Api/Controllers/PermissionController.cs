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
        /// Obtiene todos los permisos.
        /// </summary>
        /// <returns>Lista de permisos.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetPermissions()
        {
            var result = await _mediator.Send(new GetPermissionsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Obtiene un permiso por su ID.
        /// </summary>
        /// <param name="id">ID del permiso.</param>
        /// <returns>El permiso encontrado.</returns>
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
        /// Crea un nuevo permiso.
        /// </summary>
        /// <param name="command">Comando con los datos del nuevo permiso.</param>
        /// <returns>El permiso creado.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PermissionDto>> CreatePermission([FromBody] CreatePermissionCommand command)
        {
            var dto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPermissionById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Modifica un permiso existente.
        /// </summary>
        /// <param name="id">ID del permiso a modificar.</param>
        /// <param name="command">Datos actualizados del permiso.</param>
        /// <returns>No content si es exitoso; NotFound en caso contrario.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] UpdatePermissionCommand command)
        {
            // Asigna el ID de la ruta al comando; las validaciones se realizan en el handler.
            command.Id = id;
            var success = await _mediator.Send(command);
            if (!success)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Busca permisos usando un término de consulta.
        /// </summary>
        /// <param name="query">Texto a buscar.</param>
        /// <returns>Lista de permisos que coinciden con la búsqueda.</returns>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> SearchPermissions([FromQuery] string query)
        {
            var result = await _mediator.Send(new SearchPermissionsQuery(query));
            return Ok(result);
        }

        /// <summary>
        /// Reindexa los permisos en Elasticsearch.
        /// </summary>
        /// <returns>No Content si es exitoso; Error 500 en caso de fallo.</returns>
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
