using Newtonsoft.Json;
namespace FmsEliteBilling.Model.AppNavigationModel
{
    public class MenuList
    {
        [JsonProperty(PropertyName = "TMenuId")]
        public long TMenuId{get;set;}

        [JsonProperty(PropertyName = "MenuId")]
        public long id{get;set;}

        [JsonProperty(PropertyName = "Menu_Name")]
        public string? name{get;set;}

        [JsonProperty(PropertyName = "Menu_Url")]
        public string? url{get;set;}

        [JsonProperty(PropertyName = "Menu_Level")]
        public int? MenuLevel{get;set;}

        [JsonProperty(PropertyName = "Parent_MenuId")]
        public long? ParentMenuId{get;set;}

        [JsonProperty(PropertyName = "Icon")]
        public string? Icon{get;set;}

        [JsonProperty(PropertyName = "Created_By")]
        public string? CreatedBy{get;set;}

        [JsonProperty(PropertyName = "Created_On")]
        public string? CreatedOn{get;set;}

        [JsonProperty(PropertyName = "Updated_By")]
        public string? UpdatedBy{get;set;}

        [JsonProperty(PropertyName = "Updated_On")]
        public string? UpdatedOn{get;set;}

        [JsonProperty(PropertyName = "DisplayOrder")]
        public int? DisplayOrder{get;set;}
        [JsonProperty(PropertyName = "PersonaId")]
        public int? PersonaId{get;set;}

        [JsonProperty(PropertyName = "PersonaDefault")]
        public char? PersonaDefault{get;set;}

        [JsonProperty(PropertyName = "SpecificSysRole")]
        public char? SpecificSysRole{get;set;}

        [JsonProperty(PropertyName = "SpecificBoard")]
        public char? SpecificBoard{get;set;}

        [JsonProperty(PropertyName = "SpecificOrg")]
        public char? SpecificOrg{get;set;}

         [JsonProperty(PropertyName = "MenuEnabled")]
        public char? MenuEnabled{get;set;}

        [JsonProperty(PropertyName = "MenuDescription")]
        public string? MenuDescription{get;set;}

        [JsonProperty(PropertyName = "SpecificAcFilters")]
        public char? SpecificAcFilters{get;set;}

        [JsonProperty(PropertyName = "FavMenu")]
        public char? FavMenu{get;set;}

       
        [JsonIgnore]

        public List<MenuList>? subMenu{get;set;}
        
        


    }
}