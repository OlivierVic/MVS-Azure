// <copyright file="Program.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Business;
using MVS.Common;
using MVS.Common.Interfaces;
using MVS.EmailSender;
using MVS.EmailSender.Sender;
using MVS.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smartclause.SDK;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*builder.Services.AddControllersWithViews().Add
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);*/


string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedEmail = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Home/Login";
    options.LogoutPath = "/Home/Logout";
});

builder.Services.AddRazorPages();

builder.Services.AddTransient<IVaultService, VaultService>();
builder.Services.AddTransient<IVaultContactService, VaultContactService>();
builder.Services.AddTransient<IVaultCategoryService, VaultCategoryService>();
builder.Services.AddTransient<IVaultContractService, VaultContractService>();
builder.Services.AddTransient<IAnswerService, AnswerService>();
builder.Services.AddTransient<IVaultAnswerHeritageService, VaultAnswerHeritageService>();
builder.Services.AddTransient<IVaultAnswerDigitalLifeService, VaultAnswerDigitalLifeService>();
builder.Services.AddTransient<IVaultAnswerAnticipationMeasuresService, VaultAnswerAnticipationMeasuresService>();
builder.Services.AddTransient<IJobParticularService, JobParticularService>();
builder.Services.AddTransient<IVaultPersonalInfoService, VaultPersonalInfoService>();
builder.Services.AddTransient<IVaultFamilyInfoService, VaultFamilyInfoService>();
builder.Services.AddTransient<IVaultAnticipationMeasuresInfoService, VaultAnticipationMeasuresInfoService>();
builder.Services.AddTransient<IVaultDocumentService, VaultDocumentService>();
builder.Services.AddTransient<IVaultHeritageService, VaultHeritageService>();
builder.Services.AddTransient<IJobProfessionelService, JobProfessionelService>();
builder.Services.AddTransient<IAspNetUserService, AspNetUserService>();
builder.Services.AddTransient<IVaultUsersService, VaultUsersService>();
builder.Services.AddTransient<ICounterVaultCreateService, CounterVaultCreateService>();

builder.Services.AddTransient<IVaultAdministrativeLifeService, VaultAdministrativeLifeService>();
builder.Services.AddTransient<IVaultDigitalLifeService, VaultDigitalLifeService>();
builder.Services.AddTransient<IVaultFuneraryVolonteService, VaultFuneraryVolonteService>();
builder.Services.AddTransient<IVaultTiersContactService, VaultTiersContactService>();

builder.Services.AddEmailing(options =>
{
    options.Host = builder.Configuration["EmailSettings:Host"];
    options.Port = int.Parse(s: builder.Configuration["EmailSettings:Port"]);
    options.SenderEmail = builder.Configuration["EmailSettings:SenderEmail"];
    options.SenderName = builder.Configuration["EmailSettings:SenderName"];
    options.Username = builder.Configuration["EmailSettings:Username"];
    options.Password = builder.Configuration["EmailSettings:Password"];
});
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddTransient((provider =>
{
    IConfiguration configuration = provider.GetService<IConfiguration>();
    return new Client(
        builder.Configuration["SCM:Email"],
        builder.Configuration["SCM:Pwd"],
        builder.Configuration["SCM:Url"]);
}));

WebApplication? app = builder.Build();

string license = Path.Combine(app.Environment.WebRootPath, "licences", "Aspose.Total.lic");
new Aspose.Words.License().SetLicense(license);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
