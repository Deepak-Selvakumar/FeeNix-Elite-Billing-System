using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FmsEliteBilling.Model.AppNavigationModel
{
    public class VaidTokenList
    {
         [JsonProperty(PropertyName = "TOKEN_ID")]
        public int TokenID{get;set;}

        [JsonProperty(PropertyName = "TOKEN_VALUE")]
        public string? TokeValue{get;set;}

        [JsonProperty(PropertyName = "TOKEN_USERSYSID")]
        public int Useid{get;set;}

        [JsonProperty(PropertyName = "LASTUPDATED_ON")]
        public string? UpdatedOn{get;set;}

        [JsonProperty(PropertyName = "TOKEN_ISREVOKED")]
        public string? TokenIsRevoked{get;set;}

        [JsonProperty(PropertyName = "TOKEN_ISACTIVE")]
        public string? TokenISActive{get;set;}
 

        					 
    }
}