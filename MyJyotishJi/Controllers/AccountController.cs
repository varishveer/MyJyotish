using BusinessAccessLayer.Abstraction;
using BusinessAccessLayer.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace MyJyotishJiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _account;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountServices account , IWebHostEnvironment environment, IConfiguration configuration)
        {
            _account = account;
            _environment = environment;
            _configuration = configuration;
           
        }

        #region Admin
        [Authorize(Policy = "Policy1")]
        [HttpPost("registerAdmin")]
        public IActionResult RegisterAdmin([FromBody] AdminModel admin)
        {
            try
            {
                var result = _account.SignUpAdmin(admin);
                if (result == "Successful")
                {

                    return Ok(new { Status = 200, Message = result });
                }
                else if (result == "Email already exist")
                {
                    return Conflict(new { Status = 409, Message = result });
                }
                else if (result == "Data not saved")
                { return StatusCode(500, new { Status = 500, Message = result }); }
                else { return StatusCode(500, new { Status = 500, Message = result }); }
            }
            catch(Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }

        }

        [HttpPost("loginAdmin")]
        public IActionResult LoginAdmin([FromBody] LoginModel login)
        {
            try
            { // Validate the user credentials (use real validation in a production app)
                string result = _account.SignInAdmin(login.Email, login.Password);
                if (result == "Login Successful")
                {
                    var token = GenerateJwtToken(login.Email, "Scheme1");

                    return Ok(new { Status = 200, Message = result, Token = token, User = login.Email });
                }
                else if (result == "Incorrect Password")
                {
                    return Unauthorized(new { Status = 400, Message = "Incorrect Password" });
                }
                else if (result == "Invalid Email")
                {
                    return Unauthorized(new { Status = 400, Message = "Invalid Email" });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex });
            }
        }
        #endregion

        #region Jyotish
        [HttpPost("RegisterJMobile")]
        public IActionResult RegisterJMobile(string Mobile)
        {
            try {
                var result = _account.JRegisterAndSendOtp(Mobile);
                if (result == "Successfull") { return Ok(new {Status = 200,Message = result}); }
                else if(result == "Mobile Number already existed")
                { return Conflict(new { Status = 409, Message = result }); }
                else if(result == "Data not saved") { return StatusCode(500, new { Status = 500, Message = result }); }
                

                else { return StatusCode(500, new { Status = 500, Message = "Internal Server Error" }); }
               
            }
            catch(Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
             
        }

        [HttpPost("VerifyJMobile")]
        public IActionResult VerifyJMobile( string Mobile, int Otp)
        {
            try
            {
                var result = _account.VerifyJOtp(Mobile,Otp);
                if (result == "Successfull") { return Ok(new { Status = 200, Message = result }); }
                else if(result == "Mobile number not found") { return Conflict(new { Status = 409, Message = result }); }
                else if(result == "Invalid Otp") { return BadRequest(new { Status = 400, Message = result }); }
                else if (result == "Mobile Number already Verified")
                { return Conflict(new { Status = 409, Message = result }); }
                else { return StatusCode(500, new { Status = 500, Message = "Internal Server Error" }); }
            }
            catch(Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }
        [HttpPost("RegisterJyotish")]
        public IActionResult RegisterJyotish([FromBody]JyotishViewModel jyotishViewModel) 
        {
            try
            {
                string? path = _environment.ContentRootPath;
                var result = _account.SignUpJyotish(jyotishViewModel);
                if (result == "Successfull")
                { return Ok(new { Status = 200, Message = result });}
                else if (result == "Mobile Number not found")
                { return Conflict(new { Status = 409, Message = result }); }
                else if (result == "Not authorized to register")
                { return Conflict(new { Status = 409, Message = result }); }
                
                else if(result == "Data not saved") { return StatusCode(500, new { Status = 500, Message = "Internal Server Error" }); }
                else
                {
                    return StatusCode(500, new { Status = 500, Message = "Internal Server Error" });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex });
            }
            
        }
        [HttpPost("LoginJyotish")]
        public IActionResult LoginJyotish([FromBody] LoginViewModel jyotishLogin)
        {
            try
            {
                string result = _account.SignInJyotish(jyotishLogin.Mobile, jyotishLogin.Password);
                if (result == "Login Successful")
                {
                    var JyotishName = _account.JUserName(jyotishLogin.Mobile);
                    var UName = JyotishName;
                    var token = GenerateJwtToken(jyotishLogin.Mobile, "Scheme2");
                    return Ok(new { Status = 200, Message = result, Token = token, User = jyotishLogin.Mobile, UserName = UName });
                }
                else if (result == "Invalid Email")
                {
                    return Conflict(new { status = 409, Message = "Invalid Email" });
                }
                else if (result == "Incorrect Password")
                {
                    return Conflict(new { status = 409, Message = "Incorrect Password" });
                }
                else
                return Unauthorized(new { status = 401, Message = "Unauthorized" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex });
            }
        }
        [HttpPost("JForgotPasswordRequest")]
        public IActionResult JForgotPasswordOtpRequest(string Email)
        {
            try
            {
                var result = _account.JForgotPasswordOtpRequest(Email);
                if (result == "Email send successfull")
                { return Ok(new {Status = 200,Message = result}); }
                else if(result == "Email Not found") { return Conflict(new { Status = 409, Message = "Email Not found" }); }
                else if(result == "Otp not send")
                { return StatusCode(500,new { Status = 500, Message = "Otp not send" }); }
                else
                { return StatusCode(500, new { Status = 500, Message = "Internal Server Error" }); }
            }
            catch(Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }
        [HttpPost("JForgotPasswordOtpCheck")]
        public IActionResult JForgotPasswordOtpCheck([FromBody] ForgotPasswordViewModel model)
        {
            try
            {
                var result = _account.JForgotPasswordOtpCheck(model.Email, model.Otp);
                if (result == "Successfull")
                { return Ok(new { Status = 200, Message = result }); }
                else if (result == "Email not found") { return Conflict( new { Status = 409, Message = result }); }
                else if( result == "Invalid Otp") { return BadRequest(new { Status = 400, Message = result }); }
                else { return StatusCode(500, new { Status = 500, Message = "Internal Server Error" }); }
            }
            catch(Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }
        [HttpPost("JChangePassword")]
        public IActionResult JChangePassword([FromBody] ForgotPasswordViewModel model)
        {
            try
            {
                var result = _account.JSavePassword(model.Email, model.Otp, model.Password);
                if (result== "Successfull")
                { return Ok(new { Status = 200, Message = result }); }
                else if (result == "Email not found") { return Conflict(new { Status = 409, Message = result }); }
                else if (result == "Invalid Otp") { return BadRequest(new { Status = 400, Message = result }); }
                else if (result== "Password not Saved") { return StatusCode(500, new { Status = 500, Message = result }); }
                else { return StatusCode(500, new { Status = 500, Message = "Internal Server Error" }); }
            }
            catch(Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }
        
      

        
        #endregion





        #region Token
        private string GenerateJwtToken(string username, string scheme)
        {
            var key = Encoding.UTF8.GetBytes(_configuration[$"Jwt:{scheme}:Key"]);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration[$"Jwt:{scheme}:Issuer"],
                audience: _configuration[$"Jwt:{scheme}:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region User

        [HttpPost("RegisterUserMobile")]
        public IActionResult RegisterUserMobile(string Mobile)
        {
            try
            {
                var result =_account.RegisterUserMobile(Mobile);
                if (result == "Successfull") 
                {
                    return Ok(new {Status = 200, Message = result,}); 
                }
                else if(result == "Mobile Number already exist")
                {
                    return Ok(new { Status = 409, Message = result });
                }
                else if (result == "Otp not sent")
                {
                    return Ok( new { Status = 500, Message = result });
                }
                else if(result == "Data not saved")
                {
                    return Ok( new { Status = 500, Message = "Internal Server Error" });
                }

                else { return Ok( new { Status = 500, Message = "Internal Server Error" }); }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex });
            }
        }

       
        [HttpPost("VerifyUserOtp")]
        public IActionResult VerifyUserOtp(string Mobile , int Otp)
        {
            try
            {
                var result = _account.VerifyUserOtp(Mobile, Otp);
                if (result == "Successful") { return Ok(new {Status = 200,Message= result}); }
                else if(result == "Invalid Data") { return Ok(new { Status = 400, Message = result }); }
                else if(result == "Unauthorized User") { return Ok(new {Status = 409, MEssage = result});}
                else if (result == "User not found" ) { return Ok(new { Status = 409, Message = result }); }

                else { return Ok(new { Status = 400, Message = result }); }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex });
            }
        }

        
        [HttpPost("RegisterUserDetails")]
        public IActionResult RegisterUserDetails([FromBody] UserViewModel _user)
        {
            try
            {
                var result = _account.RegisterUserDetails(_user);
                
                if (result == "Successful") { return Ok(new { Status = 200, Message = result }); }
                else if (result== "Mobile Number Not Registered") { return Ok(new {Status = 409, Message = result }); }
                else if(result == "Unauthorozed") { return Ok(new { Status = 400, Message = result }); }
                else { return Ok( new { Status = 500, Message = result }); }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex });
            }
        }
        [HttpPost("LoginUser")]
        public IActionResult LoginUser(LoginViewModel model)
        {
            try
            {
                var result = _account.LoginUser(model);
                if(result == "Successful Login")
                {
                    var token = GenerateJwtToken(model.Mobile, "Scheme4");
                    return Ok(new {Status = 200,Message = result, Token = token });
                }
                else if(result == "Invalid Number")
                {
                    return Ok(new {Status = 400, Message = result});
                }
                else if (result == "Invalid Password")
                {
                    return Ok(new { Status = 400, Message = result });
                }
                else if (result == "Invalid Credential")
                {
                    return Ok(new { Status = 400, Message = result });
                }
                else
                {
                    return Ok(result );
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex });
            }
        }
        #endregion





    }
}
