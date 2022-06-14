using SchoolRegister.Models;

namespace SchoolRegister.Services.Students; 

public interface IStudentsService {
    Task<int> GetCountAsync();

    Task<IEnumerable<Student>> GetAllAsync();

    Task<Student> GetById(int id);

    Task AddAsync(Student student);

    Task UpdateAsync(Student student);

    Task DeleteAsync(Student student);
}