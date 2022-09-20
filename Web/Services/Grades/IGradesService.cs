using SchoolRegister.Models;

namespace SchoolRegister.Services.Grades; 

public interface IGradesService {
    Task AddAsync(int subjectId, int studentSubjectId, int gradeType, GradeAdditionalInfo gradeInfo, string comment);

    Task<TModel> GetById<TModel>(int id);

    Task UpdateAsync(int id, int gradeType, GradeAdditionalInfo gradeInfo, string comment);

    Task DeleteAsync(int id);

    Task<IEnumerable<Grade>> GetStudentGrades(int studentSubjectId);

    Task<bool> IsOwner(int gradeId, int studentId);

    /*Task<float> GetStudentSubjectAverage(int studentId, StudentSubject subject);

    Task<float> GetSubjectAverage(Subject subject, int classId);*/
}