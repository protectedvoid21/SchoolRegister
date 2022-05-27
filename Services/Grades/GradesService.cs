using SchoolRegister.Models;
using SchoolRegister.Services.Persons;

namespace SchoolRegister.Services.Grades;

public class GradesService : IGradesService {
    private readonly SchoolRegisterContext schoolContext;
    private readonly IPersonsService personService;

    public GradesService(SchoolRegisterContext schoolContext, IPersonsService personService) {
        this.schoolContext = schoolContext;
        this.personService = personService;
    }

    public async Task AddGrade(Grade grade, int studentId) {
        Student student = await personService.GetStudentById(studentId);
        student.StudentSubjects.First(s => s.Subject == grade.Subject).Grades.Add(grade);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<Grade> GetGrade(int id) {
        Grade grade = await schoolContext.Grades.FindAsync(id);
        return grade;
    }

    public async Task UpdateGrade(Grade grade) {
        schoolContext.Grades.Update(grade);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteGrade(Grade grade) {
        schoolContext.Grades.Remove(grade);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Grade>> GetStudentGrades(int studentId, Subject subject) {
        Student student = await personService.GetStudentById(studentId);
        return student.StudentSubjects.First(p => p.Subject == subject).Grades;
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