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
        public string JRegisterAndSendOtp(string Email);
        public string VerifyJOtp(string Email, int Otp);
        public string SignUpJyotish(JyotishViewModel jyotishView, string path );
        public string SignInJyotish(string Email, string password);
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
        public string RegisterUserEmail(string Email);
        public string VerifyUserOtp(string Email, int Otp);
        public string RegisterUserDetails(UserViewModel _user);
        public string LoginUser(LoginModel model);
        #endregion
        public List<string> PlaceOfBirthList(string CityName);
        public List<string> LanguageList();

    }
}
