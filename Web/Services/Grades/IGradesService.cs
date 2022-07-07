using SchoolRegister.Models;

namespace SchoolRegister.Services.Grades; 

public interface IGradesService {
    Task AddAsync(Grade grade);

    Task<Grade> GetById(int id);

    Task UpdateAsync(Grade grade);

    Task DeleteAsync(Grade grade);

    Task<IEnumerable<Grade>> GetStudentGrades(int studentSubjectId);

    Task<bool> IsOwner(int gradeId, int studentId);

    /*Task<float> GetStudentSubjectAverage(int studentId, StudentSubject subject);

    Task<float> GetSubjectAverage(Subject subject, int classId);*/
}