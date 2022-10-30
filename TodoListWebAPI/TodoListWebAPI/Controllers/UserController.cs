using Microsoft.AspNetCore.Authorization;
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
        private DataAccess _db;
        private JwtGenerator _jwtGenerator;

        public UserController(
            ILogger<UserController> logger,
            DataAccess db,
            JwtGenerator jwtGenerator)
        {
            _logger = logger;
            _db = db;
            _jwtGenerator = jwtGenerator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogInOrSignUp(UserModel form)
        {
            var user = await _db.GetUser(form.UserName);
            if (user.Count == 0)
            {
                //sign up
                await _db.CreateUser(form);
                var data = new { 
                    userName = form.UserName,
                    token = _jwtGenerator.GenerateJwtToken(form.UserName)
                };
                return Ok(new JsonResult(new { data = data }));
            }
            else
            {
                var username = user.First().Password == form.Password ? user.First().UserName : null;
                var data = new
                {
                    userName = form.UserName,
                    token = _jwtGenerator.GenerateJwtToken(form.UserName)
                };
                return Ok(new JsonResult(new { data = data }));
            }
        }
    }
}