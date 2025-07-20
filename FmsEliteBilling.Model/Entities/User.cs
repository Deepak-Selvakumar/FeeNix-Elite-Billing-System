namespace FmsEliteBilling.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
[Table("UserMaster")]

public class User
{
    [Column("UserSysId",TypeName = "BIGINT")]
    public long Id { get; set; }
    [Column("User_FirstName",TypeName = "VARCHAR")]
    [MaxLength(200)]
    public string? FirstName { get; set; }
    [Column("User_LastName",TypeName = "VARCHAR")]
    [MaxLength(200)]
    public string? LastName { get; set; }
    [Column("EmailId",TypeName = "VARCHAR")]
     [MaxLength(200)]
    public string? Username { get; set; }
    [Column("USERID",TypeName = "VARCHAR")]
     [MaxLength(200)]
    public string? UserID{get;set;}

   
   [Column("StaffId",TypeName = "INT")]
    public decimal? Staffid{get;set;}
    [Column("Default_SiteID",TypeName = "INT")]
    
    
    public decimal? DefaultSiteId{get;set;}
    [Column("OrgID",TypeName = "INT")]
   
    
    public decimal? OrgId{get;set;}

    [Column("AccessLevel",TypeName = "INT")]
   
    
    public decimal? AccessLevel{get;set;}
   

    [NotMapped]
    [JsonIgnore]
    public string? PasswordHash { get; set; }
    [NotMapped]
    [JsonIgnore]
    public List<RefreshToken>? RefreshTokens { get; set; }
}