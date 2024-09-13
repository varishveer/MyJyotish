using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Abstraction
{
    public interface IAccountServices
    {
        #region Jyotish
        public string JRegisterAndSendOtp(string Mobile);
        public string VerifyJOtp(string Mobile, int Otp);
        public string SignUpJyotish(JyotishViewModel jyotishView );
        public string SignInJyotish(string mobile, string password);
        public string JUserName(string Mobile);
        public string JForgotPasswordOtpRequest(string Email);
        public string JForgotPasswordOtpCheck(string Email, int Otp);
        public string JSavePassword(string Email, int Otp, string Password);

        #endregion

        #region Admin
        public string SignUpAdmin(AdminModel admin);
        public string SignInAdmin(string email, string password);
        #endregion

     

        #region User
        public string RegisterUserMobile(string Email);
        public string VerifyUserOtp(string Mobile, int Otp);
        public string RegisterUserDetails(UserViewModel _user);
        public string LoginUser(LoginViewModel model);
        #endregion

      
    }
}
