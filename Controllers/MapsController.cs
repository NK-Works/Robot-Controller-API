/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using Microsoft.AspNetCore.Authorization;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/maps")]
public class MapsController : ControllerBase
{   
    private readonly  IMapDataAccess _mapsRepo; 
    public MapsController(IMapDataAccess mapCommandsRepo)
    {
        _mapsRepo = mapCommandsRepo;
    }

    // Get all maps
    /// <summary>
    /// Retrieves all existing maps.
    /// </summary>
    /// <returns>All maps.</returns>
    /// <remarks>
    /// Sample request: 
    /// 
    /// GET /api/maps
    /// 
    /// </remarks>
    /// <response code="200">Returns all the maps.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    [AllowAnonymous]
    public IEnumerable<Map> GetAllMaps()    // Everyone
    {
        var maps = _mapsRepo.GetAllMaps(); 
        return maps;
    }

    // Get maps where columns equal rows
    /// <summary>
    /// Retrieves maps where columns equal rows.
    /// </summary>
    /// <returns>Maps where columns equal rows.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// GET /api/maps/square
    /// 
    /// </remarks>
    /// <response code="200">Returns maps that are square.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("square")]
    [Authorize(Policy = "UserOnly")]
    public IEnumerable<Map> GetSquareMaps()
    {
        var squareMaps = _mapsRepo.GetAllMaps().Where(field => field.IsSquare == true);
        return squareMaps;
    }

    // Get map by ID
    /// <summary>
    /// Retrieves a map by its ID.
    /// </summary>
    /// <param name="id">The ID of the map to be fetched.</param>
    /// <returns>The map with the specified ID.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// GET /api/maps/{id}
    /// 
    /// </remarks>
    /// <response code="200">Returns the map with the specified ID.</response>
    /// <response code="404">If the map with the specified ID is not found.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}", Name = "GetMap")]
    [Authorize(Policy = "UserOnly")]
    public IActionResult GetMapById(int id)
    {
        var map = _mapsRepo.GetAllMaps().FirstOrDefault(field => field.Id == id);
        if (map == null)
        {
            return NotFound($"Map with id {id} not found.");
        }
        return Ok(map);
    }

    // Add a new map
    /// <summary>
    /// Creates a new map.
    /// </summary>
    /// <param name="newMap">A new map from the HTTP request.</param>
    /// <returns>A newly created map.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// POST /api/maps
    /// {
    ///     "columns: 10,
    ///     "rows": 10,
    ///     "name": "10 Map", 
    ///     "description": "Basic Moon Map" 
    /// }  
    /// 
    /// </remarks> 
    /// <response code="201">Returns the newly created map</response>
    /// <response code="400">If the map is null</response> 
    /// <response code="409">If a map with the same name already exists.</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult AddMap([FromBody] Map newMap)
    {
        if (newMap  == null)
        {
            return BadRequest("Map data is required.");
        }
        if (_mapsRepo.GetAllMaps().Any(field => field.Name == newMap.Name))
        {
            return Conflict();
        }
        _mapsRepo.InsertMap(newMap);
        return CreatedAtRoute("GetMap", new { id = newMap.Id }, newMap); 
    }
    
    // Update an existing map
    /// <summary>
    /// Updates an already existing map.
    /// </summary>
    /// <param name="id">The ID of the map to update.</param>
    /// <param name="updatedMap">The updated information for the map.</param>
    /// <returns>No content if the update is successful.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// PUT /api/maps/{id}
    /// {
    ///     "id": {id},
    ///     "columns: 100,
    ///     "rows": 100,
    ///     "name": "100 Map", 
    ///     "description": "Updated Moon Map" 
    /// }  
    /// 
    /// </remarks> 
    /// <response code="204">No content if the update is successful.</response>
    /// <response code="400">If the provided data is invalid.</response>
    /// <response code="404">If the map with the specified ID is not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    [Authorize(Policy = "MemberOnly")]
    public IActionResult UpdateMap(int id, [FromBody] Map updatedMap)
    {
        var existingMap = _mapsRepo.GetAllMaps().FirstOrDefault(field => field.Id == id);
        if (existingMap == null)
            return NotFound($"Map with id {id} not found.");

        if (updatedMap == null || updatedMap.Id != id)
            return BadRequest("Invalid map data.");

        _mapsRepo.UpdateMap(updatedMap);
        return NoContent();
    }

    // Delete a map by ID
    /// <summary>
    /// Deletes a map by its ID.
    /// </summary>
    /// <param name="id">The ID of the map to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// DELETE /api/maps/{id}
    /// 
    /// </remarks>
    /// <response code="204">No content if the deletion is successful.</response>
    /// <response code="404">If the map with the specified ID is not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult DeleteMap(int id)
    {
        var mapToRemove = _mapsRepo.GetAllMaps().FirstOrDefault(field => field.Id == id);
        if (mapToRemove == null)
            return NotFound($"Map with id {id} not found.");

        _mapsRepo.DeleteMap(id);
        return NoContent(); 
    }
    
    // Check if given coordinate (x, y) is on the map with specified ID
    /// <summary>
    /// Checks if the given coordinate (x, y) is on the map with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the map to check.</param>
    /// <param name="x">The x-coordinate to check.</param>
    /// <param name="y">The y-coordinate to check.</param>
    /// <returns>True if the coordinate is on the map; otherwise, false.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// GET /api/maps/{id}/{x}-{y}
    /// 
    /// </remarks>
    /// <response code="200">Returns true if the coordinate is present in the map with the specified else false.</response>
    /// <response code="400">If the provided map coordinated is invalid.</response>
    /// <response code="404">If the specified map is not found.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}/{x}-{y}")] // From )-0 to (n-1)-(n-1)
    [Authorize(Policy = "DevsOnly")]
    public IActionResult CheckCoordinate(int id, int x, int y) 
    { 
        if (x < 0 || y < 0)
        {
            return BadRequest();
        }
        var map = _mapsRepo.GetAllMaps().FirstOrDefault(field => field.Id == id);

        if (map == null)
        {
            return NotFound();
        }
        bool isOnMap = x < map.Columns && y < map.Rows;
        return Ok(isOnMap); 
    } 
}
