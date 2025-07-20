using Newtonsoft.Json;
namespace FmsEliteBilling.Model.AppNavigationModel
{

    #nullable enable
    public class UserInformation
    {
        public string? Currdatetime;

        [JsonProperty(PropertyName = "UserSysID")]
        public decimal? UserSysId{get;set;}
 
        [JsonProperty(PropertyName = "UserID")]
        public string? UserID{get;set;}

        [JsonProperty(PropertyName = "DefaultSiteId")]
        public decimal DefaultSiteId{get;set;}
 
        [JsonProperty(PropertyName = "staffid")]
        public decimal? Staffid{get;set;}

        [JsonProperty(PropertyName = "UserName")]
        public string? UserName{get;set;}
         
        [JsonProperty(PropertyName = "Category")]
        public string? Category{get;set;}
           
        [JsonProperty(PropertyName = "ADSServerName")]
        public string? ADSServerName{get;set;}         


        [JsonProperty(PropertyName = "DisplayName")]
        public string? DisplayName{get;set;} 

        [JsonProperty(PropertyName = "role_names")]
        public string? RoleNames{get;set;}

        [JsonProperty(PropertyName = "roleids")]
        public string? RoleIDs{get;set;} 


        [JsonProperty(PropertyName = "ToApps")]
        public string? ToApps{get;set;}


        [JsonProperty(PropertyName = "PersonaID")]
        public decimal? PersonaID{get;set;}
         
        [JsonProperty(PropertyName = "loginHID")]
        public string? loginHID{get;set;}
  
        [JsonProperty(PropertyName = "AccessLevel")]
        public decimal? AccessLevel{get;set;}

        [JsonProperty(PropertyName = "PersonaName")]
        public string? PersonaName{get;set;}

        [JsonProperty(PropertyName = "OrgID")]
        public decimal? OrgId{get;set;}


        [JsonProperty(PropertyName = "Admincred")]
        public string? Admincred{get;set;}

        [JsonProperty(PropertyName = "SessionYearTxt")]
        public string? SessionYearTxt{get;set;} 
        
  
         [JsonProperty(PropertyName = "RandNo")]
        public string? RandNo{get;set;}

        
         [JsonProperty(PropertyName = "logintype")]
        public string? logintype{get;set;}
  
    }
}