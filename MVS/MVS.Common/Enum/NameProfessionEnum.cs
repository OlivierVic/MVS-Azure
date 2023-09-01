// <copyright file="NameProfessionEnum.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

namespace MVS.Common.Enum;

public enum NameProfession
{
    Null = 0,
    Farmers = 1, // Farmers (employees of their holding)
    Artisans = 2, // Craftsmen (employees of their company)
    TradersAndSimilar = 3, // Traders and similar (employees of their company)
    HeadsOfEnterprises = 4, // Heads of enterprise with 10 or more employees (employees of their enterprise)
    LiberalProfessions = 5, // Liberal professions (exercised under employee status)
    PublicServiceExecutives = 6, // public service executives
    ProfessorsScientificProfessions = 7, // Professors, scientific professions
    InformationArts = 8, // Information, arts and entertainment professions
    BusinessAndAdministrativeStaff = 9, // Business and administrative staff
    Engineers = 10, // Engineers and technical staff of companies
    TeachersInSchools = 11, // Teachers in schools, teachers and related professions
    IntermediateProfessionsInHealth = 12, // Intermediate professions in health and social work
    ClergyReligious = 13, // Clergy, religious
    IntermediateAdministrativeOccupations = 14, // Intermediate administrative occupations in the public service
    IntermediateBusiness = 15, // Intermediate business and administrative occupations
    Technicians = 16, // Technicians (except tertiary technicians)
    ForemenSupervisors = 17, // Foremen, supervisors (excluding administrative control)
    CivilianEmployees = 18, // Civilian Employees and Public Service Officers
    PoliceMilitary = 19, // Police, military and surveillance officers
    BusinessAdministrationEmployees = 20, // Business Administration Employees
    CommercialWorkers = 21, // Commercial Workers
    PersonnelInDirectServices = 22, // Personnel in direct services to individuals
    SkilledIndustrialWorkers = 23, // Skilled industrial workers
    SkilledCraftWorkers = 24, // Skilled craft workers
    Drivers = 25, // drivers
    MaterialHandling = 26, // Material handling, warehousing and transport trades
    UnskilledIndustrialWorkers = 27, // Unskilled industrial workers
    UnskilledCraftWorkers = 28, // Unskilled craft workers
    AgriculturalWorkers = 29, // Agricultural and related manual workers
}