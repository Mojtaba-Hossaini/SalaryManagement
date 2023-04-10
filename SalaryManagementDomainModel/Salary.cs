namespace SalaryManagementDomainModel;

public class Salary
{
    public int SalaryId { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal Allowance { get; set; }
    public decimal Transportation { get; set; }
    public decimal TotalSalary { get; set; }
    public decimal Tax { get; set; }
    public decimal FinalPayment { get; set; }
    public DateTime Date { get; set; }
    public bool IsDeleted { get; set; }
    public int EmpoyeeId { get; set; }
    public Empoyee Empoyee { get; set; }
}
