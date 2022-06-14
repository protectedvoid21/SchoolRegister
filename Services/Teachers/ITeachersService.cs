using SchoolRegister.Models;

namespace SchoolRegister.Services.Teachers;

public interface ITeachersService {
    Task<int> GetCountAsync();

    Task<IEnumerable<Teacher>> GetAllAsync();

    Task<Teacher> GetById(int id);

    Task AddAsync(Teacher teacher);

    Task UpdateAsync(Teacher teacher);

    Task DeleteAsync(Teacher teacher);
}