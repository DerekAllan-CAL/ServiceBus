// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using ServiceBus.Contracts;

var config = new ConfigurationBuilder()
.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
.AddUserSecrets<Program>()
.Build();


var client = new ServiceBusClient(config.GetConnectionString("ServiceBus"));


await using var ordersSender = client.CreateSender("orderstopic");
await using var accountSender = client.CreateSender("accountstopic");
while (true)
{
    Console.WriteLine("Press Enter To Send Message ('a' for Account, 'o' for Order, 'q' to quit): ");
    var s = Console.ReadLine();

    if (s?.ToLower() == "q")
    {
        break;
    }

    switch (s?.ToLower())
    {
        case "o":
            Console.WriteLine("Sending Order Message...");
            var orderMessage = new OrderMessage
            {
                OrderId = Guid.NewGuid(),
                Amount = Random.Shared.Next(1, 1000) // Random amount for demonstration
            };

            await ordersSender.SendMessageAsync(new ServiceBusMessage(System.Text.Json.JsonSerializer.Serialize(orderMessage))
            {
                ContentType = "application/json"
            });
            break;
        case "a":
            Console.WriteLine("Sending Account Message...");
            var accountMessage = new AccountMessage
            {
                AccountId = Guid.NewGuid(),
                AccountNumber = Random.Shared.Next(1, 10000) // Random account number for demonstration
            };

            await accountSender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(accountMessage, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true }))
            {
                ContentType = "application/json"
            });
            break;
        default:
            Console.WriteLine("Unknown command. Please enter 'o' for Order or 'a' for Account.");
            break;
    }
}