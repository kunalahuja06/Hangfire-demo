using Hangfire;
using HangFire.Application.Contracts;
using HangFire.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace HangFire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController ( IUserService userService )
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Add ( User user )
        {
            var res = await _userService.Add(user);

            if ( res != null )
            {
                BackgroundJob.Enqueue<IMailService>(x => x.SendAsync(new MailRequest()
                {
                    Subject = "User Added" ,
                    Body = "<p> Hi [user], <br> Welcome to our platform</p>" ,
                    Recipient = new Recipient() { Email = user.Email , Name = user.Name } ,
                    Placeholders = new Dictionary<string , string>() { { "user" , user.Name } }
                }));
            }

            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete ( Guid id )
        {
            var res = await _userService.Delete(id);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get ( Guid id )
        {
            var res = await _userService.Get(id);
            return Ok(res);
        }
    }
}
