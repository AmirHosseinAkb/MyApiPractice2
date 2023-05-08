using System.Text;using Common;
using Data;
using Data.Contracts;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Services;
using WebFramework.Configurations;
using WebFramework.MiddleWares;

var builder = WebApplication.CreateBuilder(args);
SiteSettings _siteSettings=builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();

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
builder.Services.AddScoped<IJwtService,JwtService>();
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(nameof(SiteSettings)));

#endregion

builder.Services.AddJWTAuthentication(_siteSettings);

var app = builder.Build();

app.UseCustomExceptionHandlerMiddleware();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
