using System.Threading.Channels;
using EventQueueSample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
// required for making http requests
builder.Services.AddHttpClient();

// Single channel for all events
builder.Services.AddSingleton(Channel.CreateUnbounded<IEventMessage>());

// Queue and consumer
builder.Services.AddSingleton<EventQueue>();
builder.Services.AddHostedService<EventConsumerService>();

// reads and bind Discord settings from appsettings.json
builder.Services.Configure<DiscordSettings>(builder.Configuration.GetSection("Discord"));

// Providers
builder.Services.AddSingleton<IAlertProvider, DiscordProvider>();
builder.Services.AddSingleton<IAlertProvider, SlackProvider>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();