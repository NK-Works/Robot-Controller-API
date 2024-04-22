/* This code is made by Anneshu Nag, Student ID: 2210994760 */
using robot_controller_api.Models;
using Microsoft.EntityFrameworkCore;

namespace robot_controller_api.Persistence;
public class UserServiceEF : IUserDataAccess
{
    private readonly RobotContext _context;

    public UserServiceEF(RobotContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetUsers()
    {
        return _context.Users.ToList();
    }
    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    public void AddUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void UpdateUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var existingUser = _context.Users.Find(user.Id);
        if (existingUser == null)
        {
            throw new ArgumentException($"User with ID {user.Id} not found.", nameof(user.Id));
        }

        // Update allowed fields
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Role = user.Role;

        _context.SaveChanges();
    }

    public void DeleteUser(int id)
    {
        var userToDelete = _context.Users.Find(id);
        if (userToDelete == null)
        {
            throw new ArgumentException($"User with ID {id} not found.", nameof(id));
        }

        _context.Users.Remove(userToDelete);
        _context.SaveChanges();
    }

    public void UpdateUserCredentials(int id, string email, string password)
    {
        var userToUpdate = _context.Users.Find(id);
        if (userToUpdate == null)
        {
            throw new ArgumentException($"User with ID {id} not found.", nameof(id));
        }

        // Update email and password
        userToUpdate.Email = email;
        userToUpdate.PasswordHash = password;

        _context.SaveChanges();
    }
}
