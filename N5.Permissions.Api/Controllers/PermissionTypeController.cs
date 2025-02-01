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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionTypeDto>>> GetPermissionTypes()
        {
            var dtos = await _mediator.Send(new GetPermissionTypesQuery());
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<PermissionTypeDto>> CreatePermissionType([FromBody] CreatePermissionTypeCommand command)
        {
            var dto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPermissionTypes), new { id = dto.Id }, dto);
        }
    }
}
