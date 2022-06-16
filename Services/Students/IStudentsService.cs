using Microsoft.AspNetCore.Identity;
using SchoolRegister.Models;

namespace SchoolRegister.Services.Students; 

public interface IStudentsService {
    Task<int> GetCountAsync();

    Task<IEnumerable<Student>> GetAllAsync();

    Task<Student> GetById(int id);

    Task<Student> GetByUser(IdentityUser user);

    Task AddAsync(Student student);

    Task UpdateAsync(Student student);

    Task DeleteAsync(Student student);
}