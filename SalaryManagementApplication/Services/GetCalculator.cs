using SalaryManagementApplication.Contracts;
using SalaryManagementApplication.Enums;

namespace SalaryManagementApplication.Services;

public class GetCalculator
{
    public static IOvertimePolicies Instance(OverTimeCalculator calculator) => calculator switch
    {
        OverTimeCalculator.CalculatorA => new CalcurlatorA(),
        OverTimeCalculator.CalculatorB => new CalcurlatorB(),
        OverTimeCalculator.CalculatorC => new CalcurlatorC(),
        _ => null
    };
}
