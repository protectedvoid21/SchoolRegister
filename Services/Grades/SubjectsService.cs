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
        Student student = await personService.GetStudentById(studentId);
        student.StudentSubjects.First(s => s.SchoolSubject.Subject == grade.Subject).Grades.Add(grade);
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

    public Task<Subject> GetSubject(int id) {
        throw new NotImplementedException();
    }

    public Task UpdateSubject(Subject subject) {
        throw new NotImplementedException();
    }

    public Task DeleteSubject(Subject subject) {
        throw new NotImplementedException();
    }

    public async Task<int> GetSubjectCount() {
        return await schoolContext.Subjects.CountAsync();
    }

    public async Task AddSchoolSubject(SchoolSubject schoolSubject) {
        await schoolContext.SchoolSubjects.AddAsync(schoolSubject);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<bool> IsSubjectExisting(string name) {
        return await schoolContext.Subjects.AnyAsync(s => s.Name == name);
    }

    public async Task<List<Subject>> GetAllSubjects() {
        return await schoolContext.Subjects.ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetStudentGrades(int studentId, Subject subject) {
        Student student = await personService.GetStudentById(studentId);
        return student.StudentSubjects.First(p => p.SchoolSubject.Subject == subject).Grades;
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