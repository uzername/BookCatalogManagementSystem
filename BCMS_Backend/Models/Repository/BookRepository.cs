using BCMS_Backend.Entities;
using BCMS_Backend.Helpers;
using Dapper;

namespace BCMS_Backend.Repository
{
    public class BookRepository:DapperBase<Book>
    {
        /// <summary>
        /// get all books but with category name and parent category name
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Book>> GetAllBooksExtended()
        {
            var connection = DatabaseHelper.GetInMemoryDbConnection();
            return connection.QueryAsync<Book>("SELECT  b.BookTitle, b.BookAuthor, b.IdCategory, c.CategoryName, c.ParentCategoryName FROM Book b LEFT JOIN category c on b.IdCategory=c.Id ");
        }
    }
}
