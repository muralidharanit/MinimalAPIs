using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Department> departments = new List<Department>()
{
    new Department (){ Id = 1, Name = "HR" },
    new Department (){ Id = 2, Name = "Sales" },
    new Department (){ Id = 3, Name = "Marketing" },
    new Department (){ Id = 3, Name = "Research and Development" }
};

List<Employee> employees = new List<Employee>()
{
    new Employee (){ Id = 1, Firstname = "Muralidharan", Lastname="Mohan", Address="Coimbatore", DepartmentId=4 },
    new Employee (){ Id = 2, Firstname = "Sidarth", Lastname="Muralidharan", Address="Coimbatore", DepartmentId=1 }
};

app.MapGet("/", () => "Hello World!");

app.MapGet("/employee", async (HttpContext context) => {
    await context.Response.WriteAsync(JsonSerializer.Serialize(employees));
});

app.MapGet("/employee/{id:int}", async (HttpContext context) => {

    if (!int.TryParse(Convert.ToString(context.Request.RouteValues["id"]), out int id))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Invalid employee ID");
        return;
    }

    var employee = employees.FirstOrDefault(emp => emp.Id == id);
    if (employee == null)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsync("Employee not found");
        return;
    }

    await context.Response.WriteAsync(JsonSerializer.Serialize(employee));
});

app.MapPost("/employee", async (HttpContext context, Employee employee) => {
    employees.Add(employee);
    await context.Response.WriteAsync("New employee has been successfully added");
});

app.MapPut("/employee/{id:int}", async (HttpContext context, Employee updatedEmployee) => {

    if (!int.TryParse(Convert.ToString(context.Request.RouteValues["id"]), out int id))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Invalid employee ID");
        return;
    }

    var employee = employees.FirstOrDefault(emp => emp.Id == id);
    if (employee == null)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsync("Employee not found");
        return;
    }

    //Update logic
    employee.Firstname = updatedEmployee.Firstname;
    employee.DepartmentId = updatedEmployee.DepartmentId;

    await context.Response.WriteAsync("Employee has been successfully updated");
});

app.MapDelete("/employee/{id:int}", async (HttpContext context) => {

    if (!int.TryParse(Convert.ToString(context.Request.RouteValues["id"]), out int id))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Invalid employee ID");
        return;
    }

    var employee = employees.FirstOrDefault(emp => emp.Id == id);
    if (employee == null)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsync("Employee not found");
        return;
    }

    employees.Remove(employee);
    await context.Response.WriteAsync("Employee has been successfully deleted");

});

app.Run();

public class Department
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class Employee
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Address { get; set; }
    public int DepartmentId { get; set; }
}