using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;//for  SymmetricSecurityKey
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JWTWebAPI.Models;
using JWTWebAPI.Businesslogic;
using JWTWebAPI.DataAccess;

namespace JWTWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserManagement _userManagement;
        public UserController(IConfiguration configuration, IUserManagement userManagement)
        {
            _configuration = configuration;
            _userManagement = userManagement;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody]RegisterModel model)
        {
            User user = new User()
            {
                Username = model.Username,
                Email= model.Email,
                Password = model.Password
            };
            _userManagement.CreateUser(user);
            return Ok(new Response() {Status = "Success", Message="User created successfully" });
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody]LoginModel model)
        {
           User user =  _userManagement.GetUser(model.Username, model.Password);
            if(user !=null && user.UserId > 0)
            {
                var authSignningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var authClaims = new List<Claim>
                {
                    new Claim("UserName", user.Username),
                    new Claim("UserId", user.UserId.ToString())
                };

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSignningKey, SecurityAlgorithms.HmacSha256)
                    );


                //var token = new JwtSecurityToken(
                //    issuer: _configuration["JWT:ValidIssuer"],
                //    audience: _configuration["JWT:ValidAudience"],
                //    claims: authClaims,
                //    expires: DateTime.Now.AddHours(3),
                //    SigningCredentials = new SigningCredentials(authSignningKey, SecurityAlgorithms.HmacSha256)

                //    );
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expires = token.ValidTo});
            }
            return Unauthorized();
        }
    }
}
