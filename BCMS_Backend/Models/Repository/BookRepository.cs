using BCMS_Backend.Entities;
using BCMS_Backend.Helpers;
using Dapper;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

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
            return connection.QueryAsync<Book>("SELECT  b.BookTitle, b.BookAuthor, b.IdCategory, c.CategoryName, c2.CategoryName as ParentCategoryName FROM Book b LEFT JOIN category c on b.IdCategory=c.Id LEFT JOIN category c2 on c.ParentCategory=c2.Id ");
        }
        /// <summary>
        /// extremely complicated method to get list of items paginated and filtered. It probably requires debugging.
        /// https://dev.to/drsimplegraffiti/pagination-in-net-api-4opp
        /// </summary>
        /// <param name="inParameters"></param>
        /// <returns></returns>
        public async Task<PaginatedList<Book>> GetAllBooksExtendedFilteredPagination(QueryParameters inParameters)
        {
            bool authorFilterActive = String.IsNullOrEmpty(inParameters.BookAuthorFilter);
            bool titleFilterActive = String.IsNullOrEmpty(inParameters.BookTitleFilter);
            bool categoryFilterActive = String.IsNullOrEmpty(inParameters.CategoryFilter);
            bool onlyOneFilterActive = (authorFilterActive && !titleFilterActive && !categoryFilterActive) || (!authorFilterActive && titleFilterActive && !categoryFilterActive) || (!authorFilterActive && !titleFilterActive && categoryFilterActive);
            StringBuilder completeQuery = new StringBuilder("SELECT  b.BookTitle, b.BookAuthor, b.IdCategory, c.CategoryName, c.ParentCategoryName FROM Book b LEFT JOIN category c on b.IdCategory=c.Id ");
            var dictionaryParameters = new Dictionary<string, object>();
            if (authorFilterActive || titleFilterActive || categoryFilterActive)
            {
                bool pleaseAddAND = false;
                completeQuery.Append(" WHERE ");
                
                    if (authorFilterActive)  {
                        if (onlyOneFilterActive == false)
                            completeQuery.Append(" ( ");
                        completeQuery.Append("b.BookAuthor = @AUTHOR");
                        if (onlyOneFilterActive == false)
                            completeQuery.Append(" ) ");
                        pleaseAddAND = true;
                        dictionaryParameters.Add("@AUTHOR", inParameters.BookAuthorFilter);
                    }
                    if (titleFilterActive)  {
                        if (pleaseAddAND)  {
                            completeQuery.Append(" AND ");
                        }
                        if (titleFilterActive)  {
                            if (onlyOneFilterActive == false)
                                completeQuery.Append(" ( ");
                            completeQuery.Append("b.BookTitle = @TITLE");
                            if (onlyOneFilterActive == false)
                                completeQuery.Append(" ) ");
                        }
                        pleaseAddAND = true;
                        dictionaryParameters.Add("@TITLE", inParameters.BookTitleFilter);
                    }
                    if (categoryFilterActive)
                    {
                        if (pleaseAddAND)  {
                            completeQuery.Append(" AND ");
                        }
                        if (onlyOneFilterActive == false)
                            completeQuery.Append(" ( ");
                        completeQuery.Append("c.CategoryName = @CATEGORY OR c.ParentCategoryName = @CATEGORY");
                        if (onlyOneFilterActive == false)
                            completeQuery.Append(" ) ");
                        dictionaryParameters.Add("@CATEGORY", inParameters.CategoryFilter);
                    }
                
            }
            var connection = DatabaseHelper.GetInMemoryDbConnection();
            IEnumerable<Book> playas;
            if (authorFilterActive || titleFilterActive || categoryFilterActive)
            {
                var currentQueryParameters = new DynamicParameters(dictionaryParameters);
                playas = await connection.QueryAsync<Book>(completeQuery.ToString(), currentQueryParameters);
            }
            else
                playas = await connection.QueryAsync<Book>(completeQuery.ToString());
            var count = playas.Count();
            var totalPages = (int)Math.Ceiling(count / (double)inParameters.pageSize);
            var almostDone = playas.Skip((inParameters.pageIndex - 1) * inParameters.pageSize).Take(inParameters.pageSize);
            return new PaginatedList<Book>(almostDone.AsList(), inParameters.pageIndex, totalPages);
        }

    }
}
