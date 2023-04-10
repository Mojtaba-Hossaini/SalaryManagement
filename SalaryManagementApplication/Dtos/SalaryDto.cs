namespace SalaryManagementApplication.Dtos;

public class SalaryDto
{
    public decimal BasicSalary { get; set; }
    public decimal Allowance { get; set; }
    public decimal Transportation { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Date { get; set; }
}
