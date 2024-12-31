using Azure.Core;
using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelAccessLayer.ViewModels;
using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.TwiML;
using Twilio.Types;

namespace MyJyotishGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        [HttpGet("accessToken")]

        public IActionResult GenerateToken(string identity)
        {
            const string twilioAccountSid = "AC69120ba0ee6d3c27dc72e8d3385c798e";
            const string twilioApiKey = "SK9d52035ce4a96de77326d79095741a4d";
            const string twilioApiSecret = "nUZk65XltHpTQxSdqwhBEpkbwgZ6w4y7";
            TwilioClient.Init(twilioApiKey, twilioApiSecret);
            var grants = new HashSet<IGrant>
        {
            new VoiceGrant
            {
                IncomingAllow = true,
                OutgoingApplicationSid = "AP8e437be7b812411488baf1ff2335baa9"
            }
        };

            var token = new Token(
                twilioAccountSid,
                twilioApiKey,
                twilioApiSecret,
                identity,
                DateTime.UtcNow.AddMinutes(30),
                grants: grants
            );
            return Ok(new { status=200,message="Token generated successfully",token=token.ToJwt()});
        }

    }

}
