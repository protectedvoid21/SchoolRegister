using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;

namespace SchoolRegister.Services.SchoolClasses; 

public class SchoolClassesService : ISchoolClassesService {
    private readonly SchoolRegisterContext schoolContext;

    public SchoolClassesService(SchoolRegisterContext schoolContext) {
        this.schoolContext = schoolContext;
    }

    public async Task<int> GetCountAsync() {
        return await schoolContext.SchoolClasses.CountAsync();
    }

    public async Task<IEnumerable<SchoolClass>> GetAllAsync() {
        return await schoolContext.SchoolClasses
            .Include(s => s.StudentsList)
            .ThenInclude(s => s.User)
            .Include(s => s.SchoolSubjects)
            .ToListAsync();
    }

    public async Task<SchoolClass> GetById(int id) {
        return await schoolContext.SchoolClasses
            .Include(c => c.StudentsList)
            .ThenInclude(c => c.User)
            .Include(c => c.SchoolSubjects)
            .FirstAsync(c => c.Id == id);
    }

    public async Task<bool> IsSchoolClassExisting(string name) {
        return await schoolContext.SchoolClasses.AnyAsync(c => c.Name == name);
    }

    public async Task AddAsync(SchoolClass schoolClass) {
        await schoolContext.AddAsync(schoolClass);
        await schoolContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(SchoolClass schoolClass) {
        schoolContext.Update(schoolClass);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        schoolContext.Remove(await schoolContext.SchoolClasses.FindAsync(id));
        await schoolContext.SaveChangesAsync();
    }
}