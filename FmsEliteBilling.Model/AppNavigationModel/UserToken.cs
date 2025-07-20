using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace FmsEliteBilling.Model.AppNavigationModel
{
    #nullable enable
    [Table("USER_TOKEN")]
    public class UserToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         [Column("TOKEN_ID",TypeName = "BIGINT")]
        
        public long TokenId{get;set;}
         [Column("TOKEN_VALUE",TypeName = "VARCHAR")]
         [MaxLength(5000)]
        public string? TokenValue{get;set;}
         [Column("TOKEN_USERSYSID",TypeName = "BIGINT")]
        public long UserSysId{get;set;}

          [Column("LASTUPDATED_ON",TypeName = "DATETIME")]
        public DateTime UpdatedOn{get;set;}
        [Column("TOKEN_ISREVOKED",TypeName = "CHAR")]
         [MaxLength(1)]
        public string? IsRevoked{get;set;} 
         [Column("TOKEN_ISACTIVE",TypeName = "CHAR")]
         [MaxLength(1)]
        public string? IsActive{get;set;} 
    }
}