using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;
using SchoolRegister.Services.SchoolClasses;

namespace SchoolRegister.Services.Grades;

public class GradesService : IGradesService {
    private readonly SchoolRegisterContext schoolContext;

    public GradesService(SchoolRegisterContext schoolContext) {
        this.schoolContext = schoolContext;
    }

    public async Task AddAsync(Grade grade) {
        await schoolContext.AddAsync(grade);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<Grade> GetById(int id) {
        return await schoolContext.Grades
            .Include(g => g.Subject)
            .Include(g => g.StudentSubject)
            .ThenInclude(g => g.Student)
            .FirstAsync(g => g.Id == id);
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

    public async Task<bool> IsOwner(int gradeId, int studentId) {
        var grade = await schoolContext.Grades
            .Include(g => g.StudentSubject.Student)
            .FirstAsync(g => g.Id == gradeId);
        return grade.StudentSubject.Student.Id == studentId;
    }

    public async Task<bool> IsTeaching(int gradeId, int teacherId) {
        var grade = await schoolContext.Grades
            .Include(g => g.StudentSubject.SchoolSubject.Teacher)
            .FirstAsync(g => g.Id == gradeId);

        return grade.StudentSubject.SchoolSubject.Teacher.Id == teacherId;
    }


    /*public async Task<IEnumerable<Grade>> GetStudentGrades(int studentId, StudentSubject subject) {
        var grades = await schoolContext.StudentSubjects
            .Include(s => s.Grades)
            .Where(s => s.Student.Id == studentId && s.Subject == subject)
            .SelectMany(s => s.Grades)
            .ToListAsync();
        return grades;
    }*/
}