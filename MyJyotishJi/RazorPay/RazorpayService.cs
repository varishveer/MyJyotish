using BusinessAccessLayer.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using MyJyotishGApi.Controllers;
using Razorpay.Api;

namespace MyJyotishGApi.RazorPay
{
    public class RazorpayService
    {


        private readonly RazorpaySettings _razorpaySettings;
        private readonly IRazorPayServices _services;
        public RazorpayService(IOptions<RazorpaySettings> razorpaySettings, IRazorPayServices razorPayServices)
        {
            _razorpaySettings = razorpaySettings.Value;
            _services = razorPayServices;
        }

        // Method to create a new Razorpay order
        public Order CreateOrder(PaymentCreateOrderViewModel model)
        {
            string currency = "INR";
            // Set the API key and secret
            RazorpayClient client = new RazorpayClient(_razorpaySettings.Key, _razorpaySettings.Secret);


            // Convert the amount to the smallest currency unit (e.g., paise)
            Dictionary<string, object> options = new Dictionary<string, object>
            {
                { "amount", (int)(model.Amount * 100) }, // Amount in paise
                { "currency", currency },
                { "payment_capture", "1" }
            };

          
            // Create and return the order
            var response = client.Order.Create(options);
            model.OrderId = response["id"].ToString();
            var result = _services.Order(model );
            if(result )
            { return response; }
            else { return null; }
        }

        
        public bool VerifyPaymentStatus(string orderId, string paymentId, string signature)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string> {
                { "razorpay_order_id", orderId },
                { "razorpay_payment_id", paymentId },
                { "razorpay_signature", signature }
            };

            try
            {
                Utils.verifyPaymentSignature(attributes);  
                return true;
            }
            catch (Exception ex)
            {
                // You can log the error for debugging purposes
                Console.WriteLine($"Payment verification failed: {ex.Message}");
                return false;
            }
        }





        public Payment CapturePayment(PaymentCaptureModel model)
        {
            RazorpayClient client = new RazorpayClient(_razorpaySettings.Key, _razorpaySettings.Secret);

            // Verify the payment signature before capturing
            if (!VerifyPaymentStatus(model.OrderId, model.PaymentId, model.Signature))
            {
                throw new InvalidOperationException("Payment verification failed.");
            }

            // Fetch the payment details from Razorpay
            Payment payment = client.Payment.Fetch(model.PaymentId);

            // Ensure the payment is authorized before capturing
            if (payment["status"] == "captured")
            {
                // Payment is already captured, log and return the captured payment
                var isPaymentCapturedUpdated = _services.LogPayment(model, "success");
                if (!isPaymentCapturedUpdated)
                {
                    throw new Exception("Failed to log the already captured payment in the database.");
                }

                return payment; // No need to capture again
            }

            // Ensure the payment is authorized before capturing
            if (payment["status"] != "authorized")
            {
                throw new InvalidOperationException("Only authorized payments can be captured.");
            }

            // Capture the payment
            Dictionary<string, object> options = new Dictionary<string, object>
    {
        { "amount", (int)(model.Amount * 100) } // Convert amount to paise
    };
            Payment capturedPayment = payment.Capture(options);

            // Log successful payment in the database
            var isPaymentUpdated = _services.LogPayment(model, "success");
            if (!isPaymentUpdated)
            {
                throw new Exception("Failed to log the successful payment in the database.");
            }

            return capturedPayment;
        }


       

    }
}
