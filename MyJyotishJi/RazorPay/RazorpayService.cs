using Microsoft.Extensions.Options;
using Razorpay.Api;

namespace MyJyotishGApi.RazorPay
{
    public class RazorpayService
    {


        private readonly RazorpaySettings _razorpaySettings;

        public RazorpayService(IOptions<RazorpaySettings> razorpaySettings)
        {
            _razorpaySettings = razorpaySettings.Value;
        }

        // Method to create a new Razorpay order
        public Order CreateOrder(decimal amount, string currency = "INR")
        {
            // Set the API key and secret
            RazorpayClient client = new RazorpayClient(_razorpaySettings.Key, _razorpaySettings.Secret);

            // Convert the amount to the smallest currency unit (e.g., paise)
            Dictionary<string, object> options = new Dictionary<string, object>
        {
            { "amount", (int)(amount * 100) }, // Amount in paise
            { "currency", currency },
            { "payment_capture", "1" }
        };

            // Create and return the order
            return client.Order.Create(options);
        }

        // Method to capture payment
        public Payment CapturePayment(string paymentId, decimal amount)
        {
            RazorpayClient client = new RazorpayClient(_razorpaySettings.Key, _razorpaySettings.Secret);

            // Capture the payment
            Payment payment = client.Payment.Fetch(paymentId);
            Dictionary<string, object> options = new Dictionary<string, object>
        {
            { "amount", (int)(amount * 100) } // Amount in paise
        };
            return payment.Capture(options);
        }
    }
}
