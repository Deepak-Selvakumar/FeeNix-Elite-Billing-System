namespace FmsEliteBilling.Api.Helpers;
using Microsoft.EntityFrameworkCore;
using FmsEliteBilling.Data.BusinessLayer;
using FmsEliteBilling.Model.AppNavigationModel;
using FmsEliteBilling.Model.Entities;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.SqlServer;


public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserToken> UserTokens{get;set;}

    private readonly AESSecurity_BLL _security;

    public DataContext(AESSecurity_BLL security)
    {
        _security = security;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var data=Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                 
                   
                        string clientId = configuration["OpenAPIClientId"].Replace("-","");
                        string DecryptString = configuration.GetSection("ConnectionStrings").GetSection("FEEDev").Value;
                        string FinalValue = Regex.Unescape(_security.Decrypt(DecryptString,clientId));
                        var connectionString = FinalValue;
                        optionsBuilder.UseSqlServer(connectionString);
                 
            
        }
    }
}