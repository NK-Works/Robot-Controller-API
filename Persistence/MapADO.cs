/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using Npgsql;
using robot_controller_api.Models;
namespace robot_controller_api.Persistence;

public class MapADO : IMapDataAccess
{
    // Connection string is usually set in a config file for the ease of change. 
    private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=mypassword;Database=SIT331";
    
    // Getting all the Maps (SELECT METHOD)
    public List<Map> GetAllMaps()
    {
        var maps = new List<Map>();
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM map", conn);

        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            // Read values off the data reader and create a new RobotCommand
            var map = new Map(
                id: dr.GetInt32(7),
                columns: dr.GetInt32(0),
                rows: dr.GetInt32(1),
                name: dr.GetString(2),
                createdDate: dr.GetDateTime(4),
                modifiedDate: dr.GetDateTime(5),
                description: dr.IsDBNull(3) ? null : dr.GetString(3),
                isSquare: dr.GetBoolean(6)  
            );
            maps.Add(map);
        }
        return maps;
    }
    
    // Updating the Maps (UPDATE METHOD)
    public void UpdateMap(Map updatedMap)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand(
            "UPDATE map SET columns = @columns, rows = @rows, name = @name, description = @description, modified_date = @modified_date WHERE id = @id", conn);

        // Add parameters to the command
        cmd.Parameters.AddWithValue("@id", updatedMap.Id);
        cmd.Parameters.AddWithValue("@columns", updatedMap.Columns);
        cmd.Parameters.AddWithValue("@rows", updatedMap.Rows);
        cmd.Parameters.AddWithValue("@name", updatedMap.Name);
        cmd.Parameters.AddWithValue("@description", updatedMap.Description ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@modified_date", updatedMap.ModifiedDate ?? DateTime.Now);

        cmd.ExecuteNonQuery();
    }
    
    // Insterting new Map (INSERT METHOD)
    public void InsertMap(Map newMap)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand(
            "INSERT INTO map (columns, rows, name, description, created_date, modified_date)" +
            "VALUES (@columns, @rows, @name, @description, @created_date, @modified_date) RETURNING id", conn);

        // Add parameters to the command
        cmd.Parameters.AddWithValue("@columns", newMap.Columns);
        cmd.Parameters.AddWithValue("@rows", newMap.Rows);
        cmd.Parameters.AddWithValue("@name", newMap.Name);
        cmd.Parameters.AddWithValue("@description", newMap.Description ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@created_date", newMap.CreatedDate ?? DateTime.Now);
        cmd.Parameters.AddWithValue("@modified_date", newMap.ModifiedDate ?? DateTime.Now);

        var newId = (int)cmd.ExecuteScalar();
        newMap.Id = newId;
    }
    
    // Deleting a Map (DELETE METHOD)
    public void DeleteMap(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand("DELETE FROM map WHERE id = @id", conn);

        // Add parameters to the command
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
