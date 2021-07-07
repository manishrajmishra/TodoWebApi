using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppWebApi.Models;

namespace TodoAppWebApi.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetAllTodos(ApplicationUser user);
        Task<Todo> AddTodosAsync(Todo todo, ApplicationUser applicationUser);

        Task<Todo> UpdateTodoAsync(Todo todo, ApplicationUser applicationUser, int Id);

        Task<Todo> GetItemAsync(int Id);

        Task<IEnumerable<Todo>> DeleteTodoAsync(int Id, ApplicationUser applicationUser);
    }
}
