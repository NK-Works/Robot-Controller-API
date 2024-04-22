using robot_controller_api.Models;
namespace robot_controller_api.Persistence;

public interface IMapDataAccess
{
    List<Map> GetAllMaps();
    void UpdateMap(Map updatedMap);
    void InsertMap(Map newMap);
    void DeleteMap(int id);
}