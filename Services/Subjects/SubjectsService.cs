using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Subjects;

public class SubjectsService : ISubjectsService {
    private readonly SchoolContext dbContext;

    public SubjectsService(SchoolContext dbContext) {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(string name) {
        Subject subject = new() {
            Name = name,
        };

        await dbContext.AddAsync(subject);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Subject> GetById(int id) {
        Subject subject = await dbContext.Subjects.FindAsync(id);
        return subject;
    }

    public async Task<StudentSubject> GetStudentSubjectById(int id) {
        return await dbContext.StudentSubjects
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
        dbContext.Update(subject);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        Subject subject = await dbContext.Subjects.FindAsync(id);
        dbContext.Remove(subject);
        await dbContext.SaveChangesAsync();
    }

    public async Task<int> GetCountAsync() {
        return await dbContext.Subjects.CountAsync();
    }

    public async Task<int> GetCountByStudents(int subjectId) {
        return await dbContext.StudentSubjects.Where(s => s.SchoolSubject.Subject.Id == subjectId).CountAsync();
    }

    public async Task<IEnumerable<StudentSubject>> GetStudentSubjectsForStudent(int studentId) {
        return dbContext.StudentSubjects
            .Where(s => s.Student.Id == studentId)
            .Include(s => s.Grades)
            .Include(s => s.SchoolSubject)
            .ThenInclude(s => s.Subject);
    }

    public async Task<IEnumerable<StudentSubject>> GetStudentSubjectsForTeacher(int teacherId) {
        return await dbContext.StudentSubjects
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
        return await dbContext.SchoolSubjects.FindAsync(id);
    }

    public async Task<IEnumerable<SchoolSubject>> GetSchoolSubjectsByClass(int schoolClassId) {
        return await dbContext.SchoolSubjects
            .Where(s => s.SchoolClassId == schoolClassId)
            .ToListAsync();
    }

    public async Task UpdateStudentSubjectsInClass(SchoolSubject schoolSubject) {
        IEnumerable<Student> studentList = dbContext.Students
            .Include(s => s.StudentSubjects)
            .Where(s => s.SchoolClass == schoolSubject.SchoolClass);

        foreach (var student in studentList) {
            if (!student.StudentSubjects.Select(s => s.SchoolSubject).Contains(schoolSubject)) {
                StudentSubject studentSubject = new() {
                    SchoolSubject = schoolSubject,
                    SchoolSubjectId = schoolSubject.Id,
                    Student = student,
                };
                await dbContext.AddAsync(studentSubject);
            }
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateSubjectsForStudent(Student student) {
        IEnumerable<SchoolSubject> schoolSubjects =
            dbContext.SchoolSubjects.Where(s => s.SchoolClass == student.SchoolClass);

        foreach (var schoolSubject in schoolSubjects) {
            if (!dbContext.StudentSubjects.Select(s => s.SchoolSubject).Contains(schoolSubject)) {
                StudentSubject studentSubject = new() {
                    SchoolSubject = schoolSubject,
                    SchoolSubjectId = schoolSubject.Id,
                    Student = student,
                };
                await dbContext.AddAsync(studentSubject);
            }
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task AddSchoolSubjectsAsync(int subjectId, IEnumerable<int> schoolClassIds, int teacherId) {
        IEnumerable<SchoolSubject> existingSubjects =
            dbContext.SchoolSubjects.Where(s => s.SubjectId == subjectId && s.TeacherId == teacherId);

        dbContext.RemoveRange(existingSubjects);

        List<SchoolSubject> schoolSubjects = new();
        foreach (var schoolClassId in schoolClassIds) {
            schoolSubjects.Add(new SchoolSubject {
                SchoolClassId = schoolClassId,
                SubjectId = subjectId,
                TeacherId = teacherId
            });
        }

        await dbContext.AddRangeAsync(schoolSubjects);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteSchoolSubjectAsync(int id) {
        SchoolSubject? schoolSubject = await dbContext.SchoolSubjects.FindAsync(id);
        if (schoolSubject == null) {
            return;
        }

        dbContext.Remove(schoolSubject);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<SchoolSubject>> GetAllSchoolSubjects() {
        return await dbContext.SchoolSubjects
            .Include(s => s.SchoolClass)
            .Include(s => s.Subject)
            .Include(s => s.Teacher)
            .ToListAsync();
    }

    public async Task<bool> IsSubjectExisting(string name) {
        return await dbContext.Subjects.AnyAsync(s => s.Name == name);
    }

    public bool IsSchoolSubjectExisting(int classId, int teacherId, int subjectId) {
        return dbContext.SchoolSubjects.Any(
            s => s.SchoolClass.Id == classId && s.Teacher.Id == teacherId && s.Subject.Id == subjectId);
    }

    public async Task<List<Subject>> GetAllSubjects() {
        return await dbContext.Subjects.ToListAsync();
    }
}