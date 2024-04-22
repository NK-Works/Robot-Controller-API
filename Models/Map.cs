/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using System;
using System.Collections.Generic;

namespace robot_controller_api.Models;

// Map class with properties of the map
public class Map
{
    public int Id { get; set; }
    public int Columns { get; set; }
    public int Rows { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsSquare { get; set; }

    // Map Constructor
    public Map(int id, int columns, int rows, string name, DateTime? createdDate, DateTime? modifiedDate, bool isSquare, string? description = null)
    {
        Id = id;
        Columns = columns;
        Rows = rows;
        Name = name;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
        IsSquare = isSquare;
        Description = description;
    }

    public Map()
    {
        // Default constructor
    }
}