using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalaryManagementDomainModel;

namespace SalaryManagementDataAccess.Configs;
public class SalaryEntityConfig : IEntityTypeConfiguration<Salary>
{
    public void Configure(EntityTypeBuilder<Salary> builder)
    {
        builder.HasQueryFilter(c => !c.IsDeleted);
        builder.Property<DateTime>("CreateDate").HasDefaultValueSql("getdate()");
        builder.Property<DateTime?>("UpdateDate");
        builder.Property<DateTime?>("DeleteDate");
    }
}
