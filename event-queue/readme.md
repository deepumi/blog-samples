# Event Queue in ASP.NET Core (`Channel<T>` Example)

This repository contains the code for building a simple, reliable Event Queue in an ASP.NET Core application using `Channel<T>` and `BackgroundService`.

The goal is to show a reliable way to handle critical, asynchronous notifications (like failed payments or error spikes) by sending them to an external service (Discord).

## 🚀 How to Run

1. Clone the repository.

2. Update Configuration: Modify the `appsettings.json` file with your Discord Webhook details.JSON

```json
"Discord": {
  "WebhookUrl": "https://discord.com/api/webhooks/<replace_with_the_bot_webook_url>",
  "UserId": "<bot-id>",
  "Username": "<bot-username>"
}
```

3. Run the application. Events are triggered via the sample controller and processed instantly in the background by the `BackgroundService`.

For the full explanation, code breakdown, and best practices, see the blog post: [Building an Event Queue in ASP.NET Core]("#")
