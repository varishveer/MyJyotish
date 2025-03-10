﻿using BusinessAccessLayer.Abstraction;
using DataAccessLayer.DbServices;

using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Azure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

namespace BusinessAccessLayer.Implementation
{
    public class AccountServices:IAccountServices
    {
        private readonly ApplicationContext _context;
        public AccountServices(ApplicationContext context)
        {
            _context = context;
        }
       
        #region JYotish

        //check Number Duplicacy
        public bool checkDblNumber(string phoneNumber)
        {
            if (phoneNumber.Length != 10)
            {
                return false;
            }

            var IsMobileValid = _context.JyotishRecords.Where(x => x.Mobile == phoneNumber).FirstOrDefault();
            if (IsMobileValid == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string JRegisterAndSendOtp(string Email)
        {
            var IsMobileValid = _context.JyotishRecords.Where(x => x.Email == Email).FirstOrDefault();
            var userRecord = _context.Users.FirstOrDefault(e => e.Email == Email);

                if(userRecord!=null) return "Invalid email or may be already registered";

            if (IsMobileValid != null)
            {
                if (IsMobileValid.ApprovedStatus == "Unverified" || IsMobileValid.ApprovedStatus==null)
                {
                    int NewOtp = new Random().Next(100000, 1000000); ;

                    string newMessage = $@"
           <!DOCTYPE html>
            <html lang=""en"">
            <head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>My Jyotish G Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 8px;
            background-color: #f9f9f9;
        }}
        .header {{
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
        }}
        .content {{
            font-size: 16px;
            line-height: 1.5;
            margin-bottom: 20px;
        }}
        .otp {{
            font-size: 20px;
            font-weight: bold;
            color: #000;
        }}
        .logo {{
            margin: 20px 0;
            text-align: center;
        }}
        .logo img {{
            max-width: 150px;
        }}
        .footer {{
            font-size: 14px;
            color: #555;
            margin-top: 20px;
        }}
        .footer a {{
            color: #000;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
       
         <div class=""logo"">
            <img src=""https://api.myjyotishg.in/Images/Logo.png"" alt=""My Jyotish G Logo"">
        </div>
        <div class=""content"">
            Hi User,<br><br>
            Thank you for signing up! To complete your registration, please verify your email address using the code below:<br><br>

            Verification Code: <span class=""otp"">{NewOtp}</span><br><br>

            If you have any questions, feel free to reach out!
        </div>

       
            <div class=""header"" style=""color:orange"">My Jyotish G</div>
            <h4>www.myjyotishg.in</h4>
            <h4>myjyotishg@gmail.com</h4>
            <h4>7985738804</h4>
            
        
    </div>
</body>
</html>
";
                    string NewSubject = "Verification Code for My Jyotish G";

                    var isNewMailSend = SendEmail(newMessage, Email, NewSubject);
                    if (isNewMailSend)
                    {
                        IsMobileValid.Otp = NewOtp;


                        _context.JyotishRecords.Update(IsMobileValid);
                        var NewResult = _context.SaveChanges();
                        if (NewResult > 0)
                        { return "Successful"; }
                        else { return "Data not saved"; }
                    }
                    else { return "Data not saved"; }
                }
                else if (IsMobileValid.ApprovedStatus == "Verified")
                {
                    return "Email already verified";
                }
                else 
                {
                    return "Invalid email or may be already registered";
                }
                 
            }
            
            int Otp = new Random().Next(100000, 1000000); ;
            JyotishModel model = new JyotishModel();
            string message = $@"
           <!DOCTYPE html>
            <html lang=""en"">
            <head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>MyJyotishG Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 8px;
            background-color: #f9f9f9;
        }}
        .header {{
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
        }}
        .content {{
            font-size: 16px;
            line-height: 1.5;
            margin-bottom: 20px;
        }}
        .otp {{
            font-size: 20px;
            font-weight: bold;
            color: #000;
        }}
        .logo {{
            margin: 20px 0;
            text-align: center;
        }}
        .logo img {{
            max-width: 150px;
        }}
        .footer {{
            font-size: 14px;
            color: #555;
            margin-top: 20px;
        }}
        .footer a {{
            color: #000;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
       
         <div class=""logo"">
            <img src=""https://api.myjyotishg.in/Images/Logo.png"" alt=""My Jyotish G Logo"">
        </div>
        <div class=""content"">
          
            Thank you for signing up! To complete your registration, please verify your email address using the code below:<br><br>

            Verification Code: <span class=""otp"">{Otp}</span><br><br>

            If you have any questions, feel free to reach out!
        </div>

       
            <div class=""header"" style=""color:orange"">My Jyotish G</div>
            <h4>www.myjyotishg.in</h4>
            <h4>myjyotishg@gmail.com</h4>
            <h4>7985738804</h4>
            
        
    </div>
</body>
</html>
";
            string Subject = "Verification Code for My Jyotish G";
            var isMailSend =SendEmail(message,Email,Subject);
            if(isMailSend)
            {model.Otp = Otp;
            model.Email = Email;
                model.ApprovedStatus = "Unverified";
            model.Role = "Pending";
            _context.JyotishRecords.Add(model);
            var result = _context.SaveChanges();
                if (result > 0)
                { return "Successful"; }
                else { return "Data not saved"; }
            }
            else { return "Data not saved"; }
        }
        public string VerifyJOtp(string Email, int Otp)
        {
            var record = _context.JyotishRecords.Where(x => x.Email == Email).FirstOrDefault();
            if (record != null)
            {
                if (record.Otp == Otp)
                {
                    if(record.ApprovedStatus == "Unverified" || record.ApprovedStatus == null)
                    {
                        record.ApprovedStatus = "Verified";
                    }
                    else { return "Email already Verified"; }

                     record.Feedback = false;
                    _context.JyotishRecords.Update(record);

                    var result = _context.SaveChanges();

                    if (result > 0) {

                      
                        return "Successful"; }
                    else
                    {
                        return "Data not saved";
                    }
                }
                else { return "Invalid Otp"; }
            }
            return "Email not found";
        }
        public string SignUpJyotish(JyotishViewModel jyotishView)
        {
            

            var Jyotish = _context.JyotishRecords.Where(x => x.Email == jyotishView.Email).FirstOrDefault();
            if (Jyotish == null )
            { return "invalid email or maybe already register"; }
            var JyotishMobile = _context.JyotishRecords.Where(x => x.Mobile == jyotishView.Mobile).FirstOrDefault();
            if (JyotishMobile != null)
            {
                return "invalid number or maybe already in used";
            }
            if ( Jyotish.ApprovedStatus != "Verified")
            { return "Not authorized to register"; }
            var CountryName = _context.Countries.Where(x => x.Id == jyotishView.Country).FirstOrDefault();
            var StateName = _context.States.Where(x => x.Id == jyotishView.State).FirstOrDefault();
            var CityName = _context.Cities.Where(x => x.Id == jyotishView.City).FirstOrDefault();

            Jyotish.Name = jyotishView.Name;

            var countryCode = _context.CountryCode.Where(e => e.country == jyotishView.Country).Select(e => e.Id).FirstOrDefault();

            Jyotish.Mobile = jyotishView.Mobile;
            Jyotish.Gender = jyotishView.Gender;
            Jyotish.Language = jyotishView.Language;
            Jyotish.Expertise = jyotishView.Expertise;
            Jyotish.Country = CountryName.Name;
            Jyotish.Date = DateTime.Now;
            Jyotish.State = StateName.Name;
            Jyotish.City = CityName.Name;
            Jyotish.NewStatus =true;
            Jyotish.Status =true;
            Jyotish.countryCode = countryCode;
            Jyotish.Role = "Pending";
            Jyotish.ApprovedStatus = "Pending";
            Jyotish.Password = Guid.NewGuid().ToString("N").Substring(0, 8);

            _context.JyotishRecords.Update(Jyotish);
            var result = _context.SaveChanges();
            if (result > 0)
            {

                string message = $@"
           <!DOCTYPE html>
            <html lang=""en"">
            <head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>My Jyotish G Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 8px;
            background-color: #f9f9f9;
        }}
        .header {{
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
        }}
        .content {{
            font-size: 16px;
            line-height: 1.5;
            margin-bottom: 20px;
        }}
        .otp {{
            font-size: 20px;
            font-weight: bold;
            color: #000;
        }}
        .logo {{
            margin: 20px 0;
            text-align: center;
        }}
        .logo img {{
            max-width: 150px;
        }}
        .footer {{
            font-size: 14px;
            color: #555;
            margin-top: 20px;
        }}
        .footer a {{
            color: #000;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
       
         <div class=""logo"">
            <img src=""https://api.myjyotishg.in/Images/Logo.png"" alt=""My Jyotish G Logo"">
        </div>
        <div class=""content"">
           Dear Jyotish ,<br/>
            Your account has been successfully created and your Credential are below , <br/>
            Email: {Jyotish.Email}<br/>
            Password: {Jyotish.Password}<br/>
            If you have any questions, feel free to reach out!<br/>
        </div>

       
            <div class=""header"" style=""color:orange"">My Jyotish G</div>
            <h4>www.myjyotishg.in</h4>
    </div>
</body>
</html>
";
               
                var subject = "MyJyotishG Account Credential";
                SendEmail(message, Jyotish.Email, subject);
                return "Successful";
            }
            return "Data not saved";
        }

        public string SignInJyotish(string Email, string password)
        {
            var _Jyotish = _context.JyotishRecords.Where(x => x.Email == Email).FirstOrDefault();
            if (_Jyotish != null)
            {
                if (_Jyotish.Password == password)
                {


                    return JsonConvert.SerializeObject(new JyotishModel
                    {
                       Id=  _Jyotish.Id,
                       Name = _Jyotish.Name,
                       Email =  _Jyotish.Email,
                       Role = _Jyotish.Role,
                       NewStatus=_Jyotish.NewStatus,

                    });
                }
                else
                {
                    return "Incorrect Password";
                }
            }
            else
            { return "Invalid Email"; }
        }

   

        public string JForgotPasswordOtpRequest(string Email)
        {
            var isPJValid = _context.JyotishRecords.Where(x => x.Email == Email).FirstOrDefault();
            if (isPJValid == null)
            { return "Email Not found"; }
            else
            {
                var Otp = 123456;
                var message = "Hi Jyotish , \n Your verification code for password change is " + Otp + ".";
                var subject = "MyJyotishG Password Change Request";
                SendEmail(message, isPJValid.Email, subject);

                isPJValid.Otp = Otp;
                _context.JyotishRecords.Update(isPJValid);
                var result = _context.SaveChanges();
                if (result > 0)
                { return "Email send successfull"; }
                else
                { return "Otp not send"; }
            }

        }

        public string JForgotPasswordOtpCheck(string Email, int Otp)
        {
            var isValid = _context.JyotishRecords.Where(x => x.Email == Email).FirstOrDefault();
            if (isValid == null) { return "Email not found"; }
            else
            {
                if (Otp == isValid.Otp)
                {
                    return "Successfull";
                }
                else { return "Invalid Otp"; }
            }
        }

        public string JSavePassword(string Email, int Otp, string Password)
        {
            var isValid = _context.JyotishRecords.Where(x => x.Email == Email).FirstOrDefault();
            if (isValid == null) { return "Email not found"; }
            else
            {
                if (Otp == isValid.Otp)
                {
                    isValid.Password = Password;
                    _context.JyotishRecords.Update(isValid);
                    var result = _context.SaveChanges();
                    if (result > 0)
                    { return "Successfull"; }
                    else { return "Password not Saved"; }
                }
                else { return "Invalid Otp"; }
            }
        }

        #endregion

       

        #region Admin
        public string SignUpAdmin(AdminModel admin)
        {

            var IsEmailValid = _context.AdminRecords.Where(x => x.Email == admin.Email).FirstOrDefault();

            if (IsEmailValid != null)
            { return "Email already exist"; }
            AdminModel _admin = new AdminModel()
            {
                Name = admin.Name,
                Email = admin.Email,
                Password = admin.Password,
                Role = "Admin",
            };
            _context.AdminRecords.Add(_admin);
            var result = _context.SaveChanges();
            if(result >0)
            { return "Successful"; }
            return "Data not saved";
        }
        public string SignInAdmin(string email, string password)
        {
            var _admin = _context.AdminRecords.Where(x => x.Email == email).FirstOrDefault();
            if (_admin != null)
            {
                if (_admin.Password == password)
                {
                    return _admin.Id.ToString();
                }
                else
                {
                    return "Incorrect Password";
                }
            }
            else
            { return "Invalid Email"; }
        }
        public int SignInAdminEmployees(string email, string password)
        {
            var _admin = _context.Employees.Where(x => x.Email == email).FirstOrDefault();
            if (_admin != null)
            {
                if (_admin.password == password)
                {
                    

                    return _admin.Id;

                }
                else
                {
                    return 0;
                }
            }
            else
            { return -1; }
        }
        #endregion

        #region User
        public string RegisterUserEmail(string Email)
        {
            var record = _context.Users.FirstOrDefault(x => x.Email == Email);
            var jyotishRecord = _context.JyotishRecords.FirstOrDefault(x => x.Email == Email);
            if (jyotishRecord != null)
            {
                return "Email already registered";
            }
            if (record != null)
            {
                if (record.Status == "Otp Unverified")
                {
                    int newOtp = new Random().Next(100000, 1000000);
                    string newMessage = $@"
           <!DOCTYPE html>
            <html lang=""en"">
            <head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>My Jyotish G Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 8px;
            background-color: #f9f9f9;
        }}
        .header {{
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
        }}
        .content {{
            font-size: 16px;
            line-height: 1.5;
            margin-bottom: 20px;
        }}
        .otp {{
            font-size: 20px;
            font-weight: bold;
            color: #000;
        }}
        .logo {{
            margin: 20px 0;
            text-align: center;
        }}
        .logo img {{
            max-width: 150px;
        }}
        .footer {{
            font-size: 14px;
            color: #555;
            margin-top: 20px;
        }}
        .footer a {{
            color: #000;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
       
         <div class=""logo"">
            <img src=""https://api.myjyotishg.in/Images/Logo.png"" alt=""My Jyotish G Logo"">
        </div>
        <div class=""content"">
            Hi User,<br><br>
            Thank you for signing up! To complete your registration, please verify your email address using the code below:<br><br>

            Verification Code: <span class=""otp"">{newOtp}</span><br><br>

            If you have any questions, feel free to reach out!
        </div>

       
            <div class=""header"" style=""color:orange"">My Jyotish G</div>
            <h4>www.myjyotishg.in</h4>
            <h4>myjyotishg@gmail.com</h4>
            <h4>7985738804</h4>
            
        
    </div>
</body>
</html>
";

                    string newSubject = "Please Verify Your Email Address";
                    SendEmail(newMessage, Email, newSubject);

                    record.Otp = newOtp;
                    _context.Users.Update(record);
                    var newResult = _context.SaveChanges();
                    return newResult > 0 ? "Successful" : "Data not saved";
                }
                else if(record.Status == "Otp Verified") 
                {
                    return "Email already verified";
                }
                return "Email already registered";
            }

            int otp = new Random().Next(100000, 1000000);
            string message = $@"
   <!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>My Jyotish G Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 8px;
            background-color: #f9f9f9;
        }}
        .header {{
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
        }}
        .content {{
            font-size: 16px;
            line-height: 1.5;
            margin-bottom: 20px;
        }}
        .otp {{
            font-size: 20px;
            font-weight: bold;
            color: #000;
        }}
        .logo {{
            margin: 20px 0;
            text-align: center;
        }}
        .logo img {{
            max-width: 150px;
        }}
        .footer {{
            font-size: 14px;
            color: #555;
            margin-top: 20px;
        }}
        .footer a {{
            color: #000;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
       
         <div class=""logo"">
            <img src=""https://api.myjyotishg.in/Images/Logo.png"" alt=""My Jyotish G Logo"">
        </div>
        <div class=""content"">
            Hi User,<br><br>
            Thank you for signing up! To complete your registration, please verify your email address using the code below:<br><br>

            Verification Code: <span class=""otp"">{otp}</span><br><br>

            If you have any questions, feel free to reach out!
        </div>

       
            <div class=""header"" style=""color:orange"">My Jyotish G</div>
            <h4>www.myjyotishg.in</h4>
            <h4>myjyotishg@gmail.com</h4>
            <h4>7985738804</h4>
            
        
    </div>
</body>
</html>
";

            string subject = "Please Verify Your Email Address";
            SendEmail(message, Email, subject);

            UserModel _user = new UserModel()
            {
                Email = Email,
                Otp = otp,
                Status = "Otp Unverified"
            };
            _context.Users.Add(_user);
            var result = _context.SaveChanges();




            return result > 0 ? "Successful" : "Data not saved";
        }


        public string VerifyUserOtp(string Email, int Otp)
        {
            if(Email == null || Otp ==0)
            {
                return "Invalid Data";
            }
            var user = _context.Users.Where(x=>x.Email == Email).FirstOrDefault();
            if(user == null) { return "User not found"; }
            if(user.Status != "Otp Unverified") { return "Invalid email or may be already registered"; }
            if(user.Otp == Otp) 
            {
                user.Status = "Otp Verified";
               // user.Password = (new Random().Next(10000000, 100000000)).ToString();
                _context.Users.Update(user);
                _context.SaveChanges();

               
                return "Successful"; 
            }
            else { return "Invalid Otp"; }
        }

        public string RegisterUserDetails(UserViewModel _user)
        {
            var record = _context.Users.Where(x=>x.Email == _user.Email).FirstOrDefault();
            if(record == null)
            {
                return "Email Not Registered";
            }
            if(record.Status != "Otp Verified") { return "Unauthorized"; }

            
            if (_user.Mobile != null)
            { 
                if(_user.Mobile.Length != 10) { return "Invalid Number"; }
                var UserMobile = _context.Users.Where(x=>x.Mobile == _user.Mobile).FirstOrDefault();

               if(UserMobile == null) { record.Mobile = _user.Mobile; }
               else
                {
                        if (UserMobile.Mobile == null) { record.Mobile = _user.Mobile; }
                        else { return "Invalid Number"; }
                }
               
               
                
            }

            
             record.Name = _user.Name; 
             record.Gender = _user.Gender; 
             record.DoB = _user.DoB; 
             record.PlaceOfBirth = _user.PlaceOfBirth;
            var countryCode = _context.CountryCode.Where(e => e.country == _user.Country).Select(e => e.Id).FirstOrDefault();
            record.CountryCodeId = countryCode;
            record.Country = _user.Country;
            record.State = _user.State;
            record.City = _user.City;

            if (_user.TimeOfBirth != null)
            { record.TimeOfBirth = _user.TimeOfBirth; }
            record.Status = "Verified";

            record.Password = (new Random().Next(10000000, 100000000)).ToString();

            _context.Users.Update(record);
            var result = _context.SaveChanges();
            if(result>0)
            {
                string message = $@"
           <!DOCTYPE html>
            <html lang=""en"">
            <head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>MyJyotishG Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 8px;
            background-color: #f9f9f9;
        }}
        .header {{
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
        }}
        .content {{
            font-size: 16px;
            line-height: 1.5;
            margin-bottom: 20px;
        }}
        .otp {{
            font-size: 20px;
            font-weight: bold;
            color: #000;
        }}
        .logo {{
            margin: 20px 0;
            text-align: center;
        }}
        .logo img {{
            max-width: 150px;
        }}
        .footer {{
            font-size: 14px;
            color: #555;
            margin-top: 20px;
        }}
        .footer a {{
            color: #000;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
       
         <div class=""logo"">
            <img src=""https://api.myjyotishg.in/Images/Logo.png"" alt=""My Jyotish G Logo"">
        </div>
        <div class=""content"">
           Dear User ,<br/>
            Your account has been successfully created and your Credential are below , <br/>
            Email: {_user.Email}<br/>
            Password: {_user.Password}<br/>
            If you have any questions, feel free to reach out!<br/>
        </div>

       
            <div class=""header"" style=""color:orange"">My Jyotish G</div>
            <h4>www.myjyotishg.in</h4>
    </div>
</body>
</html>
";

                var subject = "MyJyotishG Account Credential";
                SendEmail(message, _user.Email, subject);
                return "Successful";
            }
            else { return "Data not saved"; }
        }
       

        public string LoginUser(LoginModel model)
        {
            if (model.Email == null || model.Password == null) { return "Invalid Credential"; }
            else
            {
                var record = _context.Users.Where(x => x.Email == model.Email).FirstOrDefault();
                if(record == null)
                { return "Invalid Email"; }
                else 
                {
                    if(record.Password == model.Password)
                    {
                        return JsonConvert.SerializeObject(new UserViewModel
                        {
                            Name = record.Name,
                            Email = record.Email,
                            Mobile = record.Mobile,
                            Status = record.Status,
                            Id = record.Id
                        });
                    }
                    else
                    {
                        return "Invalid Password";
                    }
                }
                
            }
        }


        #endregion

        public bool SendEmail(string MessageBody, string Mail, string Subjectbody)
        {
            try
            {
                var activeMail = _context.ActiveMail.Where(e => e.ActiveStatus && e.Status).FirstOrDefault();
              
                // Sender's email address and app-specific password
                string senderEmail = activeMail.Email;
                string senderPassword = activeMail.Password; // Ensure this is an app-specific password if 2FA is enabled

                // Recipient's email address
                string recipientEmail = Mail;

                // SMTP server details
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587; // Use port 587 for TLS

                // Create and configure the SMTP client
                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true; // Ensure SSL/TLS is enabled
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    // Create the email message
                    MailMessage message = new MailMessage(senderEmail, recipientEmail)
                    {
                        Subject = Subjectbody,
                        Body = MessageBody,
                        IsBodyHtml = true
                    };
                    
                    // Send the email
                    client.Send(message);

                    // If no exception is thrown, email was sent successfully
                    Console.WriteLine("Email sent successfully.");
                    return true;
                }
            }
            catch (SmtpException smtpEx)
            {
                // Handle specific SMTP errors
                Console.WriteLine($"SMTP error: {smtpEx.StatusCode} - {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle other general errors
                Console.WriteLine($"Failed to send email. Error message: {ex.Message}");
            }

            // If we reach here, it means the email sending failed
            return false;
        }

        public dynamic PlaceOfBirthList(string CityName)
        {
            

            var collection = CityName.Split(',');
            var city="";
            var state="";
            var country="";
            if(collection.Length > 0)
            {
                city = collection[0];
                state = collection.Length > 1 ? collection[1] : "";
                country = collection.Length > 2 ? collection[2] : "";
            }

            var Table = (from Country in _context.Countries
                        join State in _context.States on Country.Id equals State.CountryId
                        join City in _context.Cities on State.Id equals City.StateId
                        where City.Name.Contains(city) && State.Name.Contains(state) && Country.Name.Contains(country)
                        orderby City.Name
						 select new
                        {
                            record=City.Name+","+" "+State.Name+","+" "+Country.Name
                           
                        }).Take(15).ToList();

            return Table;
        }
        public List<string> LanguageList()
        {
            var Languages = _context.Languages.Select(l => l.Name).ToList();
            return Languages;
        }

        public bool sendOtp(string email,string sendBy)
        {
            dynamic res = null;
            if (sendBy == "client")
            {
                res = _context.Users.Where(e => e.Email == email).FirstOrDefault<UserModel>();

            }
            else
            {

              res = _context.JyotishRecords.Where(e => e.Email == email).FirstOrDefault<JyotishModel>();
            }
            if (res == null)
            {
                return false;
            }
            int Otp = new Random().Next(100000, 1000000);
            string message = $@"
           <!DOCTYPE html>
            <html lang=""en"">
            <head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>My Jyotish G Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 8px;
            background-color: #f9f9f9;
        }}
        .header {{
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
        }}
        .content {{
            font-size: 16px;
            line-height: 1.5;
            margin-bottom: 20px;
        }}
        .otp {{
            font-size: 20px;
            font-weight: bold;
            color: #000;
        }}
        .logo {{
            margin: 20px 0;
            text-align: center;
        }}
        .logo img {{
            max-width: 150px;
        }}
        .footer {{
            font-size: 14px;
            color: #555;
            margin-top: 20px;
        }}
        .footer a {{
            color: #000;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
       
         <div class=""logo"">
            <img src=""https://api.myjyotishg.in/Images/Logo.png"" alt=""My Jyotish G Logo"">
        </div>
        <div class=""content"">
          
            Thank you for using myjyotishg! To Change your password, please verify your email address using the code below:<br><br>

            Verification Code: <span class=""otp"">{Otp}</span><br><br>

            If you have any questions, feel free to reach out!
        </div>

       
            <div class=""header"" style=""color:orange"">My Jyotish G</div>
            <h4>www.myjyotishg.in</h4>
            <h4>myjyotishg@gmail.com</h4>
            <h4>7985738804</h4>
            
        
    </div>
</body>
</html>
";
            string Subject = "Verification Code for My Jyotish G";
            var isMailSend = SendEmail(message, email, Subject);
            if (isMailSend)
            {
                res.Otp = Otp;
                var gres = sendBy == "client" ? _context.Users.Update(res) : _context.JyotishRecords.Update(res);

                var result = _context.SaveChanges();
                if (result > 0)
                { return true;
                }
                else { return false; }
            }
            else { return false; }
        }
       
        public bool verifyOtps(string email,int otp,string sendBy)
        {
            dynamic res = null;
            if (sendBy == "client")
            {
                res = _context.Users.Where(e => e.Email == email && e.Otp == otp).FirstOrDefault();

            }
            else
            {

                res = _context.JyotishRecords.Where(e => e.Email == email && e.Otp==otp).FirstOrDefault();
            }
            if (res == null)
            {
                return false;
            }
            else
            {
               return true;
            }
        }

        public bool changePassword(string email,string password,string sendBy)
        {
            dynamic res = null;
            if (sendBy == "client")
            {
                res = _context.Users.Where(e => e.Email == email ).FirstOrDefault();

            }
            else
            {

                res = _context.JyotishRecords.Where(e => e.Email == email ).FirstOrDefault();
            }
            if (res == null)
            {
                return false;
            }
            else
            {
                res.Password = password;
                var gres=sendBy == "client" ? _context.Users.Update(res) : _context.JyotishRecords.Update(res);
                sendChangePasswordMail(email, password);
                return _context.SaveChanges() > 0;
            }
        }
         public bool changePasswordByOldPassword(int userId,string password,string oldPassword,string sendBy)
        {
            dynamic res = null;
            if (sendBy == "client")
            {
                res = _context.Users.Where(e => e.Id==userId && e.Password == oldPassword).FirstOrDefault();

            }
            else
            {

                res = _context.JyotishRecords.Where(e => e.Id==userId && e.Password == oldPassword).FirstOrDefault();
            }
            if (res == null)
            {
                return false;
            }
            else
            {
                res.Password = password;
                var gres=sendBy == "client" ? _context.Users.Update(res) : _context.JyotishRecords.Update(res);
                sendChangePasswordMail(res.Email, password);
                return _context.SaveChanges() > 0;
            }
        }

        public bool sendChangePasswordMail(string email, string password)
        {
            string message = $@"
           <!DOCTYPE html>
            <html lang=""en"">
            <head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>My Jyotish G Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 8px;
            background-color: #f9f9f9;
        }}
        .header {{
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
        }}
        .content {{
            font-size: 16px;
            line-height: 1.5;
            margin-bottom: 20px;
        }}
        .otp {{
            font-size: 20px;
            font-weight: bold;
            color: #000;
        }}
        .logo {{
            margin: 20px 0;
            text-align: center;
        }}
        .logo img {{
            max-width: 150px;
        }}
        .footer {{
            font-size: 14px;
            color: #555;
            margin-top: 20px;
        }}
        .footer a {{
            color: #000;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
       
         <div class=""logo"">
            <img src=""https://api.myjyotishg.in/Images/Logo.png"" alt=""My Jyotish G Logo"">
        </div>
        <div class=""content"">
           Dear Jyotish ,<br/>
            Your Password has been successfully Updated , <br/>
            Email: {email}<br/>
            Password: {password}<br/>
            If you have any questions, feel free to reach out!<br/>
        </div>

       
            <div class=""header"" style=""color:orange"">My Jyotish G</div>
            <h4>www.myjyotishg.in</h4>
            <h4>myjyotishg@gmail.com</h4>
            <h4>7985738804</h4>
            
        
    </div>
</body>
</html>
";
            string Subject = "Password Change Successfully";
            var isMailSend = SendEmail(message, email, Subject);
            return isMailSend;
        }

    }
}
