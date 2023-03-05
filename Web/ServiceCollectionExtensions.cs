using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Services.Announcements;
using Services.Grades;
using Services.Messages;
using Services.SchoolClasses;
using Services.Students;
using Services.Subjects;
using Services.Teachers;

namespace Data.Models;

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