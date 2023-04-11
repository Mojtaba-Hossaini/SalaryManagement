using SalaryManagementApplication.Dtos;

namespace SalaryManagementAPI.Helper;

public static class StringHepler
{
    public static SalaryDto ToSalaryDto(this string request)
    {
        var f = request.Split('\n')[1]?.Split('/');
        if (f == null || f.Length < 6)
            throw new ArgumentException("Invalid format");
        return new SalaryDto
        {
            FirstName = f[0],
            LastName = f[1],
            BasicSalary = decimal.Parse(f[2]),
            Allowance = decimal.Parse(f[3]),
            Transportation = decimal.Parse(f[4]),
            Date = DateTime.Parse(f[5]),
        };
    }
}
