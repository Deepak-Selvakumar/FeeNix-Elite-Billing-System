using FmsEliteBilling.Api.Authorization;
using FmsEliteBilling.Api.Helpers;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FmsEliteBilling.Data.Oracle;
using FmsEliteBilling.Data.BusinessLayer;
using Serilog.Formatting.Compact;
using Serilog.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog.Events; 
using FmsEliteBilling.Data.Repository;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using FmsEliteBilling.Model;
using FmsEliteBilling.API.Services;
using FmsEliteBilling.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
  

// ---  BEGIN ---
string currentDate = DateTime.Now.ToString("yyyyMM");
string currentDirectory = Directory.GetCurrentDirectory();
string logsDirectory = Path.Combine(Directory.GetParent(Directory.GetParent(currentDirectory).ToString()).ToString(), "0");
if (!Directory.Exists(logsDirectory))
{
    Directory.CreateDirectory(logsDirectory);
}
Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Warning()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
           .Enrich.FromLogContext()
           .WriteTo.SQLite(Path.Combine(logsDirectory, "logs_" + currentDate + ".db"), restrictedToMinimumLevel: LogEventLevel.Warning)
           .CreateLogger();



builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(new RenderedCompactJsonFormatter())
        .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
        .WriteTo.SQLite(Path.Combine(logsDirectory, "logs_" + currentDate + ".db"), restrictedToMinimumLevel: LogEventLevel.Warning)
        );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
{
    var services = builder.Services;
    var env = builder.Environment;
    services.AddDbContext<DataContext>();
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    // configure DI for application services
    services.AddScoped<IJwtUtils, JwtUtils>();
}
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
                          policy =>
                          {
                              policy.SetIsOriginAllowedToAllowWildcardSubdomains()
                              .WithOrigins("http://localhost")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod()
                                                  .AllowCredentials()
                                                  .Build();
                          });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Fee Nix Billing Api",
        Description = "A .NET 9 Web API for managing fee operations",
        TermsOfService = new Uri("https://www.google.com/"),
        Contact = new OpenApiContact
        {
            Name = "Contact Us",
            Url = new Uri("https://www.google.com/")
        },
        License = new OpenApiLicense
        {
            Name = "Privacy Policy",
            Url = new Uri("https://www.google.com/")
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
        });
});
 
builder.Services.AddTransient<TokenDecode>();
builder.Services.AddTransient<AESSecurity_BLL>();
builder.Services.AddTransient<AppNavigation_BLL>();
builder.Services.AddScoped<LogRespository>(sp => new LogRespository("Data Source=" + Path.Combine(logsDirectory, "logs_" + currentDate + ".db")));
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddSingleton<IConnectionProvider, ConnectionProvider>(); 



AESSecurity_BLL security_BLL = new AESSecurity_BLL();
string clientId ="0b60670ac98749f7b0587b09e7613a51";
string DecryptString = builder.Configuration.GetSection("ConnectionStrings").GetSection("SELDB").Value!;
string FinalValue = "Data Source=DESKTOP-59JF4FA\\SQLEXPRESS;Initial Catalog=REFLEX2025;Integrated Security=True";
string ConnectionString = FinalValue;


builder.Services.AddHealthChecks().AddSqlServer(ConnectionString, tags: new[] { "database" }).AddCheck<IHealthCheck>("My Health Checks", tags: new[] { "custom" });
builder.Services.AddControllers();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            c.OAuthClientId(app.Configuration["OpenAPIClientId"]);
            c.RoutePrefix = string.Empty;
            c.OAuthUsePkce();
            c.OAuthScopeSeparator(" ");
            c.InjectStylesheet("/swagger-ui/custom.css");
            c.InjectJavascript("/swagger-ui/custom.js");
        });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            c.OAuthClientId(app.Configuration["OpenAPIClientId"]);
            c.OAuthUsePkce();
            c.OAuthScopeSeparator(" ");
            c.InjectStylesheet("/swagger-ui/custom.css");
            c.InjectJavascript("/swagger-ui/custom.js");
        });
}
app.UseStaticFiles();
app.UseRouting();
app.UseCors("MyAllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/custom", new HealthCheckOptions
{
    Predicate = reg => reg.Tags.Contains("custom"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Use(async (httpContext, next) =>
    {
        var userName = httpContext.User.Identity.IsAuthenticated ? httpContext.User.FindFirst("unique_name")?.Value : "Guest"; //Gets user Name from user Identity  
        LogContext.PushProperty("User", userName);//User Email Configure in Logging
        await next.Invoke();
    }
);
app.MapControllers();
app.Run();