using BusinessAccessLayer.Abstraction;
using DataAccessLayer.DbServices;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Implementation
{
    public class RazorPayServices:IRazorPayServices
    {
        private readonly ApplicationContext _context;
        private readonly IJyotishServices _jyotish;
        private readonly IUserServices _user;

        public RazorPayServices(ApplicationContext context, IJyotishServices jyotish,IUserServices user)
        {
            _context = context;
            _jyotish = jyotish;
            _user = user;
        }
        public bool Order(PaymentCreateOrderViewModel model)
        {
            var jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            var user = _context.Users.Where(x => x.Id == model.UserId).FirstOrDefault();
            if (jyotish != null)
            {


                JyotishPaymentRecordModel record = new JyotishPaymentRecordModel();
                record.JyotishId = model.JyotishId;
                record.Amount = model.Amount.ToString();
          
                record.DateTime = DateTime.Now;
                record.Status = "Pending";
                record.OrderId = model.OrderId;

                _context.JyotishPaymentRecord.Add(record);
                if (_context.SaveChanges() > 0) { return true; }
                else { return false; }

            }
            else if (user != null) 
            {
                UserPaymentRecordModel record = new UserPaymentRecordModel();
                record.UserId = model.UserId;
                record.Amount = model.Amount.ToString();
             
                record.DateTime = DateTime.Now;
                record.Status = "Pending";
                record.OrderId = model.OrderId;

                _context.UserPaymentRecord.Add(record);
                if (_context.SaveChanges() > 0) { return true; }
                else { return false; }
            }
            else
            {
                return false;
            }
           
        }

        
        public bool LogPayment(PaymentCaptureModel model, string status)
        {
            // Find the payment record in Jyotish or User payment records
            var jyotish = _context.JyotishPaymentRecord.FirstOrDefault(x => x.OrderId == model.OrderId);
            var user = _context.UserPaymentRecord.FirstOrDefault(x => x.OrderId == model.OrderId);

            if (jyotish != null)
            {
                JyotishWalletViewmodel jmodel = new JyotishWalletViewmodel
                {
                    jyotishId = (int)model.JyotishId,
                    WalletAmount = (long)model.Amount
                };
               var res= _jyotish.AddWallet(jmodel);
                if (res == "Successful")
                {
                    WalletHistoryViewmodel js = new WalletHistoryViewmodel
                    {
                        JId = (int)model.JyotishId,
                        amount = (long)model.Amount,
                        PaymentId = model.PaymentId,
                        PaymentAction = "Credit",
                        PaymentStatus = "success",
                        PaymentFor = "Add to wallet"
                    };
                    var historyres = _jyotish.AddWalletHistory(js);
                }
                // Update Jyotish payment record with the new status and payment details
                jyotish.Status = status; // "success" or "failed"
                jyotish.OrderId = model.OrderId;
                jyotish.PaymentId = model.PaymentId;
                jyotish.Message = model.Message;
                jyotish.Method = model.Method;
                if (!string.IsNullOrEmpty(model.SignatureId))
                {
                    jyotish.SignatureId = model.SignatureId;
                }
                else { 
                }

                _context.JyotishPaymentRecord.Update(jyotish); // Update the Jyotish payment record
                return _context.SaveChanges() > 0; // Save changes and return true if successful
            }
            else if (user != null)
            {
                UserWalletViewmodel umodel = new UserWalletViewmodel
                {
                    userId = (int)model.UserId,
                    WalletAmount = (long)model.Amount
                };
                var res =_user.AddUserWallets(umodel);
                if (res == "Successful")
                {
                    WalletHistoryViewmodel js = new WalletHistoryViewmodel
                    {
                        UId = (int)model.UserId,
                        amount = (long)model.Amount,
                        PaymentId=model.PaymentId,
                        PaymentAction = "Credit",
                        PaymentStatus = "success",
                        PaymentFor = "Add to wallet"
                    };
                    var historyres = _user.AddWalletHistory(js);
                }
                // Update User payment record with the new status and payment details
                user.Status = status; // "success" or "failed"
                user.OrderId = model.OrderId;
                user.PaymentId = model.PaymentId;
                user.Message = model.Message;
                user.Method = model.Method;

                if (!string.IsNullOrEmpty(model.SignatureId))
                {
                    user.SignatureId = model.SignatureId;
                }

                _context.UserPaymentRecord.Update(user); // Update the User payment record
                return _context.SaveChanges() > 0; // Save changes and return true if successful
            }
            else
            {
                // No matching record found
                return false;
            }
        }

        public bool LogFailedPayment(PaymentCaptureModel model)
        {
            // Attempt to find payment record in Jyotish or User tables
            var jyotish = _context.JyotishPaymentRecord.FirstOrDefault(x => x.OrderId == model.OrderId);
            var user = _context.UserPaymentRecord.FirstOrDefault(x => x.OrderId == model.OrderId);

            if (jyotish != null)
            {
                jyotish.Status = "failed";
                jyotish.OrderId = model.OrderId;
                jyotish.PaymentId = model.PaymentId;
                jyotish.Message = model.Message;

                WalletHistoryViewmodel js = new WalletHistoryViewmodel
                {
                    JId = (int)model.JyotishId,
                    amount = (long)model.Amount,
                    PaymentId = model.PaymentId,
                    PaymentAction = "Credit",
                    PaymentStatus = "failed",
                    PaymentFor = "Add to wallet"
                };
               

                 _jyotish.AddWalletHistory(js);
                _context.JyotishPaymentRecord.Update(jyotish);
                 if(_context.SaveChanges() > 0)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            else if (user != null)
            {
                user.Status = "failed";
                user.OrderId = model.OrderId;
                user.PaymentId = model.PaymentId;
                user.Message = model.Message;

                WalletHistoryViewmodel js = new WalletHistoryViewmodel
                {
                    UId = (int)model.UserId,
                    amount = (long)model.Amount,
                    PaymentId = model.PaymentId,
                    PaymentAction = "Credit",
                    PaymentStatus = "failed",
                    PaymentFor = "Add to wallet"
                };

                   _user.AddWalletHistory(js);
                _context.UserPaymentRecord.Update(user);
                if (_context.SaveChanges() > 0)
                {

                    return true;

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false; // No matching record found
            }
        }
    }
}
