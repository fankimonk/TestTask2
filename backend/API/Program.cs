using Application.Interfaces;
using Application.Services;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Инициализация контекста БД
var connectionString = builder.Configuration.GetConnectionString(nameof(TestTask2Context));
builder.Services.AddDbContext<TestTask2Context>(
    options =>
    {
        options.UseSqlServer(connectionString);
    });

//Регистрация в dependecy injection сервисов
builder.Services.AddScoped<IFilesService, FilesService>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<ITableService, TableService>();

//Регистрация в dependecy injection репозиториев
builder.Services.AddScoped<IFilesRepository, FilesRepository>();
builder.Services.AddScoped<IBanksRepository, BanksRepository>();
builder.Services.AddScoped<IPeriodsRepository, PeriodsRepository>();
builder.Services.AddScoped<IClassesRepository, ClassesRepository>();
builder.Services.AddScoped<IGlobalTotalsRepository, GlobalTotalsRepository>();
builder.Services.AddScoped<IClassTotalsRepository, ClassTotalsRepository>();
builder.Services.AddScoped<IGroupsRepository, GroupsRepository>();
builder.Services.AddScoped<IGroupTotalsRepository, GroupTotalsRepository>();
builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
builder.Services.AddScoped<IBalanceSheetRecordsRepository, BalanceSheetRecordsRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
