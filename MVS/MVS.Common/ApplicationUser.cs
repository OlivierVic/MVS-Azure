// <copyright file="ApplicationUser.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;

namespace MVS.Common;

public class ApplicationUser : IdentityUser
{
    public int Gender { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public bool MutacAdh { get; set; }
    public string MutacNumber { get; set; }
    public string DisplayName => $"{this.FirstName} {this.LastName}";
    public string Initials => $"{this.FirstName[0]}{this.LastName[0]}";
}
