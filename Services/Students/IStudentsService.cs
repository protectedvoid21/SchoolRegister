using Microsoft.AspNetCore.Identity;
using SchoolRegister.Models;

namespace SchoolRegister.Services.Students;

public interface IStudentsService {
    Task<int> GetCountAsync();

    Task<IEnumerable<Student>> GetAllAsync();

    Task<Student> GetById(int id);

    Task<Student> GetByUser(AppUser user);

    Task AddAsync(string name, string surname, int schoolClassId);

    Task UpdateAsync(int id, string name, string surname);

    Task DeleteAsync(int id);
}