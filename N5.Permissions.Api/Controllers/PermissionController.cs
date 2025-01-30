// *? n5-reto-tecnico-api/N5.Permissions.Api/Controllers/PermissionController.cs

using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Permissions.Application.Commands.PermissionCommand;
using N5.Permissions.Application.Queries.PermissionQuerie;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtener todos los permisos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permission>>> GetPermissions()
        {
            var result = await _mediator.Send(new GetPermissionsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Obtener un permiso por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Permission>> GetPermissionById(int id)
        {
            var result = await _mediator.Send(new GetPermissionByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Crear un nuevo permiso
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Permission>> CreatePermission([FromBody] CreatePermissionCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPermissionById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Modificar un permiso existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] UpdatePermissionCommand command)
        {
            if (id != command.Id) return BadRequest("El ID del permiso no coincide con el de la URL.");

            var success = await _mediator.Send(command);
            if (!success) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Buscar permisos en Elasticsearch
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Permission>>> SearchPermissions([FromQuery] string query)
        {
            var result = await _mediator.Send(new SearchPermissionsQuery(query));
            return Ok(result);
        }
    }
}
