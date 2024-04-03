using Microsoft.AspNetCore.RateLimiting;
using MinimalAPIs.MapGroup;
using MinimalAPIs.Model;
using System;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(o => o
    .AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        // configuration
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(30);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    }));

var app = builder.Build();

app.UseRateLimiter();

app.MapGet("/", () => "Hello World!").RequireRateLimiting("fixed");

var employeeapGroup = app.MapGroup("/employee").EmployeeAPI();

app.MapGet("/hello1", () => "Hello World");
app.MapGet("/hello2", () => Results.Json(new { Message = "Hello World" }));
app.MapGet("/hello3", () => Results.StatusCode(405));
app.MapGet("/orders/{orderId}", IResult (int orderId)
    => orderId > 999 ? TypedResults.BadRequest() : TypedResults.Ok(new { Message = "Order created successfully." }));


app.Run();