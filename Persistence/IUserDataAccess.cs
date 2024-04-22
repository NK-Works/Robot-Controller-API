using robot_controller_api.Models;
namespace robot_controller_api.Persistence;
public interface IUserDataAccess
{
    IEnumerable<User> GetUsers();
    Task<User> GetUserByEmailAsync(string email); 
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);
    void UpdateUserCredentials(int id, string email, string password);
}
