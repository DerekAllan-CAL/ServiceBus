using Microsoft.Extensions.Azure;
using ServiceBus.Api;
using ServiceBus.Api.Handlers;
using ServiceBus.Api.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddTransient<IMessageHandler, OrderMessageHandler>();
builder.Services.AddTransient<IMessageHandler, AccountMessageHandler>();
builder.Services.AddAzureClients(b =>
{
    b.AddServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"));
});
builder.Services.AddHostedService<MessageProcessorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();

