using BCMS_Backend.Entities;
using BCMS_Backend.Helpers;
using Dapper;

namespace BCMS_Backend.Repository
{
    public class CategoryRepository:DapperBase<Category>
    {
        /// <summary>
        /// get all categories but also with name of parent category
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Category>> GetAllCategoriesExtended()
        {
            var connection = DatabaseHelper.GetInMemoryDbConnection();
            // this query is funny
            return connection.QueryAsync<Category>("SELECT c.CategoryName as 'ParentCategoryName',p.Id as 'Id',c.Id as 'ParentCategory',p.CategoryName as 'CategoryName' FROM category p LEFT JOIN category c on c.Id=p.ParentCategory ");
        }
    }
}
