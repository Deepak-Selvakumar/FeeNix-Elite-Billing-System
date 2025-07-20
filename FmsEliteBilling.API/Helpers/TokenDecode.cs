using System.IdentityModel.Tokens.Jwt;
using FmsEliteBilling.Model.AppNavigationModel;

namespace FmsEliteBilling.Api.Helpers
{
    public class TokenDecode
    {
         public UserInformation TokenDecodeFromString(string token)
        {
            var decodetoken = new JwtSecurityToken(jwtEncodedString: token);

            //AdminCred , SiteId, StaffId

            UserInformation user = new()
            {
                UserSysId = Int64.Parse(decodetoken.Claims.First(c => c.Type == "id").Value),
                UserName = decodetoken.Claims.First(c => c.Type == "FirstName").Value,
                UserID = decodetoken.Claims.First(c => c.Type == "UserId").Value, 
                AccessLevel = Decimal.Parse(decodetoken.Claims.First(c => c.Type == "AccessLevel").Value),
                Currdatetime = decodetoken.Claims.First(c => c.Type == "CurrentDateTime").Value,
                loginHID = decodetoken.Claims.First(c => c.Type == "loginHID").Value,                 
                ToApps = decodetoken.Claims.First(c => c.Type == "ToApps").Value,
                RandNo = decodetoken.Claims.First(c => c.Type == "RandomNumber").Value,
                
            };
            return user;
        }
    }
}