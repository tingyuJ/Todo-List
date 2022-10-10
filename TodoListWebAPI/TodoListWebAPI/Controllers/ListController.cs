using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TodoListWebAPI.Models;
using TodoListWebAPI.Services;

namespace TodoListWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ListController : ControllerBase
    {
        private readonly ILogger<ListController> _logger;
        private DataAccess db = new DataAccess();

        public ListController(
            ILogger<ListController> logger
        )
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<IActionResult> GetList(string username)
        {
            var list = await db.GetList(username);
            return Ok(new JsonResult(new { data = list }));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateListItem(ListItemModel item)
        {
            await db.UpdateListItem(item);
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> CreateListItem(ListItemModel item)
        {
            await db.CreateListItem(item);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteListItem(ListItemModel item)
        {
            await db.DeleteListItem(item);
            return Ok();
        }
    }
}