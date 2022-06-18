using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;

namespace SchoolRegister.Services.Teachers; 

public class TeachersService : ITeachersService {
    private readonly SchoolRegisterContext schoolContext;
    
    public TeachersService(SchoolRegisterContext schoolContext) {
        this.schoolContext = schoolContext;
    }

    public async Task<int> GetCountAsync() {
        return await schoolContext.Teachers.CountAsync();
    }

    public async Task<IEnumerable<Teacher>> GetAllAsync() {
        return await schoolContext.Teachers
            .Include(s => s.SchoolSubjects)
            .ThenInclude(s => s.SchoolClass)
            .Include(s => s.SchoolSubjects)
            .ThenInclude(s => s.Subject)
            .Include(t => t.User)
            .ToListAsync();
    }

    public async Task<Teacher> GetById(int id) {
        return await schoolContext.Teachers
            .Include(t => t.SchoolSubjects)
            .ThenInclude(t => t.SchoolClass)
            .Include(t => t.SchoolSubjects)
            .ThenInclude(t => t.Subject)
            .Include(t => t.User)
            .FirstAsync(t => t.Id == id);
    }

    public async Task<Teacher> GetByUser(AppUser user) {
        return await schoolContext.Teachers
            .Include(t => t.SchoolSubjects)
            .ThenInclude(t => t.SchoolClass)
            .Include(t => t.SchoolSubjects)
            .ThenInclude(t => t.Subject)
            .Include(t => t.User)
            .FirstAsync(t => t.User == user);
    }

    public async Task<IEnumerable<SchoolSubject>> GetTaughtSubjects(Teacher teacher) {
        return await schoolContext.SchoolSubjects.Where(s => s.Teacher == teacher).ToListAsync();
    }

    public async Task AddAsync(Teacher teacher) {
        await schoolContext.AddAsync(teacher);
        await schoolContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Teacher teacher) {
        schoolContext.Update(teacher);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Teacher teacher) {
        schoolContext.Remove(teacher);
        await schoolContext.SaveChangesAsync();
    }
}