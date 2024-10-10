using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyJyotishGApi.RazorPay;
using Razorpay.Api;

namespace MyJyotishGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        private readonly RazorpayService _razorpayService;

        public PaymentsController(RazorpayService razorpayService)
        {
            _razorpayService = razorpayService;
        }

        // Create an order
        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] decimal amount)
        {
            try
            {
                Order order = _razorpayService.CreateOrder(amount);
                return Ok(new { orderId = order["id"], amount = order["amount"], currency = order["currency"] });
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
            try
            {
                var payment = _razorpayService.CapturePayment(model.PaymentId, model.Amount);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
    public class PaymentCaptureModel
    {
        public string PaymentId { get; set; }
        public decimal Amount { get; set; }
    }
}
