using Confluent.Kafka;
using Confluent.Kafka.Admin;

public class KafkaConsumerService
{
    private readonly string _bootstrapServers;
    private readonly string _topic;
    private readonly int _retryAttempts;
    private readonly int _retryDelay;

    public KafkaConsumerService(string bootstrapServers, string topic, int retryAttempts = 5, int retryDelay = 5000)
    {
        _bootstrapServers = bootstrapServers;
        _topic = topic;
        _retryAttempts = retryAttempts;
        _retryDelay = retryDelay;
    }

    public async Task CreateTopicIfNotExists()
    {
        using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _bootstrapServers }).Build();

        try
        {
            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            if (!metadata.Topics.Any(t => t.Topic == _topic))
            {
                await adminClient.CreateTopicsAsync(new TopicSpecification[]
                {
                    new TopicSpecification { Name = _topic, NumPartitions = 1, ReplicationFactor = 1 }
                });
                Console.WriteLine($"Topic '{_topic}' created.");
            }
        }
        catch (CreateTopicsException ex)
        {
            Console.WriteLine($"An error occurred creating topic {ex.Results[0].Topic}: {ex.Results[0].Error.Reason}");
        }
    }

    public async Task Consume()
    {
        await CreateTopicIfNotExists();

        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = "demo-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        for (int attempt = 1; attempt <= _retryAttempts; attempt++)
        {
            try
            {
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
            catch (KafkaException ex)
            {
                Console.WriteLine($"Attempt {attempt} failed: {ex.Message}");
                if (attempt < _retryAttempts)
                {
                    Console.WriteLine($"Retrying in {_retryDelay / 1000} seconds...");
                    Thread.Sleep(_retryDelay);
                }
                else
                {
                    Console.WriteLine("Max retry attempts reached. Giving up.");
                    throw;
                }
            }
        }
    }
}
