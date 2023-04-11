namespace SalaryManagementApplication.Dtos;

public class SalaryDateAndNamesDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
