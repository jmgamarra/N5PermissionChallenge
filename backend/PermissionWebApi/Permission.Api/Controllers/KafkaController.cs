
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class KafkaController : ControllerBase
{
    private readonly KafkaProducerService _producerService;

    public KafkaController(KafkaProducerService producerService)
    {
        _producerService = producerService;
    }

    [HttpPost("produce")]
    public async Task<IActionResult> Produce([FromBody] string message)
    {
        await _producerService.ProduceAsync(message);
        return Ok("Message sent to Kafka");
    }
}
