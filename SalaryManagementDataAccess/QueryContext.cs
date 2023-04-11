using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SalaryManagementDataAccess.Configs;
using System.Data;

namespace SalaryManagementDataAccess;

public class QueryContext
{
    private readonly IOptionsMonitor<ConnectionStrings> options;

    public QueryContext(IOptionsMonitor<ConnectionStrings> options)
    {
        this.options = options;
    }
    public IDbConnection CreateConnection() => new SqlConnection(options.CurrentValue.DefaultConnection);
}
