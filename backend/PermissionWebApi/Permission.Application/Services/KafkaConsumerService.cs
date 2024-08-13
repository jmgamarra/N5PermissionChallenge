using Confluent.Kafka;

public class KafkaConsumerService
{
    private readonly string _bootstrapServers;
    private readonly string _topic;

    public KafkaConsumerService(string bootstrapServers,string topic)
    {
        _bootstrapServers = bootstrapServers;
        _topic = topic;
    }

    public void Consume()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = "demo-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
        {
            consumer.Subscribe(_topic);

            while (true)
            {
                var cr = consumer.Consume(CancellationToken.None);
                Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
            }
        }
    }
}
