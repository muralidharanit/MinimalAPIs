using MinimalAPIs.Model;

namespace MinimalAPIs.Filter
{
    public class EmployeeIValidationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext efiContext,
            EndpointFilterDelegate next)
        {
            var employee = efiContext.GetArgument<Employee>(1);

            // Check is valid employee
            bool IsValidEmployee = employee.IsValid();

            if (!IsValidEmployee)
            {
                efiContext.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await efiContext.HttpContext.Response.WriteAsync("Employee not found");

            }
            return await next(efiContext);
        }
    }
}
