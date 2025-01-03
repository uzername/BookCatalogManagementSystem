using Dapper;
using BCMS_Backend.Entities;
using BCMS_Backend.Helpers;
using BCMS_Backend.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace BCMS_Backend.Repository
{
    /// <summary>
    /// Dapper Base class adapted to work with In-Memory Database SQLITE. References:
    /// <see cref="https://github.com/dotnet/samples/blob/main/standard/data/sqlite/InMemorySample/Program.cs"/> ;
    /// <see cref="https://metanit.com/sharp/adonetcore/4.1.php"/> ;
    /// <see cref="https://github.com/Unbalanced-Tree/CSharp/blob/main/InMemoryDb/InMemoryDb/"/>
    /// After all the connections are closed, the database is deleted. At least one connection should always be open. 
    /// In-memory storage feels extremely wrong and it should not be practiced!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DapperBase<T> where T : BaseEntity
    {
        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();
        /// <summary>
        /// connection string. It should be read during creation
        /// </summary>
        protected readonly string _connection;

        public DapperBase()
        {
            _connection = GlobalSettings.GetConnectionConnectionString();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            //using (var connection = new SqliteConnection(_connection))
            var connection = DatabaseHelper.GetInMemoryDbConnection();
                return await connection.QueryAsync<T>($"SELECT * FROM {typeof(T).Name.ToLower()}").ConfigureAwait(false);
            
        }
        public async Task<T> GetByIdAsync(int id)
        {
            //using (var connection = new SqliteConnection(_connection))
            var connection = DatabaseHelper.GetInMemoryDbConnection();
            var result = await connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {typeof(T).Name.ToLower()} WHERE Id=@Id", new { Id = id }).ConfigureAwait(false);
                return result;
            
        }
        public T Filter(string column, object value)
        {
            //using (var connection = new SqliteConnection(_connection))
            var connection = DatabaseHelper.GetInMemoryDbConnection();
            
                var result = connection.QuerySingleOrDefault<T>($"SELECT * FROM {typeof(T).Name.ToLower()} WHERE {column}=@arg", new { arg = value });
                return result;
            
        }
        public IEnumerable<T> FilterAll(string column, object value)
        {
            //using (var connection = new SqliteConnection(_connection))
            var connection = DatabaseHelper.GetInMemoryDbConnection();
            var result = connection.Query<T>($"SELECT * FROM {typeof(T).Name.ToLower()} WHERE {column}=@arg", new { arg = value });
                return result;
            
        }

        public async Task DeleteByIdAsync(int id)
        {
            //using (var connection = new SqliteConnection(_connection))
            var connection = DatabaseHelper.GetInMemoryDbConnection();
            await connection.ExecuteAsync($"DELETE FROM {typeof(T).Name.ToLower()} WHERE Id=@Id", new { Id = id }).ConfigureAwait(false);
            
        }
        public async Task<T> InsertAsync(T t, bool includeID = true)
        {
            var insertQuery = GenerateInsertQuery(typeof(T), includeID);

            //using (var connection = new SqliteConnection(_connection))
            var connection = DatabaseHelper.GetInMemoryDbConnection();
            t.Id = await connection.ExecuteScalarAsync<int>(insertQuery+ " RETURNING Id", t);
                return t;
            
        }

        /// <summary>
        /// insert to database but No async. Probably bad for web project, but I need to think on it. It is really simple to use
        /// </summary>
        /// <param name="t"></param>
        public T InsertSync(T t)
        {
            var insertQuery = GenerateInsertQuery(typeof(T));
            //using (var connection = new SqliteConnection(_connection))
            var connection = DatabaseHelper.GetInMemoryDbConnection();
            t.Id = connection.ExecuteScalar<int>(insertQuery + " RETURNING Id", t);
                return t;
            
        }

        public async Task UpdateAsync(T t)
        {
            var updateQuery = GenerateUpdateQuery(typeof(T));

            //using (var connection = new SqliteConnection(_connection))
            var connection = DatabaseHelper.GetInMemoryDbConnection();
            await connection.ExecuteAsync(updateQuery, t).ConfigureAwait(false);
            
        }
        /// <summary>
        /// generate string list of class properties except of those who have [Description("ignore")]. Useful for making INSERT statement
        /// </summary>
        /// <param name="listOfProperties"></param>
        /// <returns> string list of property names</returns>
        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }
        protected string GenerateInsertQuery(Type t, bool includeID = true)
        {
            var insertQuery = new StringBuilder($"INSERT INTO {t.Name.ToLower()} ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(prop => { if ((prop != "Id") || includeID) insertQuery.Append($"{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { if ((prop != "Id") || includeID) insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }
        protected string GenerateUpdateQuery(Type t)
        {
            var updateQuery = new StringBuilder($"UPDATE {t.Name.ToLower()} SET ");
            var properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });
            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append(" WHERE Id=@Id");
            return updateQuery.ToString();
        }
    }
}
