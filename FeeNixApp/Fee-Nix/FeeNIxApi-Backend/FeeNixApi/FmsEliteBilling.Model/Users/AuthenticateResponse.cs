namespace FmsEliteBilling.Model.Users;

using System.Text.Json.Serialization;
using FmsEliteBilling.Model.AppNavigationModel;

public class AuthenticateResponse
{
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? JwtToken { get; set; }
    public string? UserID { get; set; }
    public decimal? Staffid { get; set; }
    public decimal? DefaultSiteId { get; set; }
    public decimal? OrgId { get; set; }
    public decimal? AccessLevel { get; set; }
    public string? Admincred { get; set; }
    public string? loginHID { get; set; }
    public string? SessionYearTxt { get; set; }
    public string ToApps { get; set; }
    public string? RandomNumber { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }

    public AuthenticateResponse(UserInformation user, string jwtToken, string refreshToken)
    {
        Id = (long)user.UserSysId!;
        // FirstName = user.FirstName;
        // LastName = user.LastName;
        Username = user.UserName!;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
        UserID = user.UserID;
        OrgId = user.OrgId;
        Staffid = user.Staffid;
        DefaultSiteId = user.DefaultSiteId;
        AccessLevel = user.AccessLevel;
        Admincred = user.Admincred;
        loginHID = user.loginHID;
        SessionYearTxt = user.SessionYearTxt;
        ToApps = user.ToApps!;
        RandomNumber = user.RandNo;
    }

    // public AuthenticateResponse(Entities.User user, string jwtToken, string token)
    // {
    //     JwtToken = jwtToken;
    // }
}