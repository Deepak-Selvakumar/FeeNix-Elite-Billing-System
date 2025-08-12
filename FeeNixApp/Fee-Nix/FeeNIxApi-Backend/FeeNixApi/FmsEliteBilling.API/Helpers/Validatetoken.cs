using Microsoft.AspNetCore.Mvc;
using FmsEliteBilling.Model.AppNavigationModel;

namespace FmsEliteBilling.API.Helpers
{
    public class Validatetoken
    {

        public JsonResult Validation(UserInformation userdet)
        {
            var cdate = DateTime.Now - DateTime.Parse(userdet.Currdatetime);
            if (userdet.ToApps != "SELBILL")
            {
                return new JsonResult("Token Invalid for SEL-FmsEliteBilling Personna")
                {
                    StatusCode = StatusCodes.Status500InternalServerError // Status code here 
                };
            }
            else
            {
                // If none of the conditions are met, return a success response
                return new JsonResult("Token is valid")
                {
                    StatusCode = StatusCodes.Status200OK // Status code here 
                };
            }
        }
    }
}