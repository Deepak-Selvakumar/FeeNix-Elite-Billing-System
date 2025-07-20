namespace FmsEliteBilling.Api.Authorization;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using FmsEliteBilling.Api.Helpers;
using FmsEliteBilling.Model.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FmsEliteBilling.Model.AppNavigationModel;  

public interface IJwtUtils
{
    public string GenerateJwtToken(UserInformation user);
    public int? ValidateJwtToken(string token);
    public RefreshToken GenerateRefreshToken(string ipAddress); 
}

public class JwtUtils : IJwtUtils
{
    private DataContext _context;
    private readonly AppSettings _appSettings;

    public JwtUtils(
        DataContext context,
        IOptions<AppSettings> appSettings)
    {
        _context = context;
        _appSettings = appSettings.Value;
    }

    public string GenerateJwtToken(UserInformation user)
    { 
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor 
        {
            Subject = new ClaimsIdentity(new[] { 
                new Claim("id", user.UserSysId.ToString()),
                new Claim("FirstName",user.UserName.ToString()),
                new Claim ("UserId",user.UserID.ToString()),
                new Claim("SiteId",user.DefaultSiteId.ToString()),
                new Claim("StaffId",user.Staffid.ToString()),
                new Claim("AccessLevel",user.AccessLevel.ToString()),
                new Claim("loginHID", user.loginHID.ToString()),
                new Claim("ToApps",user.ToApps.ToString()),
                new Claim("RandomNumber",user.RandNo.ToString()),
                new Claim("CurrentDateTime", DateTime.Now.ToString("o"))
                }
                ),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


    public int? ValidateJwtToken(string token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
 
            return userId;
        }
        catch
        { 
            return null;
        }
    }

    public RefreshToken GenerateRefreshToken(string ipAddress)
    {
        var refreshToken = new RefreshToken
        {
            Token = getUniqueToken(),  
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

        return refreshToken;

        string getUniqueToken()
        { 
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)); 
            
            return token;
        }
    }
 
}