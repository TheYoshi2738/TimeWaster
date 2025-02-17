using TimeWaster.Core;
using TimeWaster.Core.Services;
using TimeWaster.Core.Services.IntervalProcessing;
using TimeWaster.Core.Services.UserProcessing;
using TimeWaster.Data;
using TimeWaster.Data.Intervals.Repositories;
using TimeWaster.Data.Users.Repositories;
using TimeWaster.Web.HostedServices;
using TimeWaster.Web.Utils;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<MigrationHostedService>();

builder.Services.AddDbContext<TimeWasterDbContext>();

builder.Services.AddScoped<IIntervalsRepository, IntervalsPostgreSqlRepository>();
builder.Services.AddScoped<IUsersRepository, UsersPostgreSqlRepository>();
builder.Services.AddScoped<IIntervalService, IntervalService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new DateTimeToUtcConverter()));

var app = builder.Build();

if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();