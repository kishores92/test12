using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using GreenPOS.Common;
using GreenPOS.Models;

namespace GreenPos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //private readonly IConfiguration _config;
        //private IAuthService _service;
        //public AuthController(IConfiguration config, IAuthService service)
        //{
        //    _config = config;
        //    _service = service;
        //}

        //[HttpPost]
        //public async Task<ApiResponse<UserViewModel>> AuthenticateUser([FromBody] LoginViewModel vm)
        //{
        //    var result = await _service.Authenticate(vm);

        //    if (result.Code != 1)
        //        return result;

        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.UniqueName, result.Data.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));

        //    var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"]
        //                                    , audience: _config["Jwt:Audience"]
        //                                    , claims: claims
        //                                    , expires: DateTime.Now.AddMinutes(double.Parse(_config["Jwt:ExpiryTime"]))
        //                                    , signingCredentials: cred
        //                                    );

        //    result.Data.Token = new JwtSecurityTokenHandler().WriteToken(token);

        //    HttpContext.Session.Set<UserVM>(SessionKeys.User.ToString(), result.Data);

        //    return result;
        //}

        //[HttpGet]
        //public long Logout()
        //{
        //    HttpContext.Session.Clear();
        //    return 1;
        //}
    }
}
