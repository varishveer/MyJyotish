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
                    int NewOtp = 123456;
                   
                    string NewMessage = "Dear User,\r\n\r\nThank you for choosing My Jyotish G! To ensure the security of your account, we need to verify your identity.\r\n\r\nYour One-Time Password (OTP) is: " + NewOtp + "\r\n\r\nPlease enter this OTP within the next 10 minutes. If you did not request this code, please disregard this email.\r\n\r\nIf you have any questions or need assistance, feel free to contact our support team.\r\n\r\nThank you for being a valued part of the My Jyotish G community!\r\n\r\nBest regards,\r\nThe My Jyotish G Team\r\n\r\n";
                    string NewSubject = "Verification Code for My Jyotish G";

                    var isNewMailSend = SendEmail(NewMessage, Email, NewSubject);
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
            int Otp = 123456;
            JyotishModel model = new JyotishModel();
            string Message = "Dear User,\r\n\r\nThank you for choosing My Jyotish G! To ensure the security of your account, we need to verify your identity.\r\n\r\nYour One-Time Password (OTP) is: "+ Otp +"\r\n\r\nPlease enter this OTP within the next 10 minutes. If you did not request this code, please disregard this email.\r\n\r\nIf you have any questions or need assistance, feel free to contact our support team.\r\n\r\nThank you for being a valued part of the My Jyotish G community!\r\n\r\nBest regards,\r\nThe My Jyotish G Team\r\n\r\n";
            string Subject = "Verification Code for My Jyotish G";

            var isMailSend =SendEmail(Message,Email,Subject);
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


                    _context.JyotishRecords.Update(record);
                    var result = _context.SaveChanges();

                    if (result > 0) { return "Successful"; }
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
            Jyotish.Password = Guid.NewGuid().ToString("N").Substring(0, 8);


            Jyotish.Role = "Pending";
            Jyotish.Status = "Offline";
            string uploadFolder = path + "/wwwroot/Images/Jyotish";
            string imageName = Guid.NewGuid().ToString("N").Substring(0, 8) + jyotishView.Image.FileName  ;
            Jyotish.ProfileImageUrl = imageName;

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
            var _pJyotish = _context.JyotishRecords.Where(x => x.Email == Email).FirstOrDefault();
            if (_pJyotish != null)
            {
                if (_pJyotish.Password == password)
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

        public string JUserName(string Mobile)
        {
            var model = _context.JyotishRecords.Where(x => x.Mobile == Mobile).FirstOrDefault();
            if (model == null)
            { return null; }
            else { return model.Name; }

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
        public string RegisterUserMobile(string  mobile)
        {
            var record = _context.Users.Where(x=>x.Mobile == mobile).FirstOrDefault();
            if(record != null) { 
                if(record.Status == "Unverified")
                {
                    var newOtp = SendOtp(mobile);
                    if (newOtp == 0 || newOtp == null) { return "Otp not sent"; }
                    record.Otp = newOtp;
                    _context.Users.Update(record);
                    var newResult = _context.SaveChanges();
                    if (newResult > 0) { return "Successfull"; }
                    else { return "Data not saved"; }
                }
                return "Mobile Number already exist";
            }
                
            var Otp = SendOtp(mobile);
            if (Otp == 0 || Otp ==  null) { return "Otp not sent"; }
            UserModel _user = new UserModel()
            { 
                Mobile= mobile,
                Otp = Otp,
                Status = "Unverified"
            };
            _context.Users.Add(_user);
            var result = _context.SaveChanges();
            if (result > 0) { return "Successfull"; }
            else { return "Data not saved"; }
        }

        public string VerifyUserOtp(string Mobile, int Otp)
        {
            if(Mobile == null || Otp ==0)
            {
                return "Invalid Data";
            }
            var user = _context.Users.Where(x=>x.Mobile == Mobile).FirstOrDefault();
            if(user == null) { return "User not found"; }
            if(user.Status != "Unverified") { return "Unauthorized User"; }
            if(user.Otp == Otp) 
            {
                user.Status = "Verified";
                _context.Users.Update(user);
                _context.SaveChanges();
                return "Successful"; 
            }
            else { return "Invalid Otp"; }
        }
        public string RegisterUserDetails(UserViewModel _user)
        {
            var record = _context.Users.Where(x=>x.Mobile == _user.Mobile).FirstOrDefault();
            if(record == null)
            {
                return "Mobile Number Not Registered";
            }
            if(record.Status != "Verified") { return "Unauthorozed"; }
            if (_user.Name != null)
            { record.Name = _user.Name; }
            if (_user.Gender != null)
            { record.Gender = _user.Gender; }
            if (_user.DoB != null)
            { record.DoB = _user.DoB; }
            if (_user.PlaceOfBirth != null)
            { record.PlaceOfBirth = _user.PlaceOfBirth; } 
                record.Password = (new Random().Next(10000000, 100000000)).ToString();
           
            _context.Users.Update(record);
            var result = _context.SaveChanges();
            if(result>0)
            {
                SendUserPassword(record.Mobile, record.Password);
                return "Successful";
            }
            else { return "Data not saved"; }
        }
        public bool SendUserPassword(string Mobile, string password)
        {
            return true;
        }

        public string LoginUser(LoginViewModel model)
        {
            if (model.Mobile == null || model.Password == null) { return "Invalid Credential"; }
            else
            {
                var record = _context.Users.Where(x => x.Mobile == model.Mobile).FirstOrDefault();
                if(record == null)
                { return "Invalid Number"; }
                else 
                {
                    if(record.Password == model.Password)
                    { return "Successful Login"; }
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
                // Sender's email address and app-specific password
                string senderEmail = "variveersingh123@gmail.com";
                string senderPassword = "htjp emoj tahk qqaj"; // Ensure this is an app-specific password if 2FA is enabled

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

        public int SendOtp(string Mobile)
        {

            return 123456;
        }
        public bool SendPJPassword(string message,string Mobile ,string  subject)
        {
            return true;
        }
    }
}
