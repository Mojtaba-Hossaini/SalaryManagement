using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalaryManagementDomainModel;

namespace SalaryManagementDataAccess.Configs;
public class EmpoyeeEntityConfig : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasQueryFilter(c => !c.IsDeleted);
        builder.Property<DateTime>("CreateDate").HasDefaultValueSql("getdate()");
        builder.Property<DateTime?>("UpdateDate");
        builder.Property<DateTime?>("DeleteDate");
    }
}
