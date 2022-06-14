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
            .ToListAsync();
    }

    public async Task<Student> GetById(int id) {
        return await schoolContext.Students.Include(s => s.StudentSubjects).FirstAsync(s => s.Id == id);
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