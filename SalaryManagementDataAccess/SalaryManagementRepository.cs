using Azure.Core;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalaryManagementDataAccess.Configs;
using SalaryManagementDomainModel;
using System;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SalaryManagementDataAccess;

public class SalaryManagementRepository
{
    private readonly SalaryManagementDbContext context;

    public SalaryManagementRepository(SalaryManagementDbContext context)
    {
        this.context = context;
    }
    
    public async Task AddAsync(Empoyee empoyee)
    {
        await context.AddAsync(empoyee);
        await context.SaveChangesAsync();
    }
    public async Task AddAsync(Salary salary)
    {
        await context.AddAsync(salary);
        await context.SaveChangesAsync();
    }
    

    public async Task UpdateAsync(Empoyee newEmpoyee)
    {
        var employee = await context.Empoyees.FirstOrDefaultAsync(c => c.EmpoyeeId == newEmpoyee.EmpoyeeId);
        if (employee == null) throw new Exception("Not Found");
        employee.FirstName = newEmpoyee.FirstName;
        employee.LastName = newEmpoyee.LastName;
        context.Entry(employee).Property("UpdateDate").CurrentValue = DateTime.Now;
        await context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Salary newSalary)
    {
        var salary = await context.Salaries.FirstOrDefaultAsync(c => c.SalaryId == newSalary.SalaryId);
        if (salary == null) throw new Exception("Not Found");
        salary.BasicSalary = newSalary.BasicSalary;
        salary.Allowance = newSalary.Allowance;
        salary.Transportation = newSalary.Transportation;
        salary.Tax = newSalary.Tax;
        salary.TotalSalary = newSalary.TotalSalary;
        salary.FinalPayment = newSalary.FinalPayment;
        salary.Date = newSalary.Date;
        context.Entry(salary).Property("UpdateDate").CurrentValue = DateTime.Now;

        await context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(Empoyee empoyee)
    {
        var employeeToDelete = await context.Empoyees.FirstOrDefaultAsync(c => c.EmpoyeeId == empoyee.EmpoyeeId);
        if (employeeToDelete == null) throw new Exception("Not Found");
        employeeToDelete.IsDeleted = true;
        context.Entry(employeeToDelete).Property("DeleteDate").CurrentValue = DateTime.Now;
        await context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Salary salary)
    {
        var salaryToDelete = await context.Salaries.FirstOrDefaultAsync(c => c.SalaryId == salary.SalaryId);
        if (salaryToDelete == null) throw new Exception("Not Found");
        salaryToDelete.IsDeleted = true;
        context.Entry(salaryToDelete).Property("DeleteDate").CurrentValue = DateTime.Now;
        await context.SaveChangesAsync();
    }
    
    public async Task<Empoyee> GetEmployeeByName(string firstName, string lastName)
    {
        var query = "select top 1 * from Employee " +
            "where FirstName = N'@FirstName' and LastName = N'@LastName' " +
            "and IsDeleted = 0";
        var parameters = new DynamicParameters();
        parameters.Add("@FirstName", firstName);
        parameters.Add("@LastName", lastName);
        using (var connection = context.CreateConnection())
        {
            var employee = await connection.QueryFirstOrDefaultAsync<Empoyee>(query, parameters);
            return employee;
        }
    }

    public async Task< List<Salary>> GetAllAsync(DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        var query = "select * from Salaries s" +
            "join Employees e on e.EmpoyeeId = s.EmployeeId " +
            "where s.Date >= @DateTimeFrom and s.Date <= @DateTimeTo and s.IsDeleted = 0 and e.IsDeleted = 0";
        var parameters = new DynamicParameters();
        parameters.Add("@DateTimeFrom", dateTimeFrom);
        parameters.Add("@DateTimeTo", dateTimeTo);

        using (var connection = context.CreateConnection())
        {
            var salaries = await connection.QueryAsync<Salary>(query, parameters);
            return salaries.ToList();
        }
    }
    public async Task<List<Salary>> GetAllUserSalaryAsync(int empoyeeId, DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        var query = "select * from Salaries s" +
            "join Employees e on e.EmpoyeeId = s.EmployeeId " +
            "where e.EmpoyeeId = @EmpoyeeId and s.Date >= @DateTimeFrom and s.Date <= @DateTimeTo and s.IsDeleted = 0 and e.IsDeleted = 0";
        var parameters = new DynamicParameters();
        parameters.Add("@EmpoyeeId", empoyeeId);
        parameters.Add("@DateTimeFrom", dateTimeFrom);
        parameters.Add("@DateTimeTo", dateTimeTo);

        using (var connection = context.CreateConnection())
        {
            var salaries = await connection.QueryAsync<Salary>(query, parameters);
            return salaries.ToList();
        }
    }
    public async Task<Salary> GetUserSalaryPerMonth(string firstName, string lastName, DateTime date)
    {
        var query = "select top 1 * from Salaries p " +
            "join Employee e on e.EmployeeId = s.EmpoyeeId " +
            "where e.FirstName = N'@FirstName' and e.LastName = N'@LastName' " +
            "and p.Date = @Date ";
        var parameters = new DynamicParameters();
        parameters.Add("@FirstName", firstName);
        parameters.Add("@LastName", lastName);
        parameters.Add("@Date", date);
        using (var connection = context.CreateConnection())
        {
            var salary = await connection.QueryFirstOrDefaultAsync<Salary>(query, parameters);
            return salary;
        }
    }
}
