using SchoolRegister.Models;

namespace SchoolRegister.Services.SchoolClasses; 

public interface ISchoolClassesService {
    Task<int> GetCountAsync();

    Task<IEnumerable<SchoolClass>> GetAllAsync();

    Task<SchoolClass> GetById(int id);

    Task<bool> IsSchoolClassExisting(string name);

    Task AddAsync(SchoolClass schoolClass);

    Task UpdateAsync(SchoolClass schoolClass);

    Task DeleteAsync(int id);
}