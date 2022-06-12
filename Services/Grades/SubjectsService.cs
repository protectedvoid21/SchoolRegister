using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;
using SchoolRegister.Services.Persons;

namespace SchoolRegister.Services.Grades;

public class SubjectsService : ISubjectsService {
    private readonly SchoolRegisterContext schoolContext;
    private readonly IPersonsService personService;

    public SubjectsService(SchoolRegisterContext schoolContext, IPersonsService personService) {
        this.schoolContext = schoolContext;
        this.personService = personService;
    }

    public async Task AddGrade(Grade grade, int studentId) {
        await personService.GetStudentById(studentId);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<Grade> GetGrade(int id) {
        return await schoolContext.Grades.FindAsync(id);
    }

    public async Task UpdateGrade(Grade grade) {
        schoolContext.Grades.Update(grade);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteGrade(Grade grade) {
        schoolContext.Grades.Remove(grade);
        await schoolContext.SaveChangesAsync();
    }

    public async Task AddSubject(Subject subject) {
        await schoolContext.Subjects.AddAsync(subject);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<Subject> GetSubject(int id) {
        return await schoolContext.Subjects.FindAsync(id);
    }

    public async Task UpdateSubject(Subject subject) {
        schoolContext.Update(subject);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteSubject(Subject subject) {
        schoolContext.Remove(subject);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<int> GetSubjectCount() {
        return await schoolContext.Subjects.CountAsync();
    }

    public async Task<SchoolSubject> GetSchoolSubject(int id) {
        return await schoolContext.SchoolSubjects.FindAsync(id);
    }

    public async Task AddSchoolSubject(SchoolSubject schoolSubject) {
        await schoolContext.SchoolSubjects.AddAsync(schoolSubject);
        await schoolContext.SaveChangesAsync();
    }

    public async Task AddSchoolSubjectRange(IEnumerable<SchoolSubject> schoolSubjects) {
        await schoolContext.AddRangeAsync(schoolSubjects);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteSchoolSubject(SchoolSubject schoolSubject) {
        schoolContext.Remove(schoolSubject);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<List<SchoolSubject>> GetAllSchoolSubjects() {
        return await schoolContext.SchoolSubjects.Include(s => s.SchoolClass).Include(s => s.Subject).ToListAsync();
    }

    public async Task<bool> IsSubjectExisting(string name) {
        return await schoolContext.Subjects.AnyAsync(s => s.Name == name);
    }

    public async Task<List<Subject>> GetAllSubjects() {
        return await schoolContext.Subjects.ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetStudentGrades(int studentId, Subject subject) {
        var grades = await schoolContext.StudentSubjects
            .Include(s => s.Grades)
            .Where(s => s.Student.Id == studentId && s.Subject == subject)
            .SelectMany(s => s.Grades)
            .ToListAsync();
        return grades;
    }

    public async Task<float> GetStudentSubjectAverage(int studentId, Subject subject) {
        var grades = await GetStudentGrades(studentId, subject);
        return (float)grades.Average(p => p.GradeType);
    }

    public async Task<float> GetSubjectAverage(Subject subject, int classId) {
        SchoolClass schoolClass = await personService.GetSchoolClassById(classId);
        List<float> averageStudentGrades = new();

        var studentList = schoolClass.StudentsList;
        foreach (var student in studentList) {
            averageStudentGrades.Add(await GetStudentSubjectAverage(student.Id, subject));
        }

        return averageStudentGrades.Average();
    }
}