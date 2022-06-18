using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;

namespace SchoolRegister.Services.Students; 

public class StudentsService : IStudentsService {
    private readonly SchoolRegisterContext schoolContext;

    public StudentsService(SchoolRegisterContext schoolContext) {
        this.schoolContext = schoolContext;
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
            .Include(s => s.Class)
            .FirstAsync(s => s.User == user);
    }

    public async Task AddAsync(Student student) {
        await schoolContext.AddAsync(student);
        await schoolContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Student student) {
        schoolContext.Update(student);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Student student) {
        schoolContext.Remove(student);
        await schoolContext.SaveChangesAsync();
    }
}