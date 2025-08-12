using Newtonsoft.Json;
namespace FmsEliteBilling.Model.AppNavigationModel
{
    public class YearfetchList
    {
        [JsonProperty(PropertyName ="YEAR_NO")]
        public int? YearNumber{get;set;}

        public string? appraisalYear{get;set;}

        [JsonProperty(PropertyName = "YEAR_START")]
        public DateTime? YearStart{get;set;}

        [JsonProperty(PropertyName = "YEAR_END")]
        public DateTime? YearEnd{get;set;}

        [JsonProperty(PropertyName = "YEAR_ACTIVEFLAG")]
        public char?  YearActiveFlag{get;set;}
    }
}