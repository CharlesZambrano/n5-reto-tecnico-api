// *? n5-reto-tecnico-api/N5.Permissions.Api/Controllers/PermissionTypeController.cs

using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Permissions.Application.Commands.PermissionTypeCommand;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Application.Queries.PermissionTypeQuerie;

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
        /// Obtener todos los tipos de permisos (sin la propiedad 'permissions')
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionTypeDto>>> GetPermissionTypes()
        {
            var permissionTypes = await _mediator.Send(new GetPermissionTypesQuery());
            var dtos = permissionTypes.Select(pt => new PermissionTypeDto
            {
                Id = pt.Id,
                Description = pt.Description,
                Code = pt.Code
            }).ToList();

            return Ok(dtos);
        }

        /// <summary>
        /// Crear un nuevo tipo de permiso (devuelve solo id, description y code)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PermissionTypeDto>> CreatePermissionType([FromBody] CreatePermissionTypeCommand command)
        {
            var permissionType = await _mediator.Send(command);
            var dto = new PermissionTypeDto
            {
                Id = permissionType.Id,
                Description = permissionType.Description,
                Code = permissionType.Code
            };

            return CreatedAtAction(nameof(GetPermissionTypes), new { id = dto.Id }, dto);
        }
    }
}
