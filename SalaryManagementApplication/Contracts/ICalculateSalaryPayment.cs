using SalaryManagementApplication.Dtos;
using SalaryManagementApplication.Enums;

namespace SalaryManagementApplication.Contracts;

public interface ICalculateSalaryPayment
{
    SalaryResultDto Calculate(decimal basicSalary, decimal allowance, decimal transportation, OverTimeCalculator overTimeCalculator);
}