using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppWebApi.Interfaces;
using TodoAppWebApi.Models;

namespace TodoAppWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITodoService todoService;

        public TodoController(UserManager<ApplicationUser> userManager, ITodoService todoService)
        {
            this.userManager = userManager;
            this.todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAllTodos()
        {
            var userId = UserId;
            var user = await userManager.FindByNameAsync(userId);

            var items = await todoService.GetAllTodos(user);

            if(items == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Failed", Message = "Error Occured" });
            }

            return Ok(new
            {
                todos = items,
                Status = "Success",
                Message = "Todos of " + userId + " Listed Successfully"
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetItemById(int id)
        {
            var item = await todoService.GetItemAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateItem([FromBody] Todo todo)
        {
            var userId = UserId;
            var user = await userManager.FindByNameAsync(userId);

            var todos = await todoService.AddTodosAsync(todo, user);

            if (todos == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Failed", Message = "Error Occured" });
            }
            return Ok(new
            {
                todo = todos,
                Status = "Success",
                Message = "Todo Added Successfully"
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> UpdateItemAsync([FromBody] Todo editTodo, int id)
        {
            var userId = UserId;
            var user = await userManager.FindByNameAsync(userId);

            var todo = await todoService.UpdateTodoAsync(editTodo, user, id);

            
            if (todo == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Failed", Message = "Error Occured" });
            }
            return Ok(new
            {
                todo,
                Status = "Success",
                Message = "Todo Updated Successfully"
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Todo>> DeleteTodoAsync(int id)
        {
            var userId = UserId;
            var user = await userManager.FindByNameAsync(userId);

            var todo = await todoService.DeleteTodoAsync(id, user);

            if (todo == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Failed", Message = "Error Occured" });
            }

            return Ok(new
            {
                todo,
                Status = "Success",
                Message = "Todo Deleted Successfully"
            });
        }
    }
}
