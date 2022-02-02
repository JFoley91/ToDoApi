using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.API.Models;
using ToDoAPI.DATA.EF;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ToDoController : ApiController
    {

        ToDoEntities db = new ToDoEntities();
        public IHttpActionResult GetToDos()
        {
            List<ToDoViewModel> todos = db.TodoItems.Include("To Do").Select(t => new ToDoViewModel()
            {
                TodoId = t.TodoId,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId
            }).ToList<ToDoViewModel>();

            if (todos.Count == 0)
            {
                return NotFound();
            }
            return Ok(todos);
        }//end GetToDos

        public IHttpActionResult GetToDo(int id)
        {
            List<ToDoViewModel> todo = db.TodoItems.Include("To Do").Select(t => new ToDoViewModel()
            {
                TodoId = t.TodoId,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId
            }).ToList<ToDoViewModel>();

            if (todo.Count == 0)
            {
                return NotFound();
            }
            return Ok(todo);
        }//end GetToDo

        public IHttpActionResult PostToDo(ToDoViewModel todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Task Error");
            }

            TodoItem newTodo = new TodoItem()
            {
                Action = todo.Action,
                Done = todo.Done,
                CategoryId = todo.CategoryId
            };
            db.TodoItems.Add(newTodo);
            db.SaveChanges();
            return Ok(newTodo);
        }//end PostTodo

        public IHttpActionResult PutToDo(ToDoViewModel todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data Error");
            }

            TodoItem existingTodo = db.TodoItems.Where(t => t.TodoId == todo.TodoId).FirstOrDefault();

            if (existingTodo != null)
            {
                existingTodo.TodoId = todo.TodoId;
                existingTodo.Action = todo.Action;
                existingTodo.Done = todo.Done;
                existingTodo.CategoryId = todo.CategoryId;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//end PutTodo

        public IHttpActionResult DeleteToDo(int id)
        {
            TodoItem todo = db.TodoItems.Where(t => t.TodoId == id).FirstOrDefault();

            if (todo != null)
            {
                db.TodoItems.Remove(todo);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//end DeleteToDo

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);    
        }


    }//end class
}//end Namespace
