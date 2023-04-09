using SalaryManagementApplication.Contracts;

namespace SalaryManagementApplication.Services;

public class GetCalculator
{
    public static IOvertimePolicies Instance(string calculator) => calculator switch
    {
        nameof(CalcurlatorA) => new CalcurlatorA(),
        nameof(CalcurlatorB) => new CalcurlatorB(),
        nameof(CalcurlatorC) => new CalcurlatorC(),
        _ => null
    };
}
