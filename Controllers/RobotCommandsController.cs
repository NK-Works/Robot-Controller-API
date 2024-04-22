/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/robot-commands")]
public class RobotCommandsController : ControllerBase
{
    private readonly IRobotCommandDataAccess _robotCommandsRepo;
    public RobotCommandsController(IRobotCommandDataAccess robotCommandsRepo)
    {
        _robotCommandsRepo = robotCommandsRepo;
    }
    // Get all robot commands
    /// <summary>
    /// Retrieves all existing robot commands.
    /// </summary>
    /// <returns>All robot commands.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// GET /api/robot-commands
    /// 
    /// </remarks>
    /// <response code="200">Returns all the robot commands.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    [AllowAnonymous]
    public IEnumerable<RobotCommand> GetAllRobotCommands()
    {
        var robot_commands = _robotCommandsRepo.GetRobotCommands();
        return robot_commands;
    }

    // Get only move commands
    /// <summary>
    /// Retrieves only move commands among all robot commands.
    /// </summary>
    /// <returns>Move robot commands.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// GET /api/robot-commands/move
    /// 
    /// </remarks>
    /// <response code="200">Returns all the robot move commands.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("move")]   
    [Authorize(Policy = "UserOnly")]
    public IEnumerable<RobotCommand> GetMoveCommandsOnly()
    {
        var move_commands = _robotCommandsRepo.GetRobotCommands().Where(field => field.IsMoveCommand == true);
        return move_commands;
    }
    
    // Get robot command by ID
    /// <summary>
    /// Retreives a robot command by its ID.
    /// </summary>
    /// <param name="id">The ID of the robot command to be fetched.</param>
    /// <returns>The robot command with the specified ID.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// GET /api/robot-commands/{id}
    /// 
    /// </remarks>
    /// <response code="200">Returns the robot command.</response>
    /// <response code="404">If the robot command with the specified ID is not found.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}", Name = "GetRobotCommand")]
    [Authorize(Policy = "UserOnly")]
    public IActionResult GetRobotCommandById(int id)
    {
        var command = _robotCommandsRepo.GetRobotCommands().FirstOrDefault(field => field.Id == id);
        if (command == null)
        {
            return NotFound($"Command with id {id} not found.");
        }
        return Ok(command);
    }

    // Add a new robot command
    /// <summary> 
    /// Creates a robot command. 
    /// </summary> 
    /// <param name="newCommand">A new robot command from the HTTP request.</param> 
    /// <returns>A newly created robot command</returns> 
    /// <remarks> 
    /// Sample request: 
    /// 
    /// POST /api/robot-commands
    /// {
    ///     "name": "JUMP", 
    ///     "isMoveCommand": true, 
    ///     "description": "Jump on the Moon" 
    /// }  
    /// 
    /// </remarks> 
    /// <response code="201">Returns the newly created robot command</response>
    /// <response code="400">If the robot command is null</response> 
    /// <response code="409">If a robot command with the same name already exists.</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost()]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult AddRobotCommand([FromBody] RobotCommand newCommand)
    {
        var present_command = _robotCommandsRepo.GetRobotCommands();
        if (newCommand == null)
        {
            return BadRequest("Command data is required.");
        }
        if (present_command.Any(field => field.Name == newCommand.Name))
        {
            return Conflict();
        }
        _robotCommandsRepo.InsertRobotCommand(newCommand);
        return CreatedAtRoute("GetRobotCommand", new { id = newCommand.Id }, newCommand);
    }

    // Update an existing robot command
    /// <summary>
    /// Updates an already existing robot command.
    /// </summary>
    /// <param name="id">The ID of the robot command to update.</param>
    /// <param name="updatedCommand">The updated information for the robot command.</param>
    /// <returns>No content if the update is successful.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// PUT /api/robot-commands/{id}
    /// {
    ///     "id": {id},
    ///     "name": "BLINK", 
    ///     "isMoveCommand": false, 
    ///     "description": "Lights on the moon" 
    /// }  
    /// 
    /// </remarks> 
    /// <response code="204">No content if the update is successful.</response>
    /// <response code="400">If the provided data is invalid.</response>
    /// <response code="404">If the robot command with the specified ID is not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")] 
    [Authorize(Policy = "MemberOnly")] 
    public IActionResult UpdateRobotCommand(int id, RobotCommand updatedCommand)
    {
        var existingCommand = _robotCommandsRepo.GetRobotCommands().FirstOrDefault(field => field.Id == id);
        if (existingCommand == null)
            return NotFound($"Command with id {id} not found.");

        if (updatedCommand == null || updatedCommand.Id != id)
            return BadRequest("Invalid map data.");

        _robotCommandsRepo.UpdateRobotCommand(updatedCommand);
        return NoContent();
    }
    // Delete a robot command by ID
    /// <summary>
    /// Deletes a robot command by its ID.
    /// </summary>
    /// <param name="id">The ID of the robot command to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// DELETE /api/robot-commands/{id}
    /// 
    /// </remarks>
    /// <response code="204">No content if the deletion is successful.</response>
    /// <response code="404">If the robot command with the specified ID is not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")] 
    public IActionResult DeleteRobotCommand(int id)
    {
        var commandToRemove = _robotCommandsRepo.GetRobotCommands().FirstOrDefault(field => field.Id == id);
        if (commandToRemove == null)
            return NotFound($"Command with id {id} not found.");

        _robotCommandsRepo.DeleteRobotCommand(id);
        return NoContent();
    }
}
