using Microsoft.EntityFrameworkCore;
using TimeWaster.Core;
using TimeWaster.Core.Services;
using TimeWaster.Data;
using TimeWaster.Data.Intervals.Repositories;
using TimeWaster.Data.Users.Repositories;
using TimeWaster.Web.Utils;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TimeWasterDbContext>(options =>
{
    options.UseNpgsql("User Id=postgres;Password=qwerty1234;Host=localhost;Port=5432;Database=timewaster;");
});
builder.Services.AddScoped<IIntervalsRepository, IntervalsPostgreSqlRepository>();
builder.Services.AddScoped<IUsersRepository, UsersPostgreSqlRepository>();
builder.Services.AddScoped<IntervalService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeToUtcConverter());
} );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();