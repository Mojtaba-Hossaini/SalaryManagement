using SalaryManagementApplication.Dtos;

namespace SalaryManagementApplication.Contracts;

public interface IOvertimePolicies
{
    decimal Calculate(decimal basicSalary, decimal allowance);
}
