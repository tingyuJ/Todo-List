using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListWebAPI.Common;
using TodoListWebAPI.Interfaces;
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
        private IDataAccess _db;

        public ListController(
            ILogger<ListController> logger,
            IDataAccess db
        )
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<Response> GetList(string username)
        {
            try
            {
                var list = await _db.GetList(username);
                return new Response(list);
            } 
            catch (Exception ex)
            {
                return new Response(500, "Error", ex);
            }
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