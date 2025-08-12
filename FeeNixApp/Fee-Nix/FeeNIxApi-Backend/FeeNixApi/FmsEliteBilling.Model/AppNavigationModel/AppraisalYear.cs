using Newtonsoft.Json;
namespace FmsEliteBilling.Model.AppNavigationModel
{
    public class AppraisalYear
    {
        [JsonProperty(PropertyName = "AcYear")]
        public int? YearName{get;set;}
		
		
		[JsonProperty(PropertyName = "Academic_year")]
        public string? YearValue{get;set;}
    }
}