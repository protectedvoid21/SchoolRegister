using SchoolRegister.Models;

namespace SchoolRegister.Services.Subjects;

public interface ISubjectsService {
    Task AddAsync(Subject subject);

    Task<Subject> GetById(int id);

    Task<StudentSubject> GetStudentSubjectById(int id);

    Task UpdateAsync(Subject subject);

    Task DeleteAsync(Subject subject);

    Task<int> GetCountAsync();

    Task<int> GetCountByStudents(Subject subject);

    Task<IEnumerable<StudentSubject>> GetSchoolSubjectsByTeacher(Teacher teacher);

    Task<SchoolSubject> GetSchoolSubjectById(int id);

    Task<IEnumerable<SchoolSubject>> GetSchoolSubjectsByClass(SchoolClass schoolClass, Subject subject);

    Task UpdateStudentSubjectsInClass(SchoolSubject schoolSubject);

    Task UpdateSubjectsForStudent(Student student);

    Task AddSchoolSubjectAsync(SchoolSubject schoolSubject);

    Task AddSchoolSubjectRangeAsync(IEnumerable<SchoolSubject> schoolSubjects);

    Task DeleteSchoolSubjectAsync(SchoolSubject schoolSubject);

    Task<List<SchoolSubject>> GetAllSchoolSubjects();

    Task<bool> IsSubjectExisting(string name);

    //Task<bool> IsSchoolSubjectExisting(int classId, int teacherId, int subjectId);

    Task<List<Subject>> GetAllSubjects();
}