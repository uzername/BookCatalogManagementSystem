using Dapper;
using BCMS_Backend.Entities;
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
    /// Someone should carry a torch of SQLITE CONNECTION. Once it fades, in-memory storage is gone. At least on connection should always be open. 
    /// After all the connections are closed, the database is deleted.
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
            using (var connection = new SqliteConnection(_connection))
            {
                return await connection.QueryAsync<T>($"SELECT * FROM {typeof(T).Name.ToLower()}").ConfigureAwait(false);
            }
        }
        public async Task<T> GetByIdAsync(int id)
        {
            using (var connection = new SqliteConnection(_connection))
            {
                var result = await connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {typeof(T).Name.ToLower()} WHERE Id=@Id", new { Id = id }).ConfigureAwait(false);
                return result;
            }
        }
        public T Filter(string column, object value)
        {
            using (var connection = new SqliteConnection(_connection))
            {
                var result = connection.QuerySingleOrDefault<T>($"SELECT * FROM {typeof(T).Name.ToLower()} WHERE {column}=@arg", new { arg = value });
                return result;
            }
        }
        public IEnumerable<T> FilterAll(string column, object value)
        {
            using (var connection = new SqliteConnection(_connection))
            {
                var result = connection.Query<T>($"SELECT * FROM {typeof(T).Name.ToLower()} WHERE {column}=@arg", new { arg = value });
                return result;
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            using (var connection = new SqliteConnection(_connection))
            {
                await connection.ExecuteAsync($"DELETE FROM {typeof(T).Name.ToLower()} WHERE Id=@Id", new { Id = id }).ConfigureAwait(false);
            }
        }
        public async Task<T> InsertAsync(T t)
        {
            var insertQuery = GenerateInsertQuery(typeof(T));

            using (var connection = new SqliteConnection(_connection))
            {
                t.Id = await connection.ExecuteScalarAsync<int>(insertQuery+ ";SELECT LAST_INSERT_ID();", t);
                return t;
            }
        }

        /// <summary>
        /// insert to database but No async. Probably bad for web project, but I need to think on it. 
        /// At least Anatoliy tried to explain that it may be dangerous, though I am not sure what could possibly go wrong
        /// </summary>
        /// <param name="t"></param>
        public T InsertSync(T t)
        {
            var insertQuery = GenerateInsertQuery(typeof(T));

            using (var connection = new SqliteConnection(_connection))
            {
                t.Id = connection.ExecuteScalar<int>(insertQuery + ";SELECT LAST_INSERT_ID();", t);
                return t;
            }
        }

        public async Task UpdateAsync(T t)
        {
            var updateQuery = GenerateUpdateQuery(typeof(T));

            using (var connection = new SqliteConnection(_connection))
            {
                await connection.ExecuteAsync(updateQuery, t).ConfigureAwait(false);
            }
        }
        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }
        protected string GenerateInsertQuery(Type t)
        {
            var insertQuery = new StringBuilder($"INSERT INTO {t.Name.ToLower()} ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(prop => { insertQuery.Append($"{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

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
