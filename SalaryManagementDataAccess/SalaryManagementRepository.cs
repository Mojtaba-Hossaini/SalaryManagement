using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalaryManagementDataAccess.Configs;
using SalaryManagementDomainModel;
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
    

    public void Add(Salary salary)
    {
        context.Add(salary);
        context.SaveChanges();
    }

    public async Task Update(Salary newSalary)
    {
        var salary = await context.Salaries.FirstOrDefaultAsync(c => c.Id == newSalary.Id);
        if (salary == null)
            throw new Exception("Not Found");
        salary.FirstName = newSalary.FirstName;
        salary.LastName = newSalary.LastName;
        salary.BasicSalary = newSalary.BasicSalary;
        salary.Allowance = newSalary.Allowance;
        salary.Transportation = newSalary.Transportation;
        salary.TotalSalary = newSalary.TotalSalary;
        salary.Tax = newSalary.Tax;
        salary.Date = newSalary.Date;
        salary.FinalPayment = newSalary.FinalPayment;
        salary.UpdateDate = DateTime.Now;
        await context.SaveChangesAsync();
    }

    public async Task< List<Salary>> GetAllAsync()
    {
        var query = "SELECT * FROM Salaries";

        using (var connection = context.CreateConnection())
        {
            var salaries = await connection.QueryAsync<Salary>(query);
            return salaries.ToList();
        }
    }
}
