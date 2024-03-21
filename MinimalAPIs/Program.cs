using MinimalAPIs.MapGroup;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
var employeeapGroup = app.MapGroup("/employee").EmployeeAPI();

app.Run();