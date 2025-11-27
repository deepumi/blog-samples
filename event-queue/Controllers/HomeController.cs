using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EventQueueSample.Models;

namespace EventQueueSample.Controllers;

public class HomeController(EventQueue eventQueue, ILogger<HomeController> logger) : Controller
{
    private readonly EventQueue _eventQueue = eventQueue;
    private readonly ILogger<HomeController> _logger = logger;

    public async Task<IActionResult> Index(bool sendAlert, CancellationToken cancellationToken)
    {
        if (sendAlert)
        {
            var email = "jon.doe@gmail.com";
            var amount = 4.00m;

            _logger.LogInformation("Received payment from {Email} amount {Amount}", email, amount);

            var evt = new PaymentEvent(amount, email);

            await _eventQueue.QueueAsync(evt, cancellationToken);
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}