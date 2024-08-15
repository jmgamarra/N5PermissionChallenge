using MediatR;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using Permission.Domain;

public class GetPermisosQueryHandler : IRequestHandler<GetPermisosQuery, IEnumerable<Permiso>>
{
    //private readonly IUnitOfWork _unitOfWork;
    private readonly IElasticClient _elasticClient;
    private readonly KafkaProducerService _kafkaProducerService;
    private readonly IConfiguration _configuration;
    public GetPermisosQueryHandler(IElasticClient elasticClient, KafkaProducerService kafkaProducerService, IConfiguration configuration)
    {
        _elasticClient = elasticClient;
        _kafkaProducerService = kafkaProducerService;
        _configuration = configuration;
    }

    public async Task<IEnumerable<Permiso>> Handle(GetPermisosQuery request, CancellationToken cancellationToken)
    {
        var searchResponse = await _elasticClient.SearchAsync<Permiso>(s => s
            .Index("permissions")
            .From(0)
            .Size(1000) 
        );

        if (!searchResponse.IsValid)
        {
            throw new Exception("Failed to retrieve documents from Elasticsearch.");
        }
        // no documents
        if (searchResponse.Documents == null || !searchResponse.Documents.Any())
        {
            return Enumerable.Empty<Permiso>();
        }

        // Add Kafka message
        var message = new
        {
            Id = Guid.NewGuid(),
            NameOperation = "get",
            Timestamp = DateTime.UtcNow
        };

        try
        {
            var jsonMessage = JsonConvert.SerializeObject(message);
            await _kafkaProducerService.ProduceAsync(jsonMessage);
        }
        catch (Exception ex)
        {
            // Manejar error de Kafka
            // Aquí podrías loggear el error y decidir si lanzar una excepción o no
            throw new InvalidOperationException("Failed to send message to Kafka.", ex);
        }

        return searchResponse.Documents;
    }
}
