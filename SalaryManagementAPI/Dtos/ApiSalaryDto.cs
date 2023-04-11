using SalaryManagementApplication.Dtos;
using SalaryManagementApplication.Enums;

namespace SalaryManagementAPI.Dtos;

public class ApiSalaryDto
{
    public SalaryDto Data { get; set; }
    public OverTimeCalculator OverTimeCalculator { get; set; }
}
