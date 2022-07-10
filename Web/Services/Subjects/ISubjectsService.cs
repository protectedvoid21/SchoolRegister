using SchoolRegister.Models;

namespace SchoolRegister.Services.Subjects;

public interface ISubjectsService {
    Task AddAsync(string name);

    Task<Subject> GetById(int id);

    Task<StudentSubject> GetStudentSubjectById(int id);

    Task UpdateAsync(int id, string name);

    Task DeleteAsync(int id);

    Task<int> GetCountAsync();

    Task<int> GetCountByStudents(Subject subject);

    Task<IEnumerable<StudentSubject>> GetStudentSubjectsForStudent(Student student);

    Task<IEnumerable<StudentSubject>> GetStudentSubjectsForTeacher(int teacherId);

    Task<SchoolSubject> GetSchoolSubjectById(int id);

    Task<IEnumerable<SchoolSubject>> GetSchoolSubjectsByClass(int schoolClassId);

    Task UpdateStudentSubjectsInClass(SchoolSubject schoolSubject);

    Task UpdateSubjectsForStudent(Student student);

    Task AddSchoolSubjectAsync(int subjectId, int schoolClassId, int teacherId);

    Task AddSchoolSubjectRangeAsync(IEnumerable<SchoolSubject> schoolSubjects);

    Task DeleteSchoolSubjectAsync(SchoolSubject schoolSubject);

    Task<List<SchoolSubject>> GetAllSchoolSubjects();

    Task<bool> IsSubjectExisting(string name);

    //Task<bool> IsSchoolSubjectExisting(int classId, int teacherId, int subjectId);

    Task<List<Subject>> GetAllSubjects();
}