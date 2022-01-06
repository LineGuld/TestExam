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
    public class AuthorController : ControllerBase
    {
        private AuthorDbContext AuthorDbContext;

        public AuthorController(AuthorDbContext authorDbContext)
        {
            AuthorDbContext = authorDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Author>>> GetAuthors()
        {
            try
            {
                IList<Author> authors = AuthorDbContext.Authors.Include(a => a.Books).ToList();
                return Ok(authors);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddAuthor([FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               await AuthorDbContext.AddAsync(author);
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