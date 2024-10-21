using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelAccessLayer.ViewModels;

namespace MyJyotishGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        /*private readonly ICallServices _callService;

        public CallController(ICallServices callService)
        {
            _callService = callService;
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinCall([FromBody] CallViewModel model)
        {
            await _callService.JoinCallAsync(model.RoomName, model.UserId);
            return Ok(new { message = "Joined successfully" });
        }

        [HttpPost("leave")]
        public async Task<IActionResult> LeaveCall([FromBody] CallViewModel model)
        {
            await _callService.LeaveCallAsync(model.RoomName, model.UserId);
            return Ok(new { message = "Left successfully" });
        }*/
    }
}
