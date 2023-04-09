using SalaryManagementApplication.Contracts;

namespace SalaryManagementApplication.Services;

public class CalcurlatorA : IOvertimePolicies
{
    public decimal Calculate(decimal basicSalary, decimal allowance) => 
        (basicSalary  + allowance) * (decimal)0.3;
}
