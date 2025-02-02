// *? n5-reto-tecnico-api/N5.Permissions.Tests/UnitTests/Handlers/PermissionHandlerTests.cs

using Moq;
using AutoMapper;
using N5.Permissions.Application.Commands.PermissionCommand;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Application.Handlers.PermissionHandler;
using N5.Permissions.Domain.Interfaces;
using N5.Permissions.Infrastructure.Elasticsearch.Services;
using N5.Permissions.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Xunit;

public class PermissionHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ElasticsearchService> _elasticServiceMock;
    private readonly Mock<IMapper> _mapperMock;

    public PermissionHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _elasticServiceMock = new Mock<ElasticsearchService>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task CreatePermissionHandler_Should_Create_Permission()
    {
        var handler = new CreatePermissionHandler(_unitOfWorkMock.Object, _elasticServiceMock.Object, _mapperMock.Object);
        var command = new CreatePermissionCommand("John", "Doe", 1, new DateTime(2025, 02, 01));

        var permissionType = new PermissionType { Id = 1, Description = "Vacaciones", Code = "VAC", Permissions = new List<Permission>() };
        _unitOfWorkMock.Setup(u => u.PermissionTypes.GetByIdAsync(1)).ReturnsAsync(permissionType);

        _unitOfWorkMock.Setup(u => u.Permissions.AddAsync(It.IsAny<Permission>()));

        _mapperMock.Setup(m => m.Map<PermissionDto>(It.IsAny<Permission>())).Returns(new PermissionDto { Id = 1 });

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task UpdatePermissionHandler_Should_Throw_ValidationException_When_Invalid_Data()
    {
        var handler = new UpdatePermissionHandler(_unitOfWorkMock.Object, _elasticServiceMock.Object);
        var command = new UpdatePermissionCommand(1, "", "Doe", 1, new DateTime(2025, 02, 01));

        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
