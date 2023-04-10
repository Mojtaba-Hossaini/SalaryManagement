using SalaryManagementApplication.Dtos;

namespace SalaryManagementApplication.Contracts;

public interface ICalculateSalaryPayment
{
    SalaryResultDto Calculate(decimal basicSalary, decimal allowance, decimal transportation, string overTimeCalculator);
}