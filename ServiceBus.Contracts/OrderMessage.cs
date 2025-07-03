namespace ServiceBus.Contracts;

public class OrderMessage
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
}
