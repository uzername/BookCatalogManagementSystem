using BCMS_Backend.Entities;
using BCMS_Backend.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BCMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        readonly CategoryRepository _categoryRepository = new CategoryRepository();
        // GET all categories but also with name of parent category. That is handy
        [HttpGet]
        public Task<IEnumerable<Category>> Get()
        {
            return  _categoryRepository.GetAllCategoriesExtended();
        }

        /// <summary>
        /// GET category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>category object</returns>
        [HttpGet("{id}")]
        public Task<Category> Get(int id)
        {

            return _categoryRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Add new category and return what was added (why did I do this?)
        /// Also one may add parent category too
        /// </summary>
        /// <param name="inCategory">Category without known ID</param>
        /// <returns>Category with known ID</returns>
        [HttpPost]
        public Task<Category> Post([FromBody] Category inCategory)
        {
            // ignore Id because with default value of Id (zero) will drop exception
           return _categoryRepository.InsertAsync(inCategory, false);
        }

        /// <summary>
        /// edit category by id and set new value
        /// </summary>
        /// <param name="id">this one is ignored, set ID in value</param>
        /// <param name="value">value to be edited, by ID. Id cannot be changed by itself, it is used for lookup</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Category value)
        {
            try
            {
                await _categoryRepository.UpdateAsync(value);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// delete all categories. It may drop exception if some books have this category.
        /// </summary>
        /// <returns>Ok (code 200) normally. BadRequest if something goes wrong like foreign constraint problem</returns>
        [HttpDelete("delall")]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                await _categoryRepository.DeleteAllRecords();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///  DELETE category by ID
        /// </summary>
        /// <param name="id">int value of category</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryRepository.DeleteByIdAsync(id);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
