using robot_controller_api.Models;
namespace robot_controller_api.Persistence;

public interface IRobotCommandDataAccess
{
    List<RobotCommand> GetRobotCommands();
    void UpdateRobotCommand(RobotCommand updatedCommand);
    void InsertRobotCommand(RobotCommand newCommand);
    void DeleteRobotCommand(int id);
}