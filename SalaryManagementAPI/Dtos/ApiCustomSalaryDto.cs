using SalaryManagementApplication.Enums;

namespace SalaryManagementAPI.Dtos;

public class ApiCustomSalaryDto
{
    public string Data { get; set; }
    public OverTimeCalculator OverTimeCalculator { get; set; }
}
