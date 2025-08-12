using Newtonsoft.Json;
namespace FmsEliteBilling.Model.AppNavigationModel
{
    public class PersonaList
    {
        [JsonProperty(PropertyName = "PersonaID")]
        public int? PersonaID{get;set;}


        [JsonProperty(PropertyName = "PersonaName")]
        public string? PersonaName{get;set;}


        [JsonProperty(PropertyName = "PersonaAppName")]
        public string? PersonaAppName{get;set;}
		
		
		[JsonProperty(PropertyName = "AppIcon")]
        public string? AppIcon{get;set;}


        [JsonProperty(PropertyName = "AppsURL")]
        public string? AppsURL{get;set;}


        [JsonProperty(PropertyName = "NavigateURL")]
        public string? NavigateURL{get;set;}
    }
}