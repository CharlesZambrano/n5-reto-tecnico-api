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
        /// Obtains all types of permissions.
        /// </summary>
        /// <returns>List of permission types.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PermissionTypeDto>>> GetPermissionTypes()
        {
            var dtos = await _mediator.Send(new GetPermissionTypesQuery());
            return Ok(dtos);
        }

        /// <summary>
        /// Create a new permission type.
        /// </summary>
        /// <param name="command">Command with the data of the new type of permission.</param>
        /// <returns>The type of permission created.</returns>
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
