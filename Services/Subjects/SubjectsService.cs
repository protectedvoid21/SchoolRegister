using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;
using SchoolRegister.Services.Subjects;

namespace SchoolRegister.Services.Subjects;

public class SubjectsService : ISubjectsService {
    private readonly SchoolRegisterContext schoolContext;

    public SubjectsService(SchoolRegisterContext schoolContext) {
        this.schoolContext = schoolContext;
    }

    public async Task AddAsync(Subject subject) {
        await schoolContext.Subjects.AddAsync(subject);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<Subject> GetById(int id) {
        return await schoolContext.Subjects.FindAsync(id);
    }

    public async Task<StudentSubject> GetStudentSubjectById(int id) {
        return await schoolContext.StudentSubjects
            .Include(s => s.SchoolSubject)
            .ThenInclude(s => s.Subject)
            .Include(s => s.Student)
            .FirstAsync(s => s.Id == id);
    }

    public async Task UpdateAsync(Subject subject) {
        schoolContext.Update(subject);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Subject subject) {
        schoolContext.Remove(subject);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<int> GetCountAsync() {
        return await schoolContext.Subjects.CountAsync();
    }

    public async Task<int> GetCountByStudents(Subject subject) {
        return await schoolContext.StudentSubjects.Where(s => s.SchoolSubject.Subject == subject).CountAsync();
    }

    public async Task<IEnumerable<StudentSubject>> GetStudentSubjectForStudent(Student student) {
        return schoolContext.StudentSubjects
            .Where(s => s.Student == student)
            .Include(s => s.Grades)
            .Include(s => s.SchoolSubject)
            .ThenInclude(s => s.Subject);
    }

    public async Task<IEnumerable<StudentSubject>> GetSchoolSubjectsByTeacher(Teacher teacher) {
        return await schoolContext.StudentSubjects
            .Include(s => s.SchoolSubject)
            .ThenInclude(s => s.Subject)
            .Include(s => s.SchoolSubject)
            .ThenInclude(s => s.SchoolClass)
            .Include(s => s.Student)
            .Include(s => s.Grades)
            .ToListAsync();
    }

    public async Task<SchoolSubject> GetSchoolSubjectById(int id) {
        return await schoolContext.SchoolSubjects.FindAsync(id);
    }

    public async Task<IEnumerable<SchoolSubject>> GetSchoolSubjectsByClass(SchoolClass schoolClass) {
        return await schoolContext.SchoolSubjects
            .Where(s => s.SchoolClass == schoolClass)
            .ToListAsync();
    }

    public async Task UpdateStudentSubjectsInClass(SchoolSubject schoolSubject) {
        IEnumerable<Student> studentList = schoolContext.Students
            .Include(s => s.StudentSubjects)
            .Where(s => s.SchoolClass == schoolSubject.SchoolClass);

        foreach (var student in studentList) {
            if (!student.StudentSubjects.Select(s => s.SchoolSubject).Contains(schoolSubject)) {
                StudentSubject studentSubject = new() {
                    SchoolSubject = schoolSubject,
                    SchoolSubjectId = schoolSubject.Id,
                    Student = student,
                };
                await schoolContext.AddAsync(studentSubject);
            }
        }
        await schoolContext.SaveChangesAsync();
    }

    public async Task UpdateSubjectsForStudent(Student student) {
        IEnumerable<SchoolSubject> schoolSubjects = schoolContext.SchoolSubjects.Where(s => s.SchoolClass == student.SchoolClass);

        foreach (var schoolSubject in schoolSubjects) {
            if (!schoolContext.StudentSubjects.Select(s => s.SchoolSubject).Contains(schoolSubject)) {
                StudentSubject studentSubject = new() {
                    SchoolSubject = schoolSubject,
                    SchoolSubjectId = schoolSubject.Id,
                    Student = student,
                };
                await schoolContext.AddAsync(studentSubject);
            }
        }
        await schoolContext.SaveChangesAsync();
    }

    public async Task AddSchoolSubjectAsync(SchoolSubject schoolSubject) {
        await schoolContext.SchoolSubjects.AddAsync(schoolSubject);
        await schoolContext.SaveChangesAsync();
    }

    public async Task AddSchoolSubjectRangeAsync(IEnumerable<SchoolSubject> schoolSubjects) {
        schoolSubjects = schoolSubjects.Where(s => !IsSchoolSubjectExisting(s.SchoolClass.Id, s.Teacher.Id, s.Subject.Id));
        await schoolContext.AddRangeAsync(schoolSubjects);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteSchoolSubjectAsync(SchoolSubject schoolSubject) {
        schoolContext.Remove(schoolSubject);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<List<SchoolSubject>> GetAllSchoolSubjects() {
        return await schoolContext.SchoolSubjects
            .Include(s => s.SchoolClass)
            .Include(s => s.Subject)
            .Include(s => s.Teacher)
            .ToListAsync();
    }

    public async Task<bool> IsSubjectExisting(string name) {
        return await schoolContext.Subjects.AnyAsync(s => s.Name == name);
    }

    public bool IsSchoolSubjectExisting(int classId, int teacherId, int subjectId) {
        return schoolContext.SchoolSubjects.Any(
            s => s.SchoolClass.Id == classId && s.Teacher.Id == teacherId && s.Subject.Id == subjectId);
    }

    public async Task<List<Subject>> GetAllSubjects() {
        return await schoolContext.Subjects.ToListAsync();
    }
}