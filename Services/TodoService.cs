using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppWebApi.Interfaces;
using TodoAppWebApi.Models;

namespace TodoAppWebApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public TodoService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Todo>> GetAllTodos(ApplicationUser user)
        {
            return await applicationDbContext.Todos.Where(t => t.UserId == user.Id).ToArrayAsync();
        }

        public async Task<Todo> AddTodosAsync(Todo todo, ApplicationUser user)
        {
            todo.Title = todo.Title;
            todo.Description = todo.Description;
            todo.Status = todo.Status;
            todo.Created_at = DateTime.Now;
            todo.Updated_at = DateTime.Now;
            todo.UserId = user.Id;

            applicationDbContext.Todos.Add(todo);

            var saved = await applicationDbContext.SaveChangesAsync();
            var repositoryTodo = todo;
            Console.WriteLine(saved);
            if (saved > 0)
                return repositoryTodo;
            return null;
        }

        public async Task<Todo> UpdateTodoAsync(Todo editedTodo, ApplicationUser user, int Id)
        {
            var todo = await applicationDbContext.Todos.Where(t => t.Id == Id && t.UserId == user.Id).SingleOrDefaultAsync();

            if (todo == null) return null;

            todo.Title = editedTodo.Title;
            todo.Description = editedTodo.Description;
            todo.Status = editedTodo.Status;
            todo.Updated_at = DateTime.Now;

            var saved = await applicationDbContext.SaveChangesAsync();
            var repositoryTodo = todo;
            Console.WriteLine(saved);
            if (saved > 0)
                return repositoryTodo;
            return null;
        }

        public async Task<Todo> GetItemAsync(int Id)
        {
            return await applicationDbContext.Todos.Where(t => t.Id == Id).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Todo>> DeleteTodoAsync(int Id, ApplicationUser user)
        {
            var todo = await applicationDbContext.Todos.Where(t => t.Id == Id && t.UserId == user.Id).SingleOrDefaultAsync();

            applicationDbContext.Todos.Remove(todo);

            var deleted = await applicationDbContext.SaveChangesAsync();

            var todos = await applicationDbContext.Todos.ToArrayAsync();

            return todos;
        }
    }
}
