/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using Npgsql;
using robot_controller_api.Models;
namespace robot_controller_api.Persistence
{
    public class RobotCommandRepository : IRobotCommandDataAccess, IRepository
    {
        private IRepository _repo => this;

        // Getting all the Robot Commands (SELECT METHOD)
        public List<RobotCommand> GetRobotCommands()
        {
            var sqlCommand = "SELECT id, name AS name, description AS description, is_move_command AS isMoveCommand, created_date AS createdDate, modified_date AS modifiedDate FROM public.robot_command";
            var commands = _repo.ExecuteReader<RobotCommand>(sqlCommand);
            return commands;
        }  

        // Updating the Robot Commands (UPDATE METHOD)
        public void UpdateRobotCommand(RobotCommand updatedCommand)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", updatedCommand.Id),
                new NpgsqlParameter("@name", updatedCommand.Name),
                new NpgsqlParameter("@description", updatedCommand.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("@is_move_command", updatedCommand.IsMoveCommand),
                new NpgsqlParameter("@modified_date", updatedCommand.ModifiedDate ?? DateTime.Now)
            };

            var sqlCommand = "UPDATE robot_command SET name = @name, description = @description, " +
                             "is_move_command = @is_move_command, modified_date = @modified_date " +
                             "WHERE id = @id RETURNING *";

            _repo.ExecuteReader<RobotCommand>(sqlCommand, sqlParams).Single();
        }

        // Insterting new Robot Command (INSERT METHOD)
        public void InsertRobotCommand(RobotCommand newCommand)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@name", newCommand.Name),
                new NpgsqlParameter("@description", newCommand.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("@is_move_command", newCommand.IsMoveCommand),
                new NpgsqlParameter("@created_date", newCommand.CreatedDate ?? DateTime.Now),
                new NpgsqlParameter("@modified_date", newCommand.ModifiedDate ?? DateTime.Now)
            };

            var sqlCommand = "INSERT INTO robot_command (name, description, is_move_command, created_date, modified_date) " +
                             "VALUES (@name, @description, @is_move_command, @created_date, @modified_date) RETURNING id";

            var newId = _repo.ExecuteScalar<int>(sqlCommand, sqlParams);

            newCommand.Id = newId;
        }

        // Deleting a Robot Command (DELETE METHOD)
        public void DeleteRobotCommand(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", id)
            };

            var sqlCommand = "DELETE FROM robot_command WHERE id = @id";
            _repo.ExecuteReader<object>(sqlCommand, sqlParams);
        }
    }
}
