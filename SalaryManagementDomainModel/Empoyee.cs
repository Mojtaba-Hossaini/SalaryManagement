namespace SalaryManagementDomainModel;
public class Empoyee
{
    public int EmpoyeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => FirstName + LastName;
    public bool IsDeleted { get; set; }
    public List<Salary> Salaries { get; set; }
}
