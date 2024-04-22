/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;
public class MapEF : IMapDataAccess
{
    private readonly RobotContext _context;

    public MapEF(RobotContext context)
    {
        _context = context;
    }

    public List<Map> GetAllMaps()
    {
        return _context.Maps.ToList();
    }

    public void UpdateMap(Map updatedMap)
    {
        var existingMap = _context.Maps.Find(updatedMap.Id);
        if (existingMap != null)
        {
            existingMap.Columns = updatedMap.Columns;
            existingMap.Rows = updatedMap.Rows;
            existingMap.Name = updatedMap.Name;
            existingMap.Description = updatedMap.Description;
            existingMap.ModifiedDate = updatedMap.ModifiedDate ?? DateTime.Now;

            _context.SaveChanges();
        }
    }

    public void InsertMap(Map newMap)
    {
        newMap.CreatedDate ??= DateTime.Now;
        newMap.ModifiedDate ??= DateTime.Now;

        _context.Maps.Add(newMap);
        _context.SaveChanges();
    }

    public void DeleteMap(int id)
    {
        var mapToDelete = _context.Maps.Find(id);
        if (mapToDelete != null)
        {
            _context.Maps.Remove(mapToDelete);
            _context.SaveChanges();
        }
    }
}