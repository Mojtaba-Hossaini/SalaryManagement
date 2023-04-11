using SalaryManagementApplication.Dtos;
using SalaryManagementDomainModel;

namespace SalaryManagementApplication;

public interface ISalaryManagementRepository
{
    Task AddAsync(Employee empoyee);
    Task AddAsync(Salary salary);
    Task AddSalaryPayment(SalaryDto salary, global::SalaryManagementApplication.Enums.OverTimeCalculator overTimeCalculator);
    Task DeleteAsync(Employee empoyee);
    Task DeleteAsync(SalaryPaymentResultDto salary);
    Task DeleteUserSalaryPerMonth(SalaryDateAndNamesDto request);
    Task<List<Salary>> GetAllAsync(DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<List<SalaryPaymentResultDto>> GetAllUserSalaryAsync(SalaryDateAndNamesDto request);
    Task<Employee> GetEmployeeByName(string firstName, string lastName);
    Task<SalaryPaymentResultDto> GetUserSalaryPerMonth(string firstName, string lastName, DateTime date);
    Task UpdateAsync(Employee newEmpoyee);
    Task UpdateAsync(SalaryPaymentResultDto newSalary);
    Task UpdateSalaryPayment(global::SalaryManagementApplication.Dtos.SalaryDto salary, global::SalaryManagementApplication.Enums.OverTimeCalculator overTimeCalculator);
}
