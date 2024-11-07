using RestApp;
using RestApp.Entities;
using RestApp.Service;
using NLog;
using NLog.Web;
using RestApp.Middleware;
using RestApp.Controllers;
using RestApp.Services;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using RestApp.Models;
using RestApp.Models.Validators;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using RestApp.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000", "http://localhost:4200", "http://localhost:8080")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader();
                      });
});

// Usuniêcie domyœlnych providerów logowania
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

// Ustawienie NLog jako providera logowania
builder.Host.UseNLog();

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler<Post>>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler<Comment>>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler<EditUser>>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler<EditPassword>>();

builder.Services.AddScoped<AppSeeder>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IAccountService, AccountService> ();

builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<EditPassword>, EditPasswordValidator>();
builder.Services.AddScoped<IValidator<EditUser>, EditUserValidator>();



//Authentication
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});

//Db Configuration     
builder.Services.AddDbContext<AppDbContext>
     (opctions =>
     {
         opctions.UseSqlServer(builder.Configuration.GetConnectionString("AppDbConnection"));
     });

//Add Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", builder =>
    builder.AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin()
    );
});

//Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rest Api", Version = "v1" });
});

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<AppSeeder>();


seeder.Seed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("FrontEndClient");

app.UseAuthorization();

app.MapControllers();

app.Run();
