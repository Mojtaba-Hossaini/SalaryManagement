using Dapper;
using Microsoft.EntityFrameworkCore;
using SalaryManagementApplication.Contracts;
using SalaryManagementApplication.Dtos;
using SalaryManagementApplication.Enums;
using SalaryManagementDataAccess;
using SalaryManagementDomainModel;

namespace SalaryManagementApplication;

public class SalaryManagementRepository : ISalaryManagementRepository
{
    private readonly SalaryManagementDbContext context;
    private readonly QueryContext queryContext;
    private readonly ICalculateSalaryPayment calculateSalaryPayment;

    public SalaryManagementRepository(SalaryManagementDbContext context, QueryContext queryContext,
        ICalculateSalaryPayment calculateSalaryPayment)
    {
        this.context = context;
        this.queryContext = queryContext;
        this.calculateSalaryPayment = calculateSalaryPayment;
    }

    public async Task AddSalaryPayment(SalaryDto salary, OverTimeCalculator overTimeCalculator)
    {
        var employee = await GetEmployeeByName(salary.FirstName, salary.LastName);
        var calulateSalary = calculateSalaryPayment.Calculate(salary.BasicSalary, salary.Allowance, salary.Transportation, overTimeCalculator);
        if (employee == null)
        {
            Employee newEmployee = CreateNewEmployeeWithSalary(salary, calulateSalary);
            await AddAsync(newEmployee);
        }
        else
        {
            Salary newSalaryPayment = CreateNewSalaryPayment(salary, employee, calulateSalary);
            await AddAsync(newSalaryPayment);
        }
    }
    public async Task UpdateSalaryPayment(SalaryDto salary, OverTimeCalculator overTimeCalculator)
    {
        var calulateSalary = calculateSalaryPayment.Calculate(salary.BasicSalary, salary.Allowance, salary.Transportation, overTimeCalculator);
        var salaryEntity = await GetUserSalaryPerMonth(salary.FirstName, salary.LastName, salary.Date);
        UpdateSalaryEntity(salary, calulateSalary, salaryEntity);
        await UpdateAsync(salaryEntity);
    }
    public async Task DeleteUserSalaryPerMonth(SalaryDateAndNamesDto request)
    {
        var salaryEntity = await GetUserSalaryPerMonth(request.FirstName, request.LastName, request.FromDate);
        await DeleteAsync(salaryEntity);
    }

    private static void UpdateSalaryEntity(SalaryDto salary, SalaryResultDto calulateSalary, SalaryPaymentResultDto salaryEntity)
    {
        salaryEntity.BasicSalary = salary.BasicSalary;
        salaryEntity.Allowance = salary.Allowance;
        salaryEntity.Transportation = salary.Transportation;
        salaryEntity.Date = salary.Date;
        salaryEntity.TotalSalary = calulateSalary.TotalSalary;
        salaryEntity.Tax = calulateSalary.Tax;
        salaryEntity.FinalPayment = calulateSalary.FinalPayment;
    }

    private static Salary CreateNewSalaryPayment(SalaryDto salary, Employee employee, SalaryResultDto calulateSalary) => new Salary
    {
        EmployeeId = employee.EmployeeId,
        Date = salary.Date,
        BasicSalary = salary.BasicSalary,
        Allowance = salary.Allowance,
        Transportation = salary.Transportation,
        FinalPayment = calulateSalary.FinalPayment,
        Tax = calulateSalary.Tax,
        TotalSalary = calulateSalary.TotalSalary
    };
    private static Employee CreateNewEmployeeWithSalary(SalaryDto salary, SalaryResultDto calulateSalary) => new Employee
    {
        FirstName = salary.FirstName,
        LastName = salary.LastName,
        Salaries = new List<Salary>
        {
            new Salary
            {
                Allowance = salary.Allowance,
                BasicSalary = salary.BasicSalary,
                Transportation = salary.Transportation,
                Date = salary.Date,
                FinalPayment = calulateSalary.FinalPayment,
                Tax = calulateSalary.Tax,
                TotalSalary = calulateSalary.TotalSalary
            }
        }
    };

    public async Task AddAsync(Employee empoyee)
    {
        await context.AddAsync(empoyee);
        await context.SaveChangesAsync();
    }
    public async Task AddAsync(Salary salary)
    {
        await context.AddAsync(salary);
        await context.SaveChangesAsync();
    }


    public async Task UpdateAsync(Employee newEmpoyee)
    {
        var employee = await context.Empoyees.FirstOrDefaultAsync(c => c.EmployeeId == newEmpoyee.EmployeeId);
        if (employee == null) throw new Exception("Not Found");
        employee.FirstName = newEmpoyee.FirstName;
        employee.LastName = newEmpoyee.LastName;
        context.Entry(employee).Property("UpdateDate").CurrentValue = DateTime.Now;
        await context.SaveChangesAsync();
    }
    public async Task UpdateAsync(SalaryPaymentResultDto newSalary)
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

    public async Task DeleteAsync(Employee empoyee)
    {
        var employeeToDelete = await context.Empoyees.FirstOrDefaultAsync(c => c.EmployeeId == empoyee.EmployeeId);
        if (employeeToDelete == null) throw new Exception("Not Found");
        employeeToDelete.IsDeleted = true;
        context.Entry(employeeToDelete).Property("DeleteDate").CurrentValue = DateTime.Now;
        await context.SaveChangesAsync();
    }
    public async Task DeleteAsync(SalaryPaymentResultDto salary)
    {
        var salaryToDelete = await context.Salaries.FirstOrDefaultAsync(c => c.SalaryId == salary.SalaryId);
        if (salaryToDelete == null) throw new Exception("Not Found");
        salaryToDelete.IsDeleted = true;
        context.Entry(salaryToDelete).Property("DeleteDate").CurrentValue = DateTime.Now;
        await context.SaveChangesAsync();
    }

    public async Task<Employee> GetEmployeeByName(string firstName, string lastName)
    {
        var query = "select top 1 * from Employee " +
            "where FirstName = N'@FirstName' and LastName = N'@LastName' " +
            "and IsDeleted = 0";
        var parameters = new DynamicParameters();
        parameters.Add("@FirstName", firstName);
        parameters.Add("@LastName", lastName);
        using (var connection = queryContext.CreateConnection())
        {
            var employee = await connection.QueryFirstOrDefaultAsync<Employee>(query, parameters);
            return employee;
        }
    }

    public async Task<List<Salary>> GetAllAsync(DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        var query = "select * from Salaries s " +
            "join Employees e on e.EmployeeId = s.EmployeeId " +
            "where s.Date >= @DateTimeFrom and s.Date <= @DateTimeTo and s.IsDeleted = 0 and e.IsDeleted = 0";
        var parameters = new DynamicParameters();
        parameters.Add("@DateTimeFrom", dateTimeFrom);
        parameters.Add("@DateTimeTo", dateTimeTo);

        using (var connection = queryContext.CreateConnection())
        {
            var salaries = await connection.QueryAsync<Salary>(query, parameters);
            return salaries.ToList();
        }
    }
    public async Task<List<SalaryPaymentResultDto>> GetAllUserSalaryAsync(SalaryDateAndNamesDto reqest)
    {
        var query = "select * from Salaries s " +
            "join Employees e on e.EmployeeId = s.EmployeeId " +
            "where e.FirstName = @FirstName and e.LastName = @LastName and s.Date >= @DateTimeFrom and s.Date <= @DateTimeTo and s.IsDeleted = 0 and e.IsDeleted = 0";
        var parameters = new DynamicParameters();
        parameters.Add("@EmployeeId", reqest.FirstName);
        parameters.Add("@EmployeeId", reqest.LastName);
        parameters.Add("@DateTimeFrom", reqest.FromDate);
        parameters.Add("@DateTimeTo", reqest.ToDate);

        using (var connection = queryContext.CreateConnection())
        {
            var salaries = await connection.QueryAsync<SalaryPaymentResultDto>(query, parameters);
            return salaries.ToList();
        }
    }
    public async Task<SalaryPaymentResultDto> GetUserSalaryPerMonth(string firstName, string lastName, DateTime date)
    {
        var query = "select top 1 * from Salaries s " +
            "join Empoyees e on e.EmployeeId = s.EmployeeId " +
            "where e.FirstName = @FirstName and e.LastName = @LastName " +
            "and s.Date = @Date ";
        var parameters = new DynamicParameters();
        parameters.Add("@FirstName", firstName);
        parameters.Add("@LastName", lastName);
        parameters.Add("@Date", date);
        using (var connection = queryContext.CreateConnection())
        {
            var salary = await connection.QueryFirstOrDefaultAsync<SalaryPaymentResultDto>(query, parameters);
            if(salary == null) throw new Exception("Not Found");
            //return new SalaryPaymentResultDto
            //{
            //    FirstName = salary.Empoyee.FirstName,
            //    LastName = salary.Empoyee.LastName,
            //    Date = salary.Date,
            //    BasicSalary = salary.BasicSalary,
            //    Allowance = salary.Allowance,
            //    Transportation = salary.Transportation,
            //    FinalPayment = salary.FinalPayment,
            //    Tax = salary.Tax,
            //    TotalSalary = salary.TotalSalary
            //};
            return salary;
        }
    }
}
