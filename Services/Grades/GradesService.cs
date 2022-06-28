using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;
using SchoolRegister.Services.SchoolClasses;

namespace SchoolRegister.Services.Grades;

public class GradesService : IGradesService {
    private readonly SchoolRegisterContext schoolContext;
    private readonly ISchoolClassesService schoolClassesService;

    public GradesService(SchoolRegisterContext schoolContext, ISchoolClassesService schoolClassesService) {
        this.schoolContext = schoolContext;
        this.schoolClassesService = schoolClassesService;
    }

    //todo: Convert to Task<bool> - returns if student was found and adding result
    public async Task AddAsync(Grade grade) {
        await schoolContext.AddAsync(grade);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<Grade> GetById(int id) {
        return await schoolContext.Grades.FindAsync(id);
    }

    public async Task UpdateAsync(Grade grade) {
        schoolContext.Grades.Update(grade);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Grade grade) {
        schoolContext.Grades.Remove(grade);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Grade>> GetStudentGrades(int studentSubjectId) {
        IEnumerable<Grade> grades = schoolContext.Grades.Where(s => s.StudentSubject.Id == studentSubjectId);
        return grades;
    }



    /*public async Task<IEnumerable<Grade>> GetStudentGrades(int studentId, StudentSubject subject) {
        var grades = await schoolContext.StudentSubjects
            .Include(s => s.Grades)
            .Where(s => s.Student.Id == studentId && s.Subject == subject)
            .SelectMany(s => s.Grades)
            .ToListAsync();
        return grades;
    }

    public async Task<float> GetStudentSubjectAverage(int studentId, StudentSubject subject) {
        var grades = await GetStudentGrades(studentId, subject);
        return (float)grades.Average(p => p.GradeType);
    }

    public async Task<float> GetSubjectAverage(Subject subject, int classId) {
        SchoolClass schoolClass = await schoolClassesService.GetById(classId);
        List<float> averageStudentGrades = new();

        var studentList = schoolClass.StudentsList;
        foreach(var student in studentList) {
            averageStudentGrades.Add(await GetStudentSubjectAverage(student.Id, subject));
        }

        return averageStudentGrades.Average();
    }*/
}