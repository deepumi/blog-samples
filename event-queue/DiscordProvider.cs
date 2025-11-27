using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace EventQueueSample;

public sealed class DiscordProvider(
    IOptions<DiscordSettings> options, 
    HttpClient httpClient, 
    ILogger<DiscordProvider> logger) : IAlertProvider
{
    // reads Discord settings from appsettings.json
    private readonly DiscordSettings _settings = options.Value;
    private readonly HttpClient _httpClient = httpClient;

    public async Task SendAsync(IEventMessage eventMessage, CancellationToken cancellationToken = default)
    {
        var message = eventMessage.BuildMessage();
        
        if (message == null)
        {
            return;
        }

        var emoji = eventMessage.Level switch
        {
            "warning" => "⚠️",
            "error" => "❌",
            _ => "ℹ️"
        };

        var payload = new DiscordWebhookPayload
        {
            username = _settings.Username,
            content = $"<@{_settings.UserId}>",
            allowed_mentions = new AllowedMentions
            {
                users = [_settings.UserId]
            },
            embeds =
            [
                new DiscordEmbed
                {
                    title = $"{emoji} {eventMessage.AlterTitle}",
                    description = message,
                    color = eventMessage.Level switch
                    {
                        "warning" => 16776960, // color code
                        "error" => 16711680,
                        _ => 3447003
                    }
                }
            ]
        };

        var response = await _httpClient.PostAsJsonAsync(_settings.WebhookUrl, payload, cancellationToken);
        response.EnsureSuccessStatusCode();

        logger.LogInformation("Discord sent: {Message}", message);
    }
}

public class DiscordSettings
{
    public string WebhookUrl { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string Username { get; set; } = null!;
}

internal class DiscordWebhookPayload
{
    public string username { get; set; } = null!;
    public string content { get; set; } = null!;
    public AllowedMentions allowed_mentions { get; set; } = null!;
    public DiscordEmbed[] embeds { get; set; } = null!;
}

internal class DiscordEmbed
{
    public string title { get; set; } = null!;
    public string description { get; set; } = null!;
    public int color { get; set; }
}

internal class AllowedMentions
{
    public string[] users { get; set; } = null!;
}