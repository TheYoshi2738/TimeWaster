using Microsoft.EntityFrameworkCore;
using TimeWaster.Core;
using TimeWaster.Core.Services;
using TimeWaster.Data;
using TimeWaster.Data.Intervals.Repositories;
using TimeWaster.Data.Users.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TimeWasterDbContext>(options =>
{
    options.UseInMemoryDatabase("TimeWaster");
});
builder.Services.AddScoped<IIntervalsRepository, IntervalsInMemoryRepository>();
builder.Services.AddScoped<IUsersRepository, UsersInMemoryRepository>();
builder.Services.AddScoped<IntervalService>();
builder.Services.AddScoped<UserService>();

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