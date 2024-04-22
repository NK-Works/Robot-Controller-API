/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using Npgsql;
using robot_controller_api.Models;
namespace robot_controller_api.Persistence
{
    public class MapRepository : IMapDataAccess, IRepository
    {
        private IRepository _repo => this;

        // Getting all the Maps (SELECT METHOD)
        public List<Map> GetAllMaps()
        {
            var sqlCommand = "SELECT id, columns AS columns, rows AS rows, name AS name, description AS description, created_date AS createdDate, modified_date AS modifiedDate, is_square AS isSquare FROM public.map";
            var commands = _repo.ExecuteReader<Map>(sqlCommand);
            return commands;
        }

        // Updating the Maps (UPDATE METHOD)
        public void UpdateMap(Map updatedMap)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", updatedMap.Id),
                new NpgsqlParameter("@columns", updatedMap.Columns),
                new NpgsqlParameter("@rows", updatedMap.Rows),
                new NpgsqlParameter("@name", updatedMap.Name),
                new NpgsqlParameter("@description", updatedMap.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("@modified_date", updatedMap.ModifiedDate ?? DateTime.Now)
            };

            var sqlCommand = "UPDATE map SET columns = @columns, rows = @rows, name = @name, description = @description, " +
                             "modified_date = @modified_date " + 
                             "WHERE id = @id RETURNING *";

            _repo.ExecuteReader<Map>(sqlCommand, sqlParams).Single();
        }
        
        // Insterting new Map (INSERT METHOD)
        public void InsertMap(Map newMap)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@columns", newMap.Columns),
                new NpgsqlParameter("@rows", newMap.Rows),
                new NpgsqlParameter("@name", newMap.Name),
                new NpgsqlParameter("@description", newMap.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("@created_date", newMap.CreatedDate ?? DateTime.Now),
                new NpgsqlParameter("@modified_date", newMap.ModifiedDate ?? DateTime.Now)
            };

            var sqlCommand = "INSERT INTO map (columns, rows, name, description, created_date, modified_date) " +
                             "VALUES (@columns, @rows, @name, @description, @created_date, @modified_date) RETURNING id";

            var newId = _repo.ExecuteScalar<int>(sqlCommand, sqlParams);

            newMap.Id = newId;
        }
        
        // Deleting a Map (DELETE METHOD)
        public void DeleteMap(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", id)
            };

            var sqlCommand = "DELETE FROM map WHERE id = @id";
            _repo.ExecuteReader<object>(sqlCommand, sqlParams);
        }
    }
}
