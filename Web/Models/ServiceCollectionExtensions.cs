using Microsoft.AspNetCore.Identity;
using SchoolRegister.Services.Announcements;
using SchoolRegister.Services.Grades;
using SchoolRegister.Services.Messages;
using SchoolRegister.Services.SchoolClasses;
using SchoolRegister.Services.Students;
using SchoolRegister.Services.Subjects;
using SchoolRegister.Services.Teachers;

namespace SchoolRegister.Models; 

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddApplicationServices(this IServiceCollection service) {
        return service
            .AddTransient<ISchoolClassesService, SchoolClassesService>()
            .AddTransient<ISubjectsService, SubjectsService>()
            .AddTransient<ITeachersService, TeachersService>()
            .AddTransient<IGradesService, GradesService>()
            .AddTransient<IStudentsService, StudentsService>()
            .AddTransient<IAnnouncementsService, AnnouncementsService>()
            .AddTransient<IMessagesService, MessagesService>();
    }

    public static IServiceCollection AddIdentity(this IServiceCollection service) {
        service.AddIdentity<AppUser, IdentityRole>(options => {
                options.Password.RequireNonAlphanumeric = false;
                //options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<SchoolContext>()
            .AddDefaultTokenProviders();
        return service;
    }
}