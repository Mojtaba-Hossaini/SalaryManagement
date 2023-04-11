using Microsoft.EntityFrameworkCore;
using SalaryManagementDomainModel;
using System.Reflection;

namespace SalaryManagementDataAccess;

public class SalaryManagementDbContext : DbContext
{
    public SalaryManagementDbContext()
    {
        
    }

    public DbSet<Employee> Empoyees { get; set; }
    public DbSet<Salary> Salaries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=SalaryManagementDb;Integrated Security=True;Pooling=False;Encrypt=false");
    }
}