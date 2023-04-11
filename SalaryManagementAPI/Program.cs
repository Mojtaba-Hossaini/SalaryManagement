using SalaryManagementApplication;
using SalaryManagementApplication.Config;
using SalaryManagementApplication.Contracts;
using SalaryManagementApplication.Services;
using SalaryManagementDataAccess;
using SalaryManagementDataAccess.Configs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddXmlSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SalaryManagementDbContext>();
builder.Services.AddScoped<QueryContext>();
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<TaxConfig>(builder.Configuration.GetSection("TaxConfig"));
builder.Services.AddScoped<ICalculateSalaryPayment, CalculateSalaryPayment>();
builder.Services.AddScoped<ISalaryManagementRepository, SalaryManagementRepository>();
//builder.Services.AddScoped<IApplicationService, ApplicationService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
