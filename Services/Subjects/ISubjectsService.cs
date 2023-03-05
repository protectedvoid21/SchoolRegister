using Data.Models;

namespace Services.Subjects;

public interface ISubjectsService {
    Task AddAsync(string name);

    Task<Subject> GetById(int id);

    Task<StudentSubject> GetStudentSubjectById(int id);

    Task UpdateAsync(int id, string name);

    Task DeleteAsync(int id);

    Task<int> GetCountAsync();

    Task<int> GetCountByStudents(int subjectId);

    Task<IEnumerable<StudentSubject>> GetStudentSubjectsForStudent(int studentId);

    Task<IEnumerable<StudentSubject>> GetStudentSubjectsForTeacher(int teacherId);

    Task<SchoolSubject> GetSchoolSubjectById(int id);

    Task<IEnumerable<SchoolSubject>> GetSchoolSubjectsByClass(int schoolClassId);

    Task UpdateStudentSubjectsInClass(SchoolSubject schoolSubject);

    Task UpdateSubjectsForStudent(Student student);

    Task AddSchoolSubjectsAsync(int subjectId, IEnumerable<int> schoolClassIds, int teacherId);

    Task DeleteSchoolSubjectAsync(int id);

    Task<List<SchoolSubject>> GetAllSchoolSubjects();

    Task<bool> IsSubjectExisting(string name);

    //Task<bool> IsSchoolSubjectExisting(int classId, int teacherId, int subjectId);

    Task<List<Subject>> GetAllSubjects();
}