using SchoolRegister.Models;

namespace SchoolRegister.Services.Grades;

public interface IGradesService {
    public Task AddGrade(Grade grade, int studentId);
    public Task<Grade> GetGrade(int id);
    public Task UpdateGrade(Grade grade);
    public Task DeleteGrade(Grade grade);

    Task<IEnumerable<Grade>> GetStudentGrades(int id, Subject subject);
    Task<float> GetStudentSubjectAverage(int id, Subject subject);
    Task<float> GetSubjectAverage(Subject subject, int classId);
}