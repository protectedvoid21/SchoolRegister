using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;
using SchoolRegister.Services.Grades;
using SchoolRegister.Services.SchoolClasses;
using SchoolRegister.Services.Students;
using SchoolRegister.Services.Subjects;
using SchoolRegister.Services.Teachers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("SchoolRegister");
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SchoolRegisterContext>(config => config.UseSqlServer(connectionString));
builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
        options.Password.RequireNonAlphanumeric = false;
        //options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<SchoolRegisterContext>()
    .AddDefaultTokenProviders();
builder.Services.AddRazorPages();

builder.Services.AddTransient<ISchoolClassesService, SchoolClassesService>();
builder.Services.AddTransient<ISubjectsService, SubjectsService>();
builder.Services.AddTransient<ITeachersService, TeachersService>();
builder.Services.AddTransient<IGradesService, GradesService>();
builder.Services.AddTransient<IStudentsService, StudentsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
