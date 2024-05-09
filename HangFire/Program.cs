using Hangfire;
using HangFire.Application;
using HangFire.Application.Contracts;
using HangFire.Data.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureServices();

builder.Services.Configure<MailConfig>(builder.Configuration.GetSection("MailConfig"));

builder.Services.AddHangfire(opt =>
{
    opt.UseSqlServerStorage(builder.Configuration[ "DBConnectionString" ]);
});

builder.Services.AddHangfireServer();

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration[ "DBConnectionString" ]);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
