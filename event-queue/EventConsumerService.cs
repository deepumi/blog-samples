namespace EventQueueSample;

public sealed class EventConsumerService(
    EventQueue queue,
    IEnumerable<IAlertProvider> providers,
    ILogger<EventConsumerService> logger) : BackgroundService
{
    private readonly EventQueue _queue = queue;
    private readonly IEnumerable<IAlertProvider> _providers = providers;
    private readonly ILogger<EventConsumerService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("EventConsumerService started");

        await foreach (var eventMessage in _queue.ReadAllAsync(cancellationToken))
        {
            _logger.LogInformation("Consuming event {Title} at {TimestampUtc}", eventMessage.AlterTitle, eventMessage.TimestampUtc);

            foreach (var provider in _providers)
            {
                try
                {
                    await provider.SendAsync(eventMessage, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex, 
                        "Provider {Provider} failed for event {Title}",
                        provider.GetType().Name, 
                        eventMessage.AlterTitle);
                }
            }
        }

        _logger.LogInformation("EventConsumerService stopping");
    }
}
