namespace SalaryManagementDomainModel;

public class Salary
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal Allowance { get; set; }
    public decimal Transportation { get; set; }
    public decimal TotalSalary { get; set; }
    public decimal Tax { get; set; }
    public decimal FinalPayment { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public DateTime DeleteDate { get; set; }

}
