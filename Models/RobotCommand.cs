/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using System;
using System.Collections.Generic;
namespace robot_controller_api.Models;

// RobotCommand class with properties of the map
public class RobotCommand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsMoveCommand { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? Description { get; set; }

    // RobotCommand constructor
    public RobotCommand(int id, string name, bool isMoveCommand, DateTime? createdDate, DateTime? modifiedDate, string? description = null)
    {
        Id = id;
        Name = name;
        IsMoveCommand = isMoveCommand;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
        Description = description; 
    }

    public RobotCommand()
    {
        // Default constructor
    }
}
