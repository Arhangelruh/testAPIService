using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.interfaces;
using Test.Models;

namespace Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIController : Controller
    {
        private ICardRepository Repository { get; set; }

        public APIController(ICardRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> Get()
        {
            var books = await Repository.GetCardsAsync();
            if (books != null)
            {
                return new ObjectResult(books);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Post(Book book)
        {
            if(book is null || book.Name == "" || book.Name is null)
            {
                return BadRequest();
            }

            await Repository.AddCardAsync(book);
            return Ok(book);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditCard(string id, Book book)
        {
            if (id is null || book.Name is null)
            {
                return NotFound("false");
            }
            
            var getBook = await Repository.GetCardByIdAsync(id);
            if (getBook is null)
            {
                return NotFound("false");
            }

            var newBook = new Book
            {
                Name = book.Name,
                Picture = book.Picture
            };

            var result = await Repository.EditCardAsync(id, newBook);
            if (result)
            {
                return Ok("true");
            }
            else
            {
                return NotFound("false");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCard(string id)
        {
            if (id is null)
            {
                return NotFound("false");
            }
            var getBook = await Repository.GetCardByIdAsync(id);
            if (getBook is null)
            {
                return NotFound("false");
            }

            var result = await Repository.DeleteCardAsync(id);
            if (result)
            {
                return Ok("true");
            }
            else
            {
                return NotFound("false");                
            }

        }
    }
}