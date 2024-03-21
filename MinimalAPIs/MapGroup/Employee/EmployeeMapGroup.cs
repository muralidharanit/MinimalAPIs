using System.Text.Json;
using MinimalAPIs.Model;

namespace MinimalAPIs.MapGroup
{
    public static class EmployeeMapGroup
    {
        static List<Department> departments = new List<Department>()
        {
            new Department (){ Id = 1, Name = "HR" },
            new Department (){ Id = 2, Name = "Sales" },
            new Department (){ Id = 3, Name = "Marketing" },
            new Department (){ Id = 3, Name = "Research and Development" }
        };

        static List<Employee> employees = new List<Employee>()
        {
            new Employee (){ Id = 1, Firstname = "Muralidharan", Lastname="Mohan", Address="Coimbatore", DepartmentId=4 },
            new Employee (){ Id = 2, Firstname = "Sidarth", Lastname="Muralidharan", Address="Coimbatore", DepartmentId=1 }
        };

        public static RouteGroupBuilder EmployeeAPI(this RouteGroupBuilder routeGroupBuilder)
        {
            // Get all employees
            routeGroupBuilder.MapGet("/", async (HttpContext context) => {
                await context.Response.WriteAsync(JsonSerializer.Serialize(employees));
            });

            // Get all employee by id
            routeGroupBuilder.MapGet("/{id:int}", async (HttpContext context) => {

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

            // Add new employee
            routeGroupBuilder.MapPost("/", async (HttpContext context, Employee employee) => {
                
                if (employee == null)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Invalid employee");
                    return;
                }
                employees.Add(employee);
                await context.Response.WriteAsync("New employee has been successfully added");
            });

            // Update employee info using id
            routeGroupBuilder.MapPut("/{id:int}", async (HttpContext context, Employee updatedEmployee) => {

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

            // Delete employee info using id
            routeGroupBuilder.MapDelete("/{id:int}", async (HttpContext context) => {

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

            return routeGroupBuilder;
        }
    }
}
