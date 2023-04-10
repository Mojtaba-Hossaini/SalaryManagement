using SalaryManagementApplication.Contracts;
using SalaryManagementApplication.Dtos;
using SalaryManagementDataAccess;
using SalaryManagementDomainModel;

namespace SalaryManagementApplication;

public class ApplicationService
{
    private readonly SalaryManagementRepository repository;
    private readonly ICalculateSalaryPayment calculateSalaryPayment;

    public ApplicationService(SalaryManagementRepository repository, ICalculateSalaryPayment calculateSalaryPayment)
    {
        this.repository=repository;
        this.calculateSalaryPayment=calculateSalaryPayment;
    }
    public async Task AddSalaryPayment(SalaryDto salary, string overTimeCalculator)
    {
        var employee = await repository.GetEmployeeByName(salary.FirstName, salary.LastName);
        var calulateSalary = calculateSalaryPayment.Calculate(salary.BasicSalary, salary.Allowance, salary.Transportation, overTimeCalculator);
        if (employee == null)
        {
            Empoyee newEmployee = CreateNewEmployeeWithSalary(salary, calulateSalary);
            await repository.AddAsync(newEmployee);
        }
        else
        {
            Salary newSalaryPayment = CreateNewSalaryPayment(salary, employee, calulateSalary);
            await repository.AddAsync(newSalaryPayment);
        }
    }

    public async Task UpdateSalaryPayment(SalaryDto salary, string overTimeCalculator)
    {
        var calulateSalary = calculateSalaryPayment.Calculate(salary.BasicSalary, salary.Allowance, salary.Transportation, overTimeCalculator);
        var salaryEntity = await repository.GetUserSalaryPerMonth(salary.FirstName, salary.LastName, salary.Date);
        UpdateSalaryEntity(salary, calulateSalary, salaryEntity);
        await repository.UpdateAsync(salaryEntity);
    }
    public async Task DeleteUserSalaryPerMonth(string firstName, string lastName, DateTime date)
    {
        var salaryEntity = await repository.GetUserSalaryPerMonth(firstName, lastName, date);
        await repository.DeleteAsync(salaryEntity);
    }

    private static void UpdateSalaryEntity(SalaryDto salary, SalaryResultDto calulateSalary, Salary salaryEntity)
    {
        salaryEntity.BasicSalary = salary.BasicSalary;
        salaryEntity.Allowance = salary.Allowance;
        salaryEntity.Transportation = salary.Transportation;
        salaryEntity.Date = salary.Date;
        salaryEntity.TotalSalary = calulateSalary.TotalSalary;
        salaryEntity.Tax = calulateSalary.Tax;
        salaryEntity.FinalPayment = calulateSalary.FinalPayment;
    }

    private static Salary CreateNewSalaryPayment(SalaryDto salary, Empoyee employee, SalaryResultDto calulateSalary) => new Salary
    {
        EmpoyeeId = employee.EmpoyeeId,
        Date = salary.Date,
        BasicSalary = salary.BasicSalary,
        Allowance = salary.Allowance,
        Transportation = salary.Transportation,
        FinalPayment = calulateSalary.FinalPayment,
        Tax = calulateSalary.Tax,
        TotalSalary = calulateSalary.TotalSalary
    };
    private static Empoyee CreateNewEmployeeWithSalary(SalaryDto salary, SalaryResultDto calulateSalary) => new Empoyee
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
}