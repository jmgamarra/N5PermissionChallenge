using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class PermisoControllerUnitTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PermisoController _controller;

    public PermisoControllerUnitTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PermisoController(_mediatorMock.Object);
    }

    [Fact]
    public async Task RequestPermiso_ReturnsCreatedResult()
    {
        // Arrange
        var command = new RequestPermisoCommand { /* Initialize properties */ };
        var permiso = new Permission.Domain.Permiso { Id = 1, /* Initialize properties */ };
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(permiso);

        // Act
        var result = await _controller.RequestPermiso(command) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode);
        Assert.Equal(permiso, result.Value);
    }

    [Fact]
    public async Task ModifyPermiso_ReturnsOkResult()
    {
        // Arrange
        var command = new ModifyPermisoCommand { Id = 1, /* Initialize properties */ };
        var permiso = new Permission.Domain.Permiso { Id = 1, /* Initialize properties */ };
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(permiso);

        // Act
        var result = await _controller.ModifyPermiso(1, command) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(permiso, result.Value);
    }

    [Fact]
    public async Task GetPermisos_ReturnsOkResult()
    {
        // Arrange
        var query = new GetPermisosQuery();
        var permisos = new List<Permission.Domain.Permiso>
    {
        new Permission.Domain.Permiso
        {
            Id = 1,
            NombreEmpleado = "Juan",
            ApellidoEmpleado = "Pérez",
            FechaPermiso = DateTime.Now,
            TipoPermisoId = 1 
            // Otras propiedades si es necesario
        }
    };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetPermisosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(permisos);

        // Act
        var result = await _controller.GetPermisos() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var returnedPermisos = Assert.IsType<List<Permission.Domain.Permiso>>(result.Value);
        Assert.Equal(permisos.Count, returnedPermisos.Count);

        for (int i = 0; i < permisos.Count; i++)
        {
            Assert.Equal(permisos[i].Id, returnedPermisos[i].Id);
            Assert.Equal(permisos[i].NombreEmpleado, returnedPermisos[i].NombreEmpleado);
            Assert.Equal(permisos[i].ApellidoEmpleado, returnedPermisos[i].ApellidoEmpleado);
            // Comparar otras propiedades si es necesario.
        }
    }
}
