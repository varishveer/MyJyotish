using BusinessAccessLayer.Abstraction;
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
        public string JRegisterAndSendOtp(string Email)
        {
            var IsMobileValid = _context.JyotishRecords.Where(x => x.Email == Email).FirstOrDefault();
            
            if (IsMobileValid != null)
            { 
                if(IsMobileValid.Status == "Unverified" || IsMobileValid.Status == "Verified")
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
                    if(isNewMailSend)
                    {IsMobileValid.Otp = NewOtp;

                        IsMobileValid.Status = "Unverified";
                        _context.JyotishRecords.Update(IsMobileValid);
                    var NewResult = _context.SaveChanges();
                        if (NewResult > 0)
                        { return "Successful"; }
                        else { return "Data not saved"; }
                    }
                    else { return "Data not saved"; }
                }
                return "Email Number already existed"; 
            }
            int Otp = new Random().Next(100000, 1000000); ;
            JyotishModel model = new JyotishModel();
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
            model.Status = "Unverified";
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
                    if(record.Status == "Unverified")
                    {
                        record.Status = "Verified";
                    }
                    else { return "Email already Verified"; }
                   
                    record.Password= Guid.NewGuid().ToString("N").Substring(0, 8);
                    _context.JyotishRecords.Update(record);

                    var result = _context.SaveChanges();

                    if (result > 0) {

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
        .password {{
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
            Thank you for signing up! Your account has been successfully created. Below is your  password.<br><br>

             Password: <span class=""password"">{record.Password}</span><br><br>

            If you encounter any issues or have any questions, feel free to reach out to us.
        </div>

        <div class=""header"" style=""color:orange"">My Jyotish G</div>
        <h4>www.myjyotishg.in</h4>
        <h4>myjyotishg@gmail.com</h4>
        <h4>7985738804</h4>
    </div>
</body>
</html>
";
                        string subject = "Your Password for Jyotish G";
                        SendEmail(message, record.Email, subject);
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
        public string SignUpJyotish(JyotishViewModel jyotishView , string path)
        {
            
           
            var Jyotish = _context.JyotishRecords.Where(x => x.Email == jyotishView.Email).FirstOrDefault();
            if (Jyotish == null )
            { return "Email Number not found"; }

            if( Jyotish.Status != "Verified")
            { return "Not authorized to register"; }
            var CountryName = _context.Countries.Where(x => x.Id == jyotishView.Country).FirstOrDefault();
            var StateName = _context.States.Where(x => x.Id == jyotishView.State).FirstOrDefault();
            var CityName = _context.Cities.Where(x => x.Id == jyotishView.City).FirstOrDefault();

            Jyotish.Name = jyotishView.Name;

            Jyotish.Mobile = jyotishView.Mobile;
            Jyotish.Gender = jyotishView.Gender;
            Jyotish.Language = jyotishView.Language;
            Jyotish.Expertise = jyotishView.Expertise;
            Jyotish.Country = CountryName.Name;
            Jyotish.State = StateName.Name;
            Jyotish.City = CityName.Name;
         


            Jyotish.Role = "Pending";
            Jyotish.Status = "Pending";
            string uploadFolder = path + "/wwwroot/Images/Jyotish";
            string imageName = Guid.NewGuid().ToString("N").Substring(0, 8) + jyotishView.Image.FileName  ;
            Jyotish.ProfileImageUrl = "/Images/Jyotish/" + imageName;

            var filePath = Path.Combine(uploadFolder,imageName);
           
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                jyotishView.Image.CopyTo(stream);
            }

            _context.JyotishRecords.Update(Jyotish);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                

                var message = "Dear Jyotish ," +
                    "/n Your account has been successfully created and your Credential are below , \n Email : " + Jyotish.Email + "\n Password: " + Jyotish.Password;
                var subject = "MyJyotishG Account Credential";
                SendEmail(message, Jyotish.Mobile, subject);
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
                       Role = _Jyotish.Role

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
                    return "Login Successful";
                }
                else
                {
                    return "Incorrect Password";
                }
            }
            else
            { return "Invalid Email"; }
        }
        #endregion

        #region User
        public string RegisterUserEmail(string Email)
        {
            var record = _context.Users.FirstOrDefault(x => x.Email == Email);

            if (record != null)
            {
                if (record.Status == "Unverified")
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
                return "Email already exist";
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
                Status = "Unverified"
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
            if(user.Status != "Unverified") { return "Unauthorized User"; }
            if(user.Otp == Otp) 
            {
                user.Status = "Verified";
                user.Password = (new Random().Next(10000000, 100000000)).ToString();
                _context.Users.Update(user);
                _context.SaveChanges();

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
        .password {{
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
            Thank you for signing up! Your account has been successfully created. Below is your  password.<br><br>

             Password: <span class=""password"">{user.Password}</span><br><br>

            If you encounter any issues or have any questions, feel free to reach out to us.
        </div>

        <div class=""header"" style=""color:orange"">My Jyotish G</div>
        <h4>www.myjyotishg.in</h4>
        <h4>myjyotishg@gmail.com</h4>
        <h4>7985738804</h4>
    </div>
</body>
</html>
";
                string subject = "Your Password for Jyotish G";
                SendEmail(message, user.Email, subject);
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
            if(record.Status != "Verified") { return "Unauthorized"; }

            if (_user.Mobile != null)
            { record.Mobile = _user.Mobile; }
            if (_user.Name != null)
            { record.Name = _user.Name; }
            if (_user.Gender != null)
            { record.Gender = _user.Gender; }
            if (_user.DoB != null)
            { record.DoB = _user.DoB; }
            if (_user.PlaceOfBirth != null)
            { record.PlaceOfBirth = _user.PlaceOfBirth; } 
               
           
            _context.Users.Update(record);
            var result = _context.SaveChanges();
            if(result>0)
            {
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

        public static bool SendEmail(string MessageBody, string Mail, string Subjectbody)
        {
            try
            {
                // Sender's email address and app-specific password
                string senderEmail = "varishveer123@gmail.com";
                string senderPassword = "yngh qauy rtkg zxzy"; // Ensure this is an app-specific password if 2FA is enabled

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

        public List<string> PlaceOfBirthList(string CityName)
        {
            CityName = char.ToUpper(CityName[0]) + CityName.Substring(1).ToLower();
            var Countries = _context.Countries.ToList();
            var States = _context.States.ToList();
            var Cities = _context.Cities.ToList();

            var Table = from Country in Countries
                        join State in States on Country.Id equals State.CountryId
                        join City in Cities on State.Id equals City.StateId
                        select new
                        {
                            CityName = City.Name,
                            StateName = State.Name,
                            CountryName = Country.Name
                        };

            var Records = Table
                            .Where(x => x.CityName.Contains(CityName)) 
                            .OrderByDescending(x => x.CityName.StartsWith(CityName)) 
                            .ThenBy(x => x.CityName) 
                            .ThenBy(x => x.StateName) 
                            .ThenBy(x => x.CountryName) 
                            .Select(x => $"{x.CityName}, {x.StateName}, {x.CountryName}")
                            .ToList();

            return Records;
        }
        public List<string> LanguageList()
        {
            var Languages = _context.Languages.Select(l => l.Name).ToList();
            return Languages;
        }


    }
}
