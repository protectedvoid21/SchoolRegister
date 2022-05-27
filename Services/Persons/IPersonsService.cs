using SchoolRegister.Models;

namespace SchoolRegister.Services.Persons; 

public interface IPersonsService {
    public Task<int> GetStudentCount();
    public Task<int> GetTeacherCount();
    public Task<int> GetClassCount();

    public Task<Student> GetStudentById(int studentId);
    public Task<Teacher> GetTeacherById(int id);
    public Task<SchoolClass> GetSchoolClassById(int id);

    public Task AddStudent(Student student);
    public Task AddTeacher(Teacher teacher);
    public Task AddSchoolClass(SchoolClass schoolClass);

    public Task UpdateStudent(Student student);
    public Task DeleteStudent(Student student);

    public Task UpdateTeacher(Teacher teacher);
    public Task DeleteTeacher(Teacher teacher);
}