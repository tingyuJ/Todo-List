using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TodoListWebAPI.Models;
using TodoListWebAPI.Services;

namespace TodoListWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class ListController : ControllerBase
    {
        private readonly ILogger<ListController> _logger;
        private DataAccess _db;

        public ListController(
            ILogger<ListController> logger,
            DataAccess db
        )
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<IActionResult> GetList(string username)
        {
            var list = await _db.GetList(username);
            return Ok(new JsonResult(new { data = list }));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateListItem(ListItemModel item)
        {
            await _db.UpdateListItem(item);
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> CreateListItem(ListItemModel item)
        {
            await _db.CreateListItem(item);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteListItem(ListItemModel item)
        {
            await _db.DeleteListItem(item);
            return Ok();
        }
    }
}