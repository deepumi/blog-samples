namespace EventQueueSample;

public interface IEventMessage
{
    // Severity level (info, warning, error, etc.)
    string Level { get; }

    // Short title for the alert
    string AlterTitle { get; }

    // When the event occurred (always UTC)
    DateTime TimestampUtc { get; }

    // Business logic for building the alert message
    string? BuildMessage();
}

public interface IAlertProvider
{
    Task SendAsync(IEventMessage eventMessage, CancellationToken cancellationToken = default);
}