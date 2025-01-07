using BCMS_Backend.Entities;
using BCMS_Backend.Helpers;
using BCMS_Backend.Repository;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;


namespace BCMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        // probably I regret using async conjuration wizardry here. Anyway, it's worth trying.
        // for example if something gets bad with Get() I may just remove Task and put Result after GetAllAsync() . . .
        readonly BookRepository _bookRepository = new BookRepository();
        readonly CategoryRepository _categoryRepository = new CategoryRepository();
        /// <summary>
        /// get all books with their category / genre
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            return _bookRepository.GetAllBooksExtended().Result;
        }
        /// <summary>
        /// Special API method for UI. get list of books but prepared to show with pagination and also with filtering applied.
        /// https://dev.to/drsimplegraffiti/pagination-in-net-api-4opp .
        /// https://unitcoding.com/filtering-your-web-api-resources/ .
        /// </summary>
        /// <param name="controlValues"></param>
        /// <returns></returns>

        [HttpPost("/forUI")]
        public PaginatedList<Book> GetBooksWithPaginationAndFiltering([FromBody] QueryParameters controlValues)
        {
            return _bookRepository.GetAllBooksExtendedFilteredPagination(controlValues).Result;
        }
        /// <summary>
        /// Upload list from CSV file. 
        /// upload file: https://kenslearningcurve.com/tutorials/upload-files-to-api-with-c-and-net-core/ .
        /// get data from csv file https://joshclose.github.io/CsvHelper/getting-started/ .
        /// csv file formatting to consider https://stackoverflow.com/questions/4617935/is-there-a-way-to-include-commas-in-csv-columns-without-breaking-the-formatting .
        /// </summary>
        /// <returns></returns>
        [HttpPost("/upload")]
        public async Task<IActionResult> Upload()
        {
            if (!Request.Form.Files.Any())
                return Ok();
            // there may appear exception on every step: File IO error, DB error. I better wrap everything
            try
            {
                // directory where will uploaded file go
                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);
                IFormFile file = Request.Form.Files[0];
                string fullPath = Path.Combine(pathToSave, file.FileName);
                using (FileStream stream = new(fullPath, FileMode.Create))  {
                    file.CopyTo(stream);
                }
                var configCSV = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                };
                List<Book> records = new List<Book> ();
                using (StreamReader readStream = new StreamReader(fullPath))  {
                    using (var csv = new CsvReader(readStream, configCSV))  {
                        records = csv.GetRecords<Book>().ToList();
                    }
                    foreach (Book bookFromFile in records)  {
                        // book without category - skipping. It may cause problems
                        if (String.IsNullOrEmpty(bookFromFile.CategoryName)) continue;
                        int? parentCategoryOfBook = null;
                        if (String.IsNullOrEmpty(bookFromFile.ParentCategoryName) == false)  {
                            Category foundParentCategory = _categoryRepository.Filter("CategoryName", bookFromFile.ParentCategoryName);
                            if (foundParentCategory == null)  {
                                //what could go wrong?
                                Category newParentCategory = await _categoryRepository.InsertAsync(new Category { CategoryName = bookFromFile.ParentCategoryName, ParentCategory = null }, false);
                                parentCategoryOfBook = newParentCategory.Id;
                            }
                            else  {
                                parentCategoryOfBook = foundParentCategory?.ParentCategory;
                            }
                        }
                        Category foundActualCategory = await _categoryRepository.FilterByNameAndParentId(bookFromFile.CategoryName, parentCategoryOfBook);
                        if (foundActualCategory == null)
                        {
                            foundActualCategory = await _categoryRepository.InsertAsync(new Category { CategoryName = bookFromFile.CategoryName, ParentCategory = parentCategoryOfBook }, false);
                        }
                        // okay, we are done with category mentioned in the record.  now add book
                        /// TODO _bookRepository.InsertAsync(new Book { BookAuthor = bookFromFile.BookAuthor, BookTitle = bookFromFile.BookTitle, }); ;
                    }
                }

                System.IO.File.Delete(fullPath);
            } catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
            return Ok();
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
