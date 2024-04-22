/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using System;
using System.Collections.Generic;

namespace robot_controller_api.Models;

// LoginModel class with properties of the map
public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }

    // Login Constructor
    public LoginModel(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public LoginModel()
    {
        // Default constructor
    }
}