using Confluent.Kafka;

public class KafkaProducerService
{
    private readonly string _bootstrapServers;
    private readonly string _topic;

    public KafkaProducerService(string bootstrapServers, string topic)
    {
        _bootstrapServers = bootstrapServers;
        _topic = topic;
    }

    public async Task ProduceAsync(string message)
    {
        var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            var deliveryResult = await producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
            Console.WriteLine($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'.");
        }
    }
}
