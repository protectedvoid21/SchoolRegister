using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;
using SchoolRegister.Services.Subjects;

namespace SchoolRegister.Services.Students;

public class StudentsService : IStudentsService {
    private readonly SchoolRegisterContext schoolContext;
    private readonly UserManager<AppUser> userManager;
    private readonly ISubjectsService subjectsService;

    public StudentsService(SchoolRegisterContext schoolContext, UserManager<AppUser> userManager, ISubjectsService subjectsService) {
        this.schoolContext = schoolContext;
        this.userManager = userManager;
        this.subjectsService = subjectsService;
    }

    public async Task<int> GetCountAsync() {
        return await schoolContext.Students.CountAsync();
    }

    public async Task<IEnumerable<Student>> GetAllAsync() {
        return await schoolContext.Students
            .Include(s => s.StudentSubjects)
            .Include(s => s.User)
            .ToListAsync();
    }

    public async Task<Student> GetById(int id) {
        return await schoolContext.Students
            .Include(s => s.StudentSubjects)
            .Include(s => s.User)
            .FirstAsync(s => s.Id == id);
    }

    public async Task<Student> GetByUser(AppUser user) {
        return await schoolContext.Students
            .Include(s => s.StudentSubjects)
            !.ThenInclude(s => s.Grades)
            .Include(s => s.SchoolClass)
            .FirstAsync(s => s.User == user);
    }

    public async Task AddAsync(string name, string surname, int schoolClassId) {
        var user = new AppUser {
            Name = name,
            Surname = surname,
            UserName = Utils.GenerateUserName(name, surname),
        };

        await userManager.CreateAsync(user, Utils.GeneratePassword(10));
        await userManager.AddToRoleAsync(user, "Student");

        Student student = new() {
            User = user,
            SchoolClassId = schoolClassId
        };

        await schoolContext.AddAsync(student);
        await subjectsService.UpdateSubjectsForStudent(student);

        await schoolContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, string name, string surname) {
        Student student = await GetById(id);
        student.User.Name = name;
        student.User.Surname = surname;
        student.User.UserName = Utils.GenerateUserName(name, surname);

        schoolContext.Update(student);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        Student student = await GetById(id);
        await userManager.DeleteAsync(student.User);
        schoolContext.Remove(student);

        await schoolContext.SaveChangesAsync();
    }
}