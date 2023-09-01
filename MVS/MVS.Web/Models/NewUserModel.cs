// <copyright file="RegisterModel.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

namespace MVS.Web.Models;

public class NewUserModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Case { get; set; }
    public string Company { get; set; }
    public string Link { get; set; }
}
