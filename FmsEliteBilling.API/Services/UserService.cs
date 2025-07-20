using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FmsEliteBilling.Api.Helpers;
using FmsEliteBilling.Data.Repositories;
using k8s.KubeConfigModels;
using Microsoft.Extensions.Options;
using FmsEliteBilling.Api.Authorization;
using FmsEliteBilling.Data.Oracle; 
using FmsEliteBilling.Model.AppNavigationModel;
using FmsEliteBilling.Model.Entities;
using FmsEliteBilling.Model.Users;
using User = FmsEliteBilling.Model.Entities.User;

namespace FmsEliteBilling.API.Services
{
   
public interface IUserService
{
    AuthenticateResponse Authenticate(UserInformation model, string ipAddress);
    AuthenticateResponse RefreshToken(string token, string ipAddress);
    void RevokeToken(string token, string ipAddress);
    IEnumerable<User> GetAll();
    User GetById(int id);
}

public class UserService : IUserService
{
    private DataContext _context;
    private IJwtUtils _jwtUtils;
    private readonly AppSettings _appSettings;

    private readonly IConnectionProvider _conn;
    
    private readonly TokenDecode _tokendecode;

    public UserService(
        DataContext context,
        IJwtUtils jwtUtils,
        IOptions<AppSettings> appSettings,IConnectionProvider conn,TokenDecode tokendecode)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _appSettings = appSettings.Value;
     
        _conn=conn;
        _tokendecode=tokendecode;
    }

    public AuthenticateResponse Authenticate(UserInformation model, string ipAddress)
    {
        
    //     var user = _context.Users.SingleOrDefault(x => x.UserID == model.Username);

    //     // validate
    //    // if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
    //    if (user == null)
    //    {
    //         return null;
    //    }

        // authentication successful so generate jwt and refresh tokens
        var jwtToken = _jwtUtils.GenerateJwtToken(model);
         //var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
        // user.RefreshTokens.Add(refreshToken);

        // // remove old refresh tokens from user
        // removeOldRefreshTokens(user);

        // save changes to db
       // _context.Update(user);
        // _context.SaveChanges();

        return new AuthenticateResponse(model, jwtToken, null);
    }

    public AuthenticateResponse RefreshToken(string token, string ipAddress)
    {

        var userdet=_tokendecode.TokenDecodeFromString(token);
       
        var user = getUserByRefreshToken(token,(long)userdet.UserSysId);

        RefreshToken   refreshtoken=new RefreshToken();
        refreshtoken.Token=user.TokenValue;
        refreshtoken.Id=user.TokenId;
        refreshtoken.CreatedBy=user.UserSysId;
        refreshtoken.Created=user.UpdatedOn;
        refreshtoken.IsRevoked=user.IsRevoked;
        refreshtoken.IsActive=user.IsActive;
         var newRefreshToken = rotateRefreshToken(refreshtoken, ipAddress);

        if (refreshtoken.IsRevoked=="Y")
        {
           
            UserToken usertoken=new UserToken();
            usertoken.TokenValue=newRefreshToken.Token;
            usertoken.UserSysId=newRefreshToken.CreatedBy;
            // revoke all descendant tokens in case this token has been compromised
            revokeDescendantRefreshTokens(refreshtoken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
             AppNavigationRepository<UserInformation> _appuser=new AppNavigationRepository<UserInformation>();
            var userresult=_appuser.UserTokenInsertion(_conn,"D",usertoken.TokenValue,usertoken.UserSysId);
        }

        if (!(refreshtoken.IsActive=="Y"))
            throw new AppException("Invalid token");

        // replace old refresh token with a new one (rotate token)
        



       
        
        
        AppNavigationRepository<UserInformation> _app=new AppNavigationRepository<UserInformation>();
        var result=_app.MenuNavigationIns(_conn,userdet.RandNo,(int)userdet.AccessLevel);
        result.value[0].RandNo=userdet.RandNo;
         
        var jwtToken = _jwtUtils.GenerateJwtToken(result.value[0]);
        AppNavigationRepository<UserInformation> _appusertoken=new AppNavigationRepository<UserInformation>();
        var userresult1=_appusertoken.UserTokenInsertion(_conn,"I",jwtToken,refreshtoken.CreatedBy);

        return new AuthenticateResponse(result.value[0], jwtToken, newRefreshToken.Token);
    }

    public void RevokeToken(string token, string ipAddress)
    {
        var userdet=_tokendecode.TokenDecodeFromString(token);
        
        var user = getUserByRefreshToken(token,(long)userdet.UserSysId);
       

        if (user is null)
            throw new AppException("Invalid token");

        RefreshToken   refreshtoken=new RefreshToken();
        refreshtoken.Token=user.TokenValue;
        refreshtoken.Id=user.TokenId;
        refreshtoken.CreatedBy=user.UserSysId;
        refreshtoken.Created=user.UpdatedOn;
        refreshtoken.IsRevoked=user.IsRevoked;
        refreshtoken.IsActive=user.IsActive;

        // revoke token and save
        revokeRefreshToken(refreshtoken, ipAddress, "Revoked without replacement");
        // _context.Update(user);
        // _context.SaveChanges();
    }

    public IEnumerable<User> GetAll()
    {
        return (IEnumerable<User>)_context.Users;
    }

    public User GetById(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }

    // helper methods

    private UserToken getUserByRefreshToken(string token,long userId)
    {
        var user = _context.UserTokens.SingleOrDefault(u => u.TokenValue==token && u.UserSysId==userId && u.IsActive=="Y");

        if (user == null)
            throw new AppException("Invalid token");

        return user;
    }

    private RefreshToken rotateRefreshToken(RefreshToken refreshToken, string ipAddress)
    {
        var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
        revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    private void removeOldRefreshTokens(User user)
    {
        // remove old inactive refresh tokens from user based on TTL in app settings
        user.RefreshTokens.RemoveAll(x => 
            x.IsActive=="N" && 
            x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
    }

    private void revokeDescendantRefreshTokens(RefreshToken refreshToken, UserToken user, string ipAddress, string reason)
    {
        // recursively traverse the refresh token chain and ensure all descendants are revoked
        if(!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
        {
           
            // if (refreshToken.IsActive)
                revokeRefreshToken(refreshToken, ipAddress, reason);
            // else
            //     revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
        }
    }

    private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
        AppNavigationRepository<UserInformation> _appusertoken=new AppNavigationRepository<UserInformation>();
        var userresult1=_appusertoken.UserTokenInsertion(_conn,"D",token.Token,token.CreatedBy);

    }
}
}