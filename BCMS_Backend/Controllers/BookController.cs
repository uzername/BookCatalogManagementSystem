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
            return _bookRepository.GetAllAsync().Result;
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
        /// save edits to a book
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// delete a book completely, by ID
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
