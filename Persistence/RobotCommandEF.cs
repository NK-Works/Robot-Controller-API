/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using Microsoft.EntityFrameworkCore;
using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public class RobotCommandEF : IRobotCommandDataAccess
{
    private readonly RobotContext _context;

    public RobotCommandEF(RobotContext context)
    {
        _context = context;
    }

    public List<RobotCommand> GetRobotCommands()
    {
        return _context.RobotCommands.ToList();
    }

    public void UpdateRobotCommand(RobotCommand updatedCommand)
    {
        var existingCommand = _context.RobotCommands.Find(updatedCommand.Id);
        if (existingCommand != null)
        {
            existingCommand.Name = updatedCommand.Name;
            existingCommand.Description = updatedCommand.Description;
            existingCommand.IsMoveCommand = updatedCommand.IsMoveCommand;
            existingCommand.ModifiedDate = updatedCommand.ModifiedDate ?? DateTime.Now;

            _context.SaveChanges();
        }
    }

    public void InsertRobotCommand(RobotCommand newCommand)
    {
        newCommand.CreatedDate ??= DateTime.Now;
        newCommand.ModifiedDate ??= DateTime.Now;

        _context.RobotCommands.Add(newCommand);
        _context.SaveChanges();
    }

    public void DeleteRobotCommand(int id)
    {
        var commandToDelete = _context.RobotCommands.Find(id);
        if (commandToDelete != null)
        {
            _context.RobotCommands.Remove(commandToDelete);
            _context.SaveChanges();
        }
    }
}