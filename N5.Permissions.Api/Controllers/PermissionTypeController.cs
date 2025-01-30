// *? n5-reto-tecnico-api/N5.Permissions.Api/Controllers/PermissionTypeController.cs

using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Permissions.Application.Commands.PermissionType;
using N5.Permissions.Application.Queries;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtener todos los tipos de permisos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionType>>> GetPermissionTypes()
        {
            var result = await _mediator.Send(new GetPermissionTypesQuery());
            return Ok(result);
        }

        /// <summary>
        /// Crear un nuevo tipo de permiso
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PermissionType>> CreatePermissionType([FromBody] CreatePermissionTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPermissionTypes), new { id = result.Id }, result);
        }
    }
}
