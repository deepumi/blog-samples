namespace EventQueueSample;

public sealed class SlackProvider(ILogger<SlackProvider> logger) : IAlertProvider
{
    private readonly ILogger<SlackProvider> _logger = logger;

    public Task SendAsync(IEventMessage eventMessage, CancellationToken cancellationToken = default)
    {
        var message = eventMessage.BuildMessage();
        
        if (message != null)
        {
            _logger.LogInformation("Slack sent: {Message}", message);
        }

        return Task.CompletedTask;
    }
}