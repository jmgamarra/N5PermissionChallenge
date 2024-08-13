using MediatR;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using Permission.Domain;

public class RequestPermisoCommandHandler : IRequestHandler<RequestPermisoCommand, Permiso>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IElasticClient _elasticClient;
    private readonly KafkaProducerService _kafkaProducerService;
    private readonly IConfiguration _configuration;

    public RequestPermisoCommandHandler(IElasticClient elasticClient, IUnitOfWork unitOfWork, KafkaProducerService kafkaProducerService, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _elasticClient = elasticClient;
        _kafkaProducerService = kafkaProducerService;
        _configuration = configuration;
    }

    public async Task<Permiso> Handle(RequestPermisoCommand request, CancellationToken cancellationToken)
    {

        var oPermiso = new Permiso
        {
            NombreEmpleado = request.Nombre,
            ApellidoEmpleado = request.Apellido,
            FechaPermiso = request.Fecha,
            TipoPermisoId = request.TipoId,
        };
        await _unitOfWork.Permissions.AddAsync(oPermiso);
        await _unitOfWork.SaveChangesAsync();

        // Guardar en Elasticsearch
        var response = await _elasticClient.IndexDocumentAsync(new Permiso
        {
            Id = oPermiso.Id,
            ApellidoEmpleado = oPermiso.ApellidoEmpleado,
            NombreEmpleado = oPermiso.NombreEmpleado,
            FechaPermiso = oPermiso.FechaPermiso,
            TipoPermisoId = oPermiso.TipoPermisoId
        });

        // send to topic
        var message = new
        {
            Id = Guid.NewGuid(),
            NameOperation = "request"
        };

        var topic = _configuration["Kafka:Topic"];
        await _kafkaProducerService.ProduceAsync(JsonConvert.SerializeObject(message));


        if (!response.IsValid)
        {
            throw new Exception("Saving failed in Elasticsearch.");
        }

        return oPermiso;
    }
}
