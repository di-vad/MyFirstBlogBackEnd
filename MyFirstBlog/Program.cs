using Microsoft.EntityFrameworkCore;
using MyFirstBlog.Helpers;
using MyFirstBlog.Services;

var  MyAllowLocalhostOrigins = "_myAllowLocalhostOrigins";

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("CNSTR => " + builder.Configuration.GetConnectionString("DefaultConnection"));
var services = builder.Services;
var env = builder.Environment;

// Add services to the container.

services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


services.AddCors(policyBuilder => {
    policyBuilder.AddPolicy( MyAllowLocalhostOrigins,
        policy => {
            policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
        });
});

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped<IPostService, PostService>();

var app = builder.Build();

var scope = app.Services.CreateScope();
await DatabaseHelper.ManageMigrationsAsync(scope.ServiceProvider);

// Configure the HTTP request pipeline.
if (env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(MyAllowLocalhostOrigins);
}

if (env.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
