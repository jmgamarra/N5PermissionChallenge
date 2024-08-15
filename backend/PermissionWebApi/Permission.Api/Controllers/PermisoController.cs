using Confluent.Kafka;
using Elasticsearch.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PermisoController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermisoController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost]
    public async Task<IActionResult> RequestPermiso([FromBody] RequestPermisoCommand command)
    {
   
        try
        {
            var oPermiso = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPermisoById), new { id = oPermiso.Id }, oPermiso);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ElasticsearchClientException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al guardar en Elasticsearch." });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error interno del servidor." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ModifyPermiso(int id, [FromBody] ModifyPermisoCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID does not exist");
        }

        try
        {
            var oPermiso = await _mediator.Send(command);
            return Ok(oPermiso);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPermisos()
    {
        try
        {
            var query = new GetPermisosQuery();
            var users = await _mediator.Send(query);
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPermisoById(int id)
    {
        var query = new GetPermisoByIdQuery { Id = id };
        var user = await _mediator.Send(query);
        if (user == null) return NotFound();
        return Ok(user);
    }
}
