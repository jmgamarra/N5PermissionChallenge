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

        // Add Kafka message
        var message = new
        {
            Id = Guid.NewGuid(),
            NameOperation = "get"
        };

        //var topic = _configuration["Kafka:Topic"];
        await _kafkaProducerService.ProduceAsync(JsonConvert.SerializeObject(message));



        return searchResponse.Documents;
    }
}
