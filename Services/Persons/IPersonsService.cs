using SchoolRegister.Models;

namespace SchoolRegister.Services.Persons; 

public interface IPersonsService {
    Task<int> GetStudentCount();
    Task<int> GetTeacherCount();
    Task<int> GetClassCount();

    Task<List<Student>> GetAllStudents();
    Task<List<SchoolClass>> GetAllSchoolClasses();
    Task<List<Teacher>> GetAllTeachers();

    Task<Student> GetStudentById(int studentId);
    Task<Teacher> GetTeacherById(int id);
    Task<SchoolClass> GetSchoolClassById(int id);

    Task AddStudentSubject(StudentSubject studentSubject);

    Task AddStudent(Student student);
    Task AddTeacher(Teacher teacher);
    Task AddSchoolClass(SchoolClass schoolClass);

    Task UpdateStudent(Student student);
    Task DeleteStudent(Student student);

    Task UpdateTeacher(Teacher teacher);
    Task DeleteTeacher(Teacher teacher);
}