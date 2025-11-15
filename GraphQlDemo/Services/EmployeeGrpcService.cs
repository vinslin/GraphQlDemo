using GraphQlDemo.Grpc;
using GraphQlDemo.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;


namespace GraphQlDemo.Services;

public class EmployeeGrpcService : EmployeeService.EmployeeServiceBase
{
    private readonly EmployeesDbContext db;
    public EmployeeGrpcService(EmployeesDbContext context)
    {
        db = context;
    }

    public override async Task<EmployeeResponse> CreateEmployee(CreateEmployeeRequest req, ServerCallContext context)
    {
        var employee = new Employee()
        {
            Id = Guid.NewGuid(),
            Name = req.Name,
            Salary = (decimal)req.Salary,
            Email = req.Email
        };
        db.Employees.Add(employee);
        await db.SaveChangesAsync();
        return new EmployeeResponse
        {
            Id = employee.Id.ToString(),
            Name = employee.Name,
            Email = employee.Email,
            Salary = (double)employee.Salary,
        };
    }
    public override async Task<EmployeeResponse> GetEmployee(EmployeeRequest req, ServerCallContext context)
    {
        Guid id = Guid.Parse(req.Id);
        var employee = await db.Employees.FindAsync(id);
        if (employee == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Employee not Available"));
        }
        return new EmployeeResponse
        {
            Id = employee.Id.ToString(),
            Name = employee.Name,
            Email = employee.Email,
            Salary = (double)employee.Salary,
        };
    }

    public override async Task<EmployeeListResponse> GetAllEmployees(Empty request, ServerCallContext context)
    {
        var employees = await db.Employees.ToListAsync();
        var response = new EmployeeListResponse();

        foreach (var emp in employees)
        {
            response.Employees.Add(new EmployeeResponse()
            {
                Id = emp.Id.ToString(),
                Name = emp.Name,
                Email = emp.Email,
                Salary = (double)emp.Salary,
            });
        }
        return response;
    }

    public override async Task<EmployeeResponse> UpdateEmployee(UpdateEmployeeRequest req, ServerCallContext context)
    {
        Guid id = Guid.Parse(req.Id);
        var employee = await db.Employees.FindAsync(id);
        if (employee == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Employee Not Available"));
        }
        employee.Name = req.Name;
        employee.Email = req.Email;
        employee.Salary = (decimal)req.Salary;
        db.Employees.Update(employee);
        await db.SaveChangesAsync();
        return new EmployeeResponse
        {
            Id = employee.Id.ToString(),
            Name = employee.Name,
            Email = employee.Email,
            Salary = (double)employee.Salary,
        };
    }
    public override async Task<DeleteResponse> DeleteEmployee(EmployeeRequest req, ServerCallContext context)
    {
        Guid id = Guid.Parse(req.Id);
        var employee = await db.Employees.FindAsync(id);
        if (employee == null)
        {
            return new DeleteResponse() { IsDeleted = false, Message = "resource notfound" };
        }
        db.Employees.Remove(employee);
        await db.SaveChangesAsync();
        return new DeleteResponse() { IsDeleted = true, Message = "deleted" };
    }
}