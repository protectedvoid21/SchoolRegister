using SchoolRegister.Models;

namespace SchoolRegister.Services.Grades;

public interface ISubjectsService {
    Task AddGrade(Grade grade, int studentId);
    Task<Grade> GetGrade(int id);
    Task UpdateGrade(Grade grade);
    Task DeleteGrade(Grade grade);

    Task AddSubject(Subject subject);
    Task<Subject> GetSubject(int id);
    Task UpdateSubject(Subject subject);
    Task DeleteSubject(Subject subject);

    Task<int> GetSubjectCount();

    Task AddSchoolSubject(SchoolSubject schoolSubject);

    Task<bool> IsSubjectExisting(string name);

    Task<List<Subject>> GetAllSubjects();

    Task<IEnumerable<Grade>> GetStudentGrades(int studentId, Subject subject);
    Task<float> GetStudentSubjectAverage(int studentId, Subject subject);
    Task<float> GetSubjectAverage(Subject subject, int classId);
}