/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using robot_controller_api.Models;
using robot_controller_api.Persistence;
using Microsoft.AspNetCore.Authorization;
using Isopoh.Cryptography.Argon2;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserDataAccess _userServiceRepo;
    private readonly IPasswordHasher<User> _passwordHasher;

    // Uncomment this constructor when using Microsoft's default Password Hasher and not BCrypt
    // public UsersController(IUserDataAccess userServiceRepo, IPasswordHasher<User> passwordHasher)
    // {
    //     _userServiceRepo = userServiceRepo;
    //     _passwordHasher = passwordHasher;
    // }

    public UsersController(IUserDataAccess userServiceRepo)
    {
        _userServiceRepo = userServiceRepo;
    }

    // Get all users
    /// <summary>
    /// Retrieves all existing users.
    /// </summary>
    /// <returns>All users.</returns>
    /// <remarks>
    /// Sample request: 
    /// 
    /// GET /api/users
    /// 
    /// </remarks>
    /// <response code="200">Returns all the users.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetUsers()
    {
        var users = _userServiceRepo.GetUsers();
        return Ok(users);
    }

    // Get "Admin" users
    /// <summary>
    /// Retrieves all the users with "Admin" privilege.
    /// </summary>
    /// <returns>Users who are "Admin".</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// GET /api/users/admin
    /// 
    /// </remarks>
    /// <response code="200">Returns the users with "Admin" status..</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("admin")]
    [Authorize(Policy = "UserOnly")]
    public IActionResult GetAdminUsers()
    {
        var adminUsers = _userServiceRepo.GetUsers().Where(field => field.Role == "Admin");
        return Ok(adminUsers);
    }

    // Get user by ID
    /// <summary>
    /// Retrieves a user by his/her ID.
    /// </summary>
    /// <param name="id">The ID of the user to be fetched.</param>
    /// <returns>The user with the specified ID.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// GET /api/users/{id}
    /// 
    /// </remarks>
    /// <response code="200">Returns the user with the specified ID.</response>
    /// <response code="404">If the user with the specified ID is not found.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}", Name = "GetUser")]
    [Authorize(Policy = "UserOnly")]
    public IActionResult GetUserById(int id)
    {
        var user = _userServiceRepo.GetUsers().FirstOrDefault(field => field.Id == id);
        if (user == null)
            return NotFound($"User with id {id} not found.");
        
        return Ok(user);
    }

    // Add a new user
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="newUser">A new user from the HTTP request.</param>
    /// <returns>A newly created user.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// POST /api/users
    /// {
    ///     "Email": "anneshu123@gmail.com",
    ///     "FirstName": "Never",
    ///     "LastName": "Know",
    ///     "PasswordHash": "password",
    ///     "Role": "Admin"
    /// }
    /// 
    /// </remarks> 
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">If the user is null</response> 
    /// <response code="409">If a user with the same name already exists.</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    [AllowAnonymous]
    public IActionResult AddUser([FromBody] User newUser)
    {
        if (newUser == null)
        {
            return BadRequest("Map data is required.");
        }
        if (_userServiceRepo.GetUsers().Any(field => field.FirstName == newUser.FirstName && field.LastName == newUser.LastName))
        {
            return Conflict();
        }
        // Hash the password before storing it in the database
        // newUser.PasswordHash = _passwordHasher.HashPassword(newUser, newUser.PasswordHash); // Microsoft Paswword Hash
        newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash); // Bcrypt Hash
        // newUser.PasswordHash = Argon2.Hash(newUser.PasswordHash); // Argon Hash
        
        // Set other properties like CreatedDate and ModifiedDate
        newUser.CreatedDate = DateTime.Now;
        newUser.ModifiedDate = DateTime.Now;
       _userServiceRepo.AddUser(newUser);
        return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
    }

    // Update an existing user
    /// <summary>
    /// Updates an already existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="updatedUser">The updated information for the user.</param>
    /// <returns>No content if the update is successful.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// PUT /api/users/{id}
    /// {
    ///     "Id": 2,
    ///     "FirstName": "Neverkkkk",
    ///     "LastName": "Know",
    ///     "Role": "User"
    /// }
    /// 
    /// Changing the Username i.e. Email and Password is not possible through this request.
    /// Changing those fields will not be allowed.
    /// 
    /// </remarks> 
    /// <response code="204">No content if the update is successful.</response>
    /// <response code="400">If the provided data is invalid.</response>
    /// <response code="404">If the user with the specified ID is not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
    {
        var existingUser = _userServiceRepo.GetUsers().FirstOrDefault(field => field.Id == id);
        if (existingUser == null)
        {
            return NotFound($"User with id {id} not found.");
        }
        if (id != updatedUser.Id)
        {
            return BadRequest("Invalid User data");
        }

        _userServiceRepo.UpdateUser(updatedUser);  // Update anything except email and password
        return NoContent();
    }

    // Delete a user by ID
    /// <summary>
    /// Deletes a user by its ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// DELETE /api/users/{id}
    /// 
    /// </remarks>
    /// <response code="204">No content if the deletion is successful.</response>
    /// <response code="404">If the user with the specified ID is not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult DeleteUser(int id)   
    {
        var userToRemove = _userServiceRepo.GetUsers().FirstOrDefault(field => field.Id == id);
        if (userToRemove == null)
        {
            return NotFound($"User with id {id} not found.");
        }
        _userServiceRepo.DeleteUser(id);
        return NoContent();
    }

    // Update the email and password of an existing user
    /// <summary>
    /// Updates the email and password of an already existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="loginModel">The updated email and password for the user.</param>
    /// <returns>No content if the update is successful.</returns>
    /// <remarks> 
    /// Sample request: 
    /// 
    /// PATCH /api/users/{id}
    /// {
    ///     "Id": 2,
    ///     "Email": "anneshu@gmail.com",
    ///     "Password": "password"
    /// }
    /// 
    /// </remarks> 
    /// <response code="204">No content if the update is successful.</response>
    /// <response code="404">If the user with the specified ID is not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    [HttpPatch("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult UpdateUserCredentials(int id, LoginModel loginModel)
    {
        var userToUpdate = _userServiceRepo.GetUsers().FirstOrDefault(field => field.Id == id);
        if (userToUpdate == null)
        {
            return NotFound($"User with id {id} not found.");
        }
        // Hash the new password before updating it in the database
        // userToUpdate.PasswordHash = _passwordHasher.HashPassword(userToUpdate, loginModel.Password);
        userToUpdate.PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginModel.Password); 
        // userToUpdate.PasswordHash = Argon2.Hash(loginModel.Password); 
        _userServiceRepo.UpdateUserCredentials(id, loginModel.Email, userToUpdate.PasswordHash);  // can specifically update the email and password 
        return NoContent();
    }
}