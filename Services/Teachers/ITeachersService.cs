using Microsoft.AspNetCore.Identity;
using SchoolRegister.Models;

namespace SchoolRegister.Services.Teachers;

public interface ITeachersService {
    Task<int> GetCountAsync();

    Task<IEnumerable<Teacher>> GetAllAsync();

    Task<Teacher> GetById(int id);

    Task<Teacher> GetByUser(AppUser user);

    Task<IEnumerable<SchoolSubject>> GetTaughtSubjects(Teacher teacher);

    Task<bool> IsTeacherGradeAuthor(int teacherId, int gradeId);

    Task AddAsync(Teacher teacher);

    Task UpdateAsync(Teacher teacher);

    Task DeleteAsync(Teacher teacher);
}