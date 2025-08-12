
using FmsEliteBilling.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using FmsEliteBilling.API.Services;
using FmsEliteBilling.Data.BusinessLayer;
using FmsEliteBilling.Data.Oracle;
using FmsEliteBilling.Data.Repositories;
using FmsEliteBilling.Model.AppNavigationModel;
using FmsEliteBilling.Model.Users;
using FmsEliteBilling.Model.Response;
using FmsEliteBilling.Model.AppNavigationModel.PostModel;
using Serilog; 
using FmsEliteBilling.API.Helpers;

namespace FmsEliteBilling.API.Controllers
{
#nullable enable
  // [Authorize]
  [ApiController]
  [Route("[controller]")]
  [Produces("application/json")]
  public class ReflexNavigationController : ControllerBase
  {
    private IUserService _userService;

    private readonly IConnectionProvider _configuration;
    private readonly TokenDecode _tokendecode;
    private readonly AppNavigation_BLL _appnavigation;

    public ReflexNavigationController(IConnectionProvider configuration, IUserService userService, TokenDecode tokendecode, AppNavigation_BLL appnavigation)
    {
      _configuration = configuration;
      _userService = userService;

      _tokendecode = tokendecode;
      _appnavigation = appnavigation;

    }

    [HttpGet("MenuNavigation/{randomnumber}/{accesslevel}")]
    public ActionResult GetUserInformation([FromRoute] string randomnumber, [FromRoute] int accesslevel)
    {
      try

      {
        AppNavigationRepository<UserInformation> _appnavig = new AppNavigationRepository<UserInformation>();
        var result = _appnavig.MenuNavigationIns(
            _configuration,
            randomnumber,
            accesslevel);
        if (result.value!.Count > 0)
        { 
          result.value[0].RandNo = randomnumber;
          var response = _userService.Authenticate(result.value[0], ipAddress());
          Console.WriteLine(response.JwtToken);
          var tokenupd = _appnavig.UserTokenInsertion(_configuration, "I", response.JwtToken!, response.Id);
          if (response == null)
          {
            return new JsonResult("Token Generation Issue")
            {
              StatusCode = StatusCodes.Status200OK // Status code here 
            };
          } 
          return Ok(response);
        }
        return new JsonResult(result.response!.ErrorMessage)
        {
          StatusCode = StatusCodes.Status200OK // Status code here 
        };

      }
      catch (Exception ex)
      {
        Log.Error(ex, ex.Message);
        return new JsonResult(ex.Message)
        {
          StatusCode = StatusCodes.Status500InternalServerError // Status code here 
        };
      }
    }
    [HttpGet("SiteIdUpdate/{siteId}")]
    public ActionResult SiteIdUpdate([FromRoute] decimal siteId)
    {
      try
      {
        AppNavigationRepository<UserInformation> _appnavig = new AppNavigationRepository<UserInformation>();
        var tokenvalue = Request.Headers["Authorization"];
        string token = tokenvalue[0].Split("Bearer ")[1];
        UserInformation userdet = _tokendecode.TokenDecodeFromString(token);
        userdet.DefaultSiteId = siteId;
        var response = _userService.Authenticate(userdet, ipAddress());
        Console.WriteLine(response.JwtToken);
        var tokenupd = _appnavig.UserTokenInsertion(_configuration, "I", response.JwtToken!, response.Id);
        if (response == null)
        {
          return new JsonResult("Token Generation Issue")
          {
            StatusCode = StatusCodes.Status200OK // Status code here 
          };
        }
        return Ok(response);
      }
      catch (Exception ex)
      {
        Log.Error(ex, ex.Message);
        return new JsonResult(ex.Message)
        {
          StatusCode = StatusCodes.Status500InternalServerError // Status code here 
        };
      }

    }

    [HttpGet("PersonaList/{LogSysId}/{AccessLevel}/{siteid}/{Type}")]
    public ActionResult<GetResponse<PersonaList>> GetPersonaList([FromRoute] int LogSysId, int AccessLevel, int siteid, string Type)
    {
      try
      {

        var tokenvalue = Request.Headers["Authorization"];

        string token = (tokenvalue[0].Split("Bearer "))[1];
        var userdet = _tokendecode.TokenDecodeFromString(token);

        Validatetoken _validatetoken = new Validatetoken();

        var validationResponse = _validatetoken.Validation(userdet);
        if (validationResponse.StatusCode == StatusCodes.Status500InternalServerError)
        {
          return validationResponse;
        }

        AppNavigationRepository<PersonaList> _appnavig2 = new AppNavigationRepository<PersonaList>();

        var result = _appnavig2.GetPersonaList(_configuration, LogSysId, AccessLevel, siteid, Type);

        return Ok(result);

      }
      catch (Exception ex)
      {
        Log.Error(ex, ex.Message);
        return new JsonResult("Failed to fetch data")
        {
          StatusCode = StatusCodes.Status500InternalServerError // Status code here 
        };
      }

    }


    [HttpPost("SaveMenuNavigation")]
    public ActionResult PostMenuNavigation([FromBody] PostMenuNavigation menu)
    {
      try
      {
        AppNavigationRepository<PostResponse> _appnavig = new AppNavigationRepository<PostResponse>();

        var result = _appnavig.PostMenuNavigation(_configuration, menu);

        return Ok(result);

      }
      catch (Exception ex)
      {
        Log.Error(ex, ex.Message);
        return new JsonResult("Failed to Save data")
        {
          StatusCode = StatusCodes.Status500InternalServerError // Status code here 
        };
      }

    }




    [HttpPost("refresh-token")]
    public IActionResult RefreshToken(RevokeTokenRequest model)
    {
      try
      {
        string token = model.Token!;
        //var refreshToken = Request.Cookies["refreshToken"];
        var response = _userService.RefreshToken(token, ipAddress());

        setTokenCookie(response.RefreshToken);
        return Ok(response);
      }
      catch (Exception ex)
      {
        Log.Error(ex, ex.Message);
        return new JsonResult("Invalid Token")
        {
          StatusCode = StatusCodes.Status500InternalServerError // Status code here 
        };
      }

    }


    [HttpPost("revoke-token")]
    public IActionResult RevokeToken(RevokeTokenRequest model)
    {
      try
      {
        // accept refresh token in request body or cookie
        var token = model.Token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
          return BadRequest(new { message = "Token is required" });

        _userService.RevokeToken(token, ipAddress());
        return Ok(new { message = "Token revoked" });

      }
      catch (Exception ex)
      {
        Log.Error(ex, ex.Message);
        return new JsonResult("Invalid Token/User do not have access")
        {
          StatusCode = StatusCodes.Status500InternalServerError // Status code here 
        };
      }
    }

    private string ipAddress()
    {
      // get source ip address for the current request
      if (Request.Headers.ContainsKey("X-Forwarded-For"))
        return Request.Headers["X-Forwarded-For"];
      else
        return HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();
    }

    private void setTokenCookie(string token)
    {
      // append cookie with refresh token to the http response
      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(7)
      };
      Response.Cookies.Append("refreshToken", token, cookieOptions);
    }





    [HttpGet("MenuList/{MenuApps}/{PersonaId}")]
    public ActionResult<GetResponse<MenuList>> GetMenuListData([FromRoute] string MenuApps, int PersonaId)
    {
      try
      {


        var tokenvalue = Request.Headers["Authorization"];

        string token = (tokenvalue[0].Split("Bearer "))[1];
        var userdet = _tokendecode.TokenDecodeFromString(token);


        Validatetoken _validatetoken = new Validatetoken();

        var validationResponse = _validatetoken.Validation(userdet);
        if (validationResponse.StatusCode == StatusCodes.Status500InternalServerError)
        {
          return validationResponse;
        }



        AppNavigationRepository<MenuList> _appnavig = new AppNavigationRepository<MenuList>();
        var result = _appnavig.GetMenuList(_configuration, MenuApps, PersonaId, userdet);
        var menuresult = _appnavigation.GetFMSMenuList(result.value!);
        var resultmenu = _appnavigation.GetMenuListResult(result.value!);
        return Ok(resultmenu);

      }
      catch (Exception ex)
      {
        Log.Error(ex, ex.Message);
        return new JsonResult("Failed to fetch data")
        {
          StatusCode = StatusCodes.Status500InternalServerError // Status code here 
        };
      }
    }

  }
}