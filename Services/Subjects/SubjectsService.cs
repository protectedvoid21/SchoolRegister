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

    public async Task<IEnumerable<StudentSubject>> GetSchoolSubjectsByTeacher(Teacher teacher) {
        return await schoolContext.StudentSubjects
            .Include(s => s.SchoolSubject)
            .Include(s => s.Student)
            .Include(s => s.Grades)
            .ToListAsync();
    }

    public async Task<SchoolSubject> GetSchoolSubjectById(int id) {
        return await schoolContext.SchoolSubjects.FindAsync(id);
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