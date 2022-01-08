using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Author_API.DataAccess;
using Author_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Author_API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class BooksController : ControllerBase
    {
        private AuthorDbContext AuthorDbContext;

        public BooksController(AuthorDbContext authorDbContext)
        {
            AuthorDbContext = authorDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Book>>> GetBooks()
        {
            try
            {
                IList<Book> books = AuthorDbContext.Books.ToList();
                return Ok(books);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook([FromQuery] int authorId, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Author author = await AuthorDbContext.Authors.
                    Include(a => a.Books).
                    FirstAsync(author1 => author1.Id == authorId );
                author.Books.Add(book);
                await AuthorDbContext.Books.AddAsync(book);
                await AuthorDbContext.SaveChangesAsync();

                return Created($"/{author.Id}", book);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<Book>> DeleteBook([FromQuery] int bookId)
        {
            try
            {
                Book toDelete = await AuthorDbContext.Books.FindAsync(bookId);
                AuthorDbContext.Books.Remove(toDelete);
                await AuthorDbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}