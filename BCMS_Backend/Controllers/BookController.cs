using BCMS_Backend.Entities;
using BCMS_Backend.Repository;
using Microsoft.AspNetCore.Mvc;


namespace BCMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        // probably I regret using async conjuration wizardry here. Anyway, it's worth trying.
        // for example if something gets bad with Get() I may just remove Task and put Result after GetAllAsync() . . .
        readonly BookRepository _bookRepository = new BookRepository();
        // get all books
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            return _bookRepository.GetAllBooksExtended().Result;
        }
        // get single book
        [HttpGet("{id}")]
        public Task<Book> Get(int id)
        {
            return _bookRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// add new Book and return added instance
        /// </summary>
        /// <param name="inBook">book to add but without specified Id</param>
        /// <returns>added book but with specified Id</returns>
        [HttpPost]
        public Task<Book> Post([FromBody] Book inBook)
        {
            return _bookRepository.InsertAsync(inBook, false);
        }

        /// <summary>
        /// save edits of a book entry
        /// </summary>
        /// <param name="id">not used, id should be specified in 'value' parameter</param>
        /// <param name="value">new value of Book to save, with target id</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Book value)
        {
            try
            {
                await _bookRepository.UpdateAsync(value);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// delete a book completely, by ID
        /// </summary>
        /// <param name="id">valid id of a book. it should exist.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bookRepository.DeleteByIdAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
