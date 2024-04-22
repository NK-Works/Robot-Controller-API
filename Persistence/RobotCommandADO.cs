/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using Npgsql;
using robot_controller_api.Models;
namespace robot_controller_api.Persistence;

public class RobotCommandADO : IRobotCommandDataAccess
{
    // Connection string is usually set in a config file for the ease of change. 
    private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=mypassword;Database=SIT331";

    // Getting all the Robot Commands (SELECT METHOD)
    public List<RobotCommand> GetRobotCommands()
    {
        var robotCommands = new List<RobotCommand>();
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM robot_command", conn);

        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            // Read values off the data reader and create a new RobotCommand
            var robotCommand = new RobotCommand(
                id: dr.GetInt32(5),
                name: dr.GetString(0),
                isMoveCommand: dr.GetBoolean(2),
                createdDate: dr.GetDateTime(3),
                modifiedDate: dr.GetDateTime(4),
                description: dr.IsDBNull(1) ? null : dr.GetString(1)
            );

            robotCommands.Add(robotCommand);
        }
        return robotCommands;
    }

    // Updating the Robot Commands (UPDATE METHOD)
    public void UpdateRobotCommand(RobotCommand updatedCommand)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand(
            "UPDATE robot_command SET name = @name, description = @description, is_move_command = @is_move_command modified_date = @modified_date WHERE id = @id", conn);

        // Add parameters to the command
        cmd.Parameters.AddWithValue("@id", updatedCommand.Id);
        cmd.Parameters.AddWithValue("@name", updatedCommand.Name);
        cmd.Parameters.AddWithValue("@description", updatedCommand.Description ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@is_move_command", updatedCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("@modified_date", updatedCommand.ModifiedDate ?? DateTime.Now);

        cmd.ExecuteNonQuery();
    }

    // Insterting new Robot Command (INSERT METHOD)
    public void InsertRobotCommand(RobotCommand newCommand)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand(
            "INSERT INTO robot_command (name, description, is_move_command, created_date, modified_date)" +
            "VALUES (@name, @description, @is_move_command, @created_date, @modified_date) RETURNING id", conn);

        // Add parameters to the command
        cmd.Parameters.AddWithValue("@name", newCommand.Name);
        cmd.Parameters.AddWithValue("@description", newCommand.Description ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@is_move_command", newCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("@created_date", newCommand.CreatedDate ?? DateTime.Now);
        cmd.Parameters.AddWithValue("@modified_date", newCommand.ModifiedDate ?? DateTime.Now);

        // ExecuteScalar is used here because we are returning the newly generated id
        var newId = (int)cmd.ExecuteScalar();
        newCommand.Id = newId;
    }

    // Deleting a Robot Command (DELETE METHOD)
    public void DeleteRobotCommand(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand("DELETE FROM robot_command WHERE id = @id", conn);

        // Add parameters to the command
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}