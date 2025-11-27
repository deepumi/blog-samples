namespace EventQueueSample;

public sealed class PaymentEvent(
    decimal amount,
    string email,
    decimal minThresholdAmount = 5,
    decimal maxThresholdAmount = 2500) : IEventMessage
{
    private const string HighPaymentTemplate = "High payment of `${0:N2}`: `{1}`";
    private const string LowPaymentTemplate = "Low payment of `${0:N2}`: `{1}`";

    private readonly string _email = email;
    private readonly decimal _amount = amount;
    private readonly decimal _minThresholdAmount = minThresholdAmount;
    private readonly decimal _maxThresholdAmount = maxThresholdAmount;

    public string Level => "warning";
    
    public string AlterTitle => "Payment Alert";

    public DateTime TimestampUtc => DateTime.UtcNow;

    public string? BuildMessage()
    {
        if (_amount >= _maxThresholdAmount)
        {
            return string.Format(HighPaymentTemplate, _amount, _email);
        }
        else if (_amount < _minThresholdAmount)
        {
            return string.Format(LowPaymentTemplate, _amount, _email);
        }
        return null;
    }
}