﻿using BusinessAccessLayer.Abstraction;
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
using Newtonsoft.Json;
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
               
                 if (result == "Incorrect Password")
                {
                    return Ok(new { Status = 400, Message = "Incorrect Password" });
                }
                else if (result == "Invalid Email")
                {
                    return Ok(new { Status = 400, Message = "Invalid Email" });
                }
                else
                {
                    var token = GenerateJwtToken(login.Email, "Scheme1");

                    return Ok(new { Status = 200, Message = result, Token = token, User = login.Email, Id= result });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex });
            }
        } 
        [HttpPost("SignInAdminEmployees")]
        public IActionResult SignInAdminEmployees([FromBody] LoginModel login)
        {
            try
            { // Validate the user credentials (use real validation in a production app)
                int result = _account.SignInAdminEmployees(login.Email, login.Password);
                if (result !=0 && result != -1)
                {
                    var token = GenerateJwtToken(login.Email, "Scheme1");

                        

                    return Ok(new { Status = 200, Message = "Login Successfully", Token = token, User = result });
                }
                else if (result == 0)
                {
                    return Ok(new { Status = 400, Message = "Incorrect Password" });
                }
                else if (result == -1)
                {
                    return Ok(new { Status = 400, Message = "Invalid Email" });
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
        [HttpPost("RegisterJyotish")]
        public IActionResult RegisterJyotish(string Email)
        {
            try 
            {

               

                var result = _account.JRegisterAndSendOtp(Email);
                if (result == "Successful" ) {
                    
                        return Ok(new { Status = 200, Message = "Otp sent Successfully" });

                    
                }
                else if (result == "Email already verified")
                { return Ok(new { Status = 200, Message = result }); }

                else if (result == "Data not saved") { 
                    return StatusCode(500, new { Status = 500, Message = result });
                }
                else
                {
                    return Ok(new { Status = 400, Message = result }); 
                };
              
               
            }
            catch(Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
             
        }

        [HttpGet("checkDblNumber")]
        public IActionResult checkDblNumber(string number)
        {
            if (number.Length != 10)
            {
                return Ok(new { status = 400, message = "invalid phone number" });

            }


            var res = _account.checkDblNumber(number);
            if (res)
            {
                return Ok(new { status = 200, message = "not matched" });
            }
            else
            {
                return Ok(new { status = 400, message = "Mobile number already in used" });

            }
        }

        [HttpPost("VerifyJyotish")]
        public IActionResult VerifyJyotish(EmailViewModel model)
        {
            try
            {
                var result = _account.VerifyJOtp(model.Email,model.Otp);
                if (result == "Successful" ) { return Ok(new { Status = 200, Message = result }); }
                else if(result == "Email not found") { return Ok(new { Status = 409, Message = result }); }
                else if(result == "Invalid Otp") { return Ok(new { Status = 400, Message = result }); }
                else if (result == "Mobile Number already Verified")
                { return Ok(new { Status = 409, Message = result }); }
                else { return Ok( new { Status = 500, Message = "Internal Server Error" }); }
            }
            catch(Exception ex)
            { return Ok( new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }
        [HttpPost("SignUpJyotish")]
        public IActionResult SignUpJyotish( JyotishViewModel jyotishViewModel) 
        {
            try
            {
                if (jyotishViewModel.Mobile.Length != 10)
                {
                     return Ok(new { Status = 400, Message = "invalid mobile number" }); 
                }
                var result = _account.SignUpJyotish(jyotishViewModel);
                if (result == "Successful")
                { return Ok(new { Status = 200, Message = result });}
                else if(result == "Data not saved") { return StatusCode(500, new { Status = 500, Message = "Internal Server Error" }); }
                else
                {
                    return Ok(new { Status = 400, Message = result });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex });
            }
            
        }
        [HttpPost("LoginJyotish")]
        public IActionResult LoginJyotish( LoginModel jyotishLogin)
        {
            try
            {
                string result = _account.SignInJyotish(jyotishLogin.Email, jyotishLogin.Password);
               
                 if (result == "Invalid Email")
                {
                    return Ok(new { status = 409, Message = "Invalid Email" });
                }
                else if (result == "Incorrect Password")
                {
                    return Ok(new { status = 409, Message = "Incorrect Password" });
                }
                else
                {
                    var Response = JsonConvert.DeserializeObject<JyotishModel>(result);

                    if (Response.Role == "Jyotish")
                    {
                        var token = GenerateJwtToken(jyotishLogin.Email, "Scheme2");
                        return Ok(new { Status = 200, Message = Response.Role, Token = token, User = Response.Email, UserName = Response.Name, Role = Response.Role, Id = Response.Id });
                    }
                    else if (Response.Role == "Pending")
                    {
                        var token = GenerateJwtToken(jyotishLogin.Email, "Scheme3");
                        return Ok(new { Status = 200, newStatus = Response.NewStatus, Message = Response.Role, Token = token, User = Response.Email, UserName = Response.Name, Role = Response.Role, Id = Response.Id });
                    }
                    else if (Response.Role == "Rejected")
                    {
                        return Ok(new { Status = 200,  UserName = Response.Name, Role = Response.Role, Id = Response.Id });
                    }
                    else {
                        return Ok(new { status = 500, Message = "Something Went Wrong" });
                    }
                   
                }
               
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
                expires: DateTime.UtcNow.AddYears(100),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region User

        [HttpPost("RegisterUserEmail")]
        public IActionResult RegisterUserEmail(string Email)
        {
            try
            {
                var result =_account.RegisterUserEmail(Email);
                if (result == "Successful" || result == "Email already verified") 
                {
                    return Ok(new {Status = 200, Message = result,});  
                }
                else if(result == "Data not saved")
                {
                    return Ok(new { Status = 500, Message = "Internal Server Error" });
                }
                
                else { return Ok( new { Status = 400, Message = result }); }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex });
            }
        }

       
        [HttpPost("VerifyUserOtp")]
        public IActionResult VerifyUserOtp(EmailViewModel model)
        {
            try
            {
                var result = _account.VerifyUserOtp(model.Email, model.Otp);
                if (result == "Successful") { return Ok(new {Status = 200,Message= result}); }
                else if(result == "Invalid Data") { return Ok(new { Status = 400, Message = result }); }
                else if(result == "Unauthorized User") { return Ok(new {Status = 401, MEssage = result});}
                else { return Ok(new { Status = 409, Message = result }); }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex });
            }
        }

        
        [HttpPost("RegisterUserDetails")]
        public IActionResult RegisterUserDetails( )
        {
            try
            {
                var httpRequest = HttpContext.Request;
                UserViewModel _user = new UserViewModel
                {
                    Email = httpRequest.Form["email"],
                    Mobile = httpRequest.Form["mobile"],
                    Name = httpRequest.Form["name"],
                    Gender = httpRequest.Form["gender"],
                    DoB =Convert.ToDateTime(httpRequest.Form["doB"]),
                    PlaceOfBirth = httpRequest.Form["placeOfBirth"],
                    Country = int.Parse( httpRequest.Form["country"]),
                    State = int.Parse( httpRequest.Form["state"]),
                    City = int.Parse( httpRequest.Form["city"]),
                    
                  
                };

                if (!string.IsNullOrEmpty(httpRequest.Form["timeOfBirth"]))
                {
                    _user.TimeOfBirth =httpRequest.Form["timeOfBirth"];
                }
               


                var result = _account.RegisterUserDetails(_user);
                
                if (result == "Successful") { return Ok(new { Status = 200, Message = result }); }
                else if (result== "Email Not Registered") { return Ok(new {Status = 409, Message = result }); }
                else if (result == "Invalid Number") { return Ok(new { Status = 409, Message = result }); }
                else if(result == "Unauthorized") { return Ok(new { Status = 401, Message = result }); }
                else { return Ok( new { Status = 500, Message = result }); }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex });
            }
        }
        [HttpPost("LoginUser")]
        public IActionResult LoginUser(LoginModel model)
        {
            try
            {
                var result = _account.LoginUser(model);
                /*if(result == "Successful Login")
                {
                    var token = GenerateJwtToken(model.Email, "Scheme4");
                    return Ok(new {Status = 200,Message = result, Token = token });
                }*/
                if (result.StartsWith("{") && result.EndsWith("}"))
                {
                    var userResponse = JsonConvert.DeserializeObject<UserViewModel>(result);
                    var token = GenerateJwtToken(model.Email, "Scheme4");
                    return Ok(new
                    {
                        Status = 200,
                        Message = "Successful Login",
                        Token = token,
                        UserName = userResponse.Name,
                        UserEmail = userResponse.Email,
                        UserStatus = userResponse.Status,
                        UserMobile = userResponse.Mobile,
                        UserId = userResponse.Id

                    });
                }
                else
                {
                    return Ok(new { Status = 409, Message = result });
                }
               
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex });
            }
        }
        #endregion
       

        [AllowAnonymous]
        [HttpGet("Languages")]
        public IActionResult Languages()
        {
            try
            {
                var Records = _account.LanguageList();
                if (Records == null)
                {
                    return Ok(new { Status = 500, Message = "Internal Server Error" });
                }
                else
                {
                    return Ok(new { Status = 200, Data = Records, Message = "Successful" });
                }
            }
            catch
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error" });
            }
        }

        [AllowAnonymous]
        [HttpGet("PlaceOfBirthList")]
        public IActionResult PlaceOfBirthList(string City)
        {
            try
            {
                var Records = _account.PlaceOfBirthList(City);
                if (Records == null)
                {
                    return Ok(new { Status = 500, Message = "Internal Server Error" });
                }
                else
                {
                    return Ok(new { Status = 200, Data = Records, Message = "Successful" });
                }
            }
            catch
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error" });
            }

        }

        [AllowAnonymous]
        [HttpGet("sendOtp")]
        public IActionResult sendOtp(string email,string sendBy)
        {
            try
            {
                var res = _account.sendOtp(email, sendBy);
                if (res)
                {
                    return Ok(new { status = 200, message = "Otp send successfully" });

                }
                else
                {
                    return Ok(new { status = 500, message = "something went wrong while sending opt" });

                }
            }catch(Exception ex)
            {
                throw ex;
            }
        } 
        [AllowAnonymous]
        [HttpGet("verifyOtp")]
        public IActionResult verifyOtp(string email,int otp,string sendBy)
        {
            try
            {
                var res = _account.verifyOtps(email,otp, sendBy);
                if (res)
                {
                    return Ok(new { status = 200, message = "Otp Verified successfully" });

                }
                else
                {
                    return Ok(new { status = 500, message = "Otp not matched" });

                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        [AllowAnonymous]
        [HttpGet("changePasswordByEmail")]
        public IActionResult changePasswordByOtp(string email,string sendBy,string password)
        {
            try
            {
                var res = _account.changePassword(email,password, sendBy);
                if (res)
                {
                    return Ok(new { status = 200, message = "Otp Verified successfully" });

                }
                else
                {
                    return Ok(new { status = 500, message = "Otp not matched" });

                }
            }catch(Exception ex)
            {
                throw ex;
            }
        } 
        [AllowAnonymous]
        [HttpGet("changePasswordByOldPassword")]
        public IActionResult changePasswordByOldPassword(int userId,string oldpassword,string sendBy,string password)
        {
            try
            {
                var res = _account.changePasswordByOldPassword( userId, password,oldpassword, sendBy);
                if (res)
                {
                    return Ok(new { status = 200, message = "Otp Verified successfully" });

                }
                else
                {
                    return Ok(new { status = 500, message = "Old Password are not matched" });

                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
