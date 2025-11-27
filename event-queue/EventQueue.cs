using System.Threading.Channels;

namespace EventQueueSample;

public sealed class EventQueue(Channel<IEventMessage> channel, ILogger<EventQueue> logger)
{
    private readonly Channel<IEventMessage> _channel = channel;
    private readonly ILogger<EventQueue> _logger = logger;

    public async ValueTask QueueAsync(IEventMessage eventMessage, CancellationToken cancellationToken)
    {
        // TryWrite attempts to put the item into the channel immediately
        if (!_channel.Writer.TryWrite(eventMessage))
        {
            _logger.LogDebug("Channel write awaiting for event {Title}", eventMessage.AlterTitle);

            // Fall back to WriteAsync, which waits until there's room in the channel
            await _channel.Writer.WriteAsync(eventMessage, cancellationToken);
        }

        _logger.LogInformation("Queued event {Title}", eventMessage.AlterTitle);
    }

    public IAsyncEnumerable<IEventMessage> ReadAllAsync(CancellationToken cancellationToken)
        => _channel.Reader.ReadAllAsync(cancellationToken);
}