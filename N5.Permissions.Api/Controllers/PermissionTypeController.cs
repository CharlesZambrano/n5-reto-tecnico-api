// *? n5-reto-tecnico-api/N5.Permissions.Api/Controllers/PermissionTypeController.cs

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N5.Permissions.Application.Commands.PermissionTypeCommand;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Application.Queries.PermissionTypeQuerie;

namespace N5.Permissions.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    public class PermissionTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtiene todos los tipos de permisos.
        /// </summary>
        /// <returns>Lista de tipos de permisos.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PermissionTypeDto>>> GetPermissionTypes()
        {
            var dtos = await _mediator.Send(new GetPermissionTypesQuery());
            return Ok(dtos);
        }

        /// <summary>
        /// Crea un nuevo tipo de permiso.
        /// </summary>
        /// <param name="command">Comando con los datos del nuevo tipo de permiso.</param>
        /// <returns>El tipo de permiso creado.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PermissionTypeDto>> CreatePermissionType([FromBody] CreatePermissionTypeCommand command)
        {
            var dto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPermissionTypes), new { id = dto.Id }, dto);
        }
    }
}
