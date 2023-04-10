using Microsoft.Extensions.Options;
using SalaryManagementApplication.Config;
using SalaryManagementApplication.Contracts;
using SalaryManagementApplication.Dtos;

namespace SalaryManagementApplication.Services;
public class CalculateSalaryPayment : ICalculateSalaryPayment
{
    private readonly IOptionsMonitor<TaxConfig> options;

    public CalculateSalaryPayment(IOptionsMonitor<TaxConfig> options)
    {
        this.options=options;
    }
    public SalaryResultDto Calculate(decimal basicSalary, decimal allowance, decimal transportation, string overTimeCalculator)
    {
        var totalSalary = basicSalary + allowance + transportation + GetCalculator.Instance(overTimeCalculator).Calculate(basicSalary, allowance);
        var tax = totalSalary * options.CurrentValue.Tax;
        var finalPayment = totalSalary - tax;
        return  new SalaryResultDto
        {
            TotalSalary = totalSalary,
            Tax = tax,
            FinalPayment = finalPayment
        };
    }
}
