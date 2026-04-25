    using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using ToDoApi.API.Middlewares;
using ToDoApi.Application.Interfaces;
using ToDoApi.Application.Services;
using ToDoApi.Domain.Interfaces;
using ToDoApi.Infrastructure.Data;
using ToDoApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

var secretKey = builder.Configuration["Jwt:SecretKey"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "JwtBearer";
})  











Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();


builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); 
builder.Services.AddProblemDetails();






builder.Host.UseSerilog();




var app = builder.Build();  

app.UseExceptionHandler();
app.UseSerilogRequestLogging();



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
