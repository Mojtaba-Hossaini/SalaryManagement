﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalaryManagementDataAccess.Configs;
using SalaryManagementDomainModel;
using System.Data;
using System.Reflection;

namespace SalaryManagementDataAccess;

public class SalaryManagementDbContext : DbContext
{
    private readonly IOptionsMonitor<ConnectionStrings> optionsMonitor;

    public SalaryManagementDbContext(DbContextOptions<SalaryManagementDbContext> options, IOptionsMonitor<ConnectionStrings> optionsMonitor) : base(options)
    {
        this.optionsMonitor = optionsMonitor;
    }

    public IDbConnection CreateConnection() => new SqlConnection(optionsMonitor.CurrentValue.DefaultConnection);
    public DbSet<Empoyee> Empoyees { get; set; }
    public DbSet<Salary> Salaries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}