using SalaryManagementApplication.Contracts;

namespace SalaryManagementApplication.Services;

public class CalcurlatorB : IOvertimePolicies
{
    public decimal Calculate(decimal basicSalary, decimal allowance) =>
        (basicSalary + allowance) * (decimal)0.2;
}
