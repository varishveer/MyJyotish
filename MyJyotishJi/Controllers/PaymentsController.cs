using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using MyJyotishGApi.RazorPay;
using Razorpay.Api;

namespace MyJyotishGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        private readonly RazorpayService _razorpayService;
        private readonly IRazorPayServices _services;
        private readonly IConfiguration _configuration;
        public PaymentsController(RazorpayService razorpayService, IRazorPayServices services, IConfiguration configuration)
        {
            _razorpayService = razorpayService;
            _services = services;
            _configuration = configuration;
        }

        // Create an order
        [HttpPost("create-order")]
        public IActionResult CreateOrder(PaymentCreateOrderViewModel model)
        {
            try
            {
                Order order = _razorpayService.CreateOrder(model);

               // return Ok(new { orderId = order["id"], amount = order["amount"], currency = order["currency"] });
               if(order == null)
                {
                    return Ok(new { status = 404, message = "User not found" });
                }
                var response = new
                {
                    id = order["id"].ToString(),
                    entity = order["entity"].ToString(),
                    amount = Convert.ToInt32(order["amount"]),
                    amount_paid = Convert.ToInt32(order["amount_paid"]),
                    amount_due = Convert.ToInt32(order["amount_due"]),
                    currency = order["currency"].ToString(),
                    receipt = order["receipt"]?.ToString(),
                    status = order["status"].ToString(),
                    attempts = Convert.ToInt32(order["attempts"]),
                    created_at = Convert.ToInt64(order["created_at"]),
                    offer_id = order["offer_id"]?.ToString(),
                    notes = order["notes"] as List<object>,
                    secretKey=_configuration["Razorpay:Key"]
            };
                return Ok(new {status =200, data =response });
            }       
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Capture payment
        [HttpPost("capture-payment")]
        public IActionResult CapturePayment([FromBody] PaymentCaptureModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Handle successful payments
                if (model.Status == "success")
                {
                    var payment = _razorpayService.CapturePayment(model); // Capture and log success
                    return Ok(new { message = "Payment captured and recorded.", payment });
                }

                // Handle failed payments
                if (model.Status == "failed")
                {
                    var isLogged = _services.LogFailedPayment(model); // Log failure
                    if (isLogged)
                    {
                        return Ok(new { message = "Payment failed and logged." });
                    }
                    return StatusCode(500, new { message = "Failed to log the payment failure." });
                }

                return BadRequest(new { message = "Invalid payment status." });
            }
            catch (Exception ex)
            {
                // Handle any unexpected exceptions
                return StatusCode(500, new { message = "An unexpected error occurred.", Error = ex.Message });
            }
        }

    }
    
}
