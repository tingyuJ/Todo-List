using Microsoft.AspNetCore.Mvc;
using TodoListWebAPI.Models;
using TodoListWebAPI.Services;

namespace TodoListWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private DataAccess db = new DataAccess();

        public UserController(
            ILogger<UserController> logger
        )
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> LogInOrSignUp(UserModel form)
        {
            var user = await db.GetUser(form.UserName);
            if (user.Count == 0)
            {
                //sign up
                await db.CreateUser(form);
                return Ok(new JsonResult(new { data = form.UserName }));
            }
            else
            {
                var username = user.First().Password == form.Password ? user.First().UserName : null;
                return Ok(new JsonResult(new { data = username }));
            }
        }
    }
}