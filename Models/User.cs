/* This code is made by Anneshu Nag, Student ID- 2210994760*/
namespace robot_controller_api.Models;

// UserModel class with properties of the map
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }
    public string? Description { get; set; }
    public string? Role { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    // User Constructor
    public User(int id, string email, string firstName, string lastName, string passwordHash, DateTime? createdDate, DateTime? modifiedDate, string? description = null, string? role = null)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
        Description = description;
        Role = role;
    }

    public User()
    {
        // Default constructor
    }
}