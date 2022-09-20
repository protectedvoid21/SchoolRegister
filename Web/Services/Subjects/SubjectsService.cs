using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;
using SchoolRegister.Services.Subjects;

namespace SchoolRegister.Services.Subjects;

public class SubjectsService : ISubjectsService {
    private readonly SchoolContext schoolContext;

    public SubjectsService(SchoolContext schoolContext) {
        this.schoolContext = schoolContext;
    }

    public async Task AddAsync(string name) {
        Subject subject = new() {
            Name = name,
        };

        await schoolContext.AddAsync(subject);
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
            .ThenInclude(s => s.User)
            .FirstAsync(s => s.Id == id);
    }

    public async Task UpdateAsync(int id, string name) {
        Subject subject = new() {
            Id = id,
            Name = name,
        };
        schoolContext.Update(subject);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        Subject subject = await schoolContext.Subjects.FindAsync(id);
        schoolContext.Remove(subject);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<int> GetCountAsync() {
        return await schoolContext.Subjects.CountAsync();
    }

    public async Task<int> GetCountByStudents(Subject subject) {
        return await schoolContext.StudentSubjects.Where(s => s.SchoolSubject.Subject == subject).CountAsync();
    }

    public async Task<IEnumerable<StudentSubject>> GetStudentSubjectsForStudent(Student student) {
        return schoolContext.StudentSubjects
            .Where(s => s.Student == student)
            .Include(s => s.Grades)
            .Include(s => s.SchoolSubject)
            .ThenInclude(s => s.Subject);
    }

    public async Task<IEnumerable<StudentSubject>> GetStudentSubjectsForTeacher(int teacherId) {
        return await schoolContext.StudentSubjects
            .Include(s => s.SchoolSubject)
            .ThenInclude(s => s.Subject)
            .Include(s => s.SchoolSubject)
            .ThenInclude(s => s.SchoolClass)
            .Include(s => s.Student)
            .Include(s => s.Grades)
            .Where(t => t.SchoolSubject.TeacherId == teacherId)
            .ToListAsync();
    }

    public async Task<SchoolSubject> GetSchoolSubjectById(int id) {
        return await schoolContext.SchoolSubjects.FindAsync(id);
    }

    public async Task<IEnumerable<SchoolSubject>> GetSchoolSubjectsByClass(int schoolClassId) {
        return await schoolContext.SchoolSubjects
            .Where(s => s.SchoolClassId == schoolClassId)
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

    public async Task AddSchoolSubjectAsync(int subjectId, int schoolClassId, int teacherId) {
        SchoolSubject schoolSubject = new() {
            SubjectId = subjectId,
            SchoolClassId = schoolClassId,
            TeacherId = teacherId
        };

        await schoolContext.AddAsync(schoolSubject);
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