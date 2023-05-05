using Data;
using Data.Contracts;
using Data.Repositories;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebFramework.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DBContext

builder.Services.AddDbContext<MyApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyApiConnection")));

#endregion

#region IOC

builder.Services.AddScoped<IUserRepository, UserRepository>();

#endregion

var app = builder.Build();

app.UseCustomExceptionHandlerMiddleware();
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
