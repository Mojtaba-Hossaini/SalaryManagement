namespace SalaryManagementDomainModel;
public class Employee
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => FirstName + LastName;
    public bool IsDeleted { get; set; }
    public List<Salary> Salaries { get; set; }
}
