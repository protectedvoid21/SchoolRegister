using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;

namespace SchoolRegister.Services.Persons; 

public class PersonsService : IPersonsService {
    private readonly SchoolRegisterContext schoolContext;
    
    public PersonsService(SchoolRegisterContext schoolContext) {
        this.schoolContext = schoolContext;
    }

    public async Task<int> GetStudentCount() {
        return await schoolContext.Students.CountAsync();
    }

    public async Task<int> GetTeacherCount() {
        return await schoolContext.Teachers.CountAsync();
    }

    public async Task<int> GetClassCount() {
        return await schoolContext.SchoolClasses.CountAsync();
    }

    public async Task<List<Student>> GetAllStudents() {
        return await schoolContext.Students
            .Include(s => s.StudentSubjects)
            .ToListAsync();
    }

    public async Task<List<SchoolClass>> GetAllSchoolClasses() {
        List<SchoolClass> schoolClasses = await schoolContext.SchoolClasses
            .Include(s => s.StudentsList)
            .Include(s => s.SchoolSubjects)
            .ToListAsync();
        return schoolClasses;
    }

    public async Task<List<Teacher>> GetAllTeachers() {
        List<Teacher> teachers = await schoolContext.Teachers.Include(s => s.SchoolSubjects).ToListAsync();
        return teachers;
    }

    public async Task<Student> GetStudentById(int id) {
        return await schoolContext.Students.Include(s => s.StudentSubjects).FirstAsync(s => s.Id == id);
    }

    public async Task<Teacher> GetTeacherById(int id) {
        return await schoolContext.Teachers.Include(t => t.SchoolSubjects).FirstAsync(t => t.Id == id);
    }

    public async Task<SchoolClass> GetSchoolClassById(int id) {
        return await schoolContext.SchoolClasses
            .Include(c => c.StudentsList)
            .Include(c => c.SchoolSubjects)
            .FirstAsync(c => c.Id == id);
    }

    public Task AddStudentSubject(StudentSubject studentSubject) {
        throw new NotImplementedException();
    }

    public async Task AddStudent(Student student) {
        await schoolContext.AddAsync(student);
        await schoolContext.SaveChangesAsync();
    }

    public async Task AddTeacher(Teacher teacher) {
        await schoolContext.Teachers.AddAsync(teacher);
        await schoolContext.SaveChangesAsync();
    }

    public async Task AddSchoolClass(SchoolClass schoolClass) {
        await schoolContext.SchoolClasses.AddAsync(schoolClass);
        await schoolContext.SaveChangesAsync();
    }

    public async Task UpdateStudent(Student student) {
        schoolContext.Update(student);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteStudent(Student student) {
        schoolContext.Remove(student);
        await schoolContext.SaveChangesAsync();
    }

    public async Task UpdateTeacher(Teacher teacher) {
        schoolContext.Update(teacher);
        await schoolContext.SaveChangesAsync();
    }

    public async Task DeleteTeacher(Teacher teacher) {
        schoolContext.Remove(teacher);
        await schoolContext.SaveChangesAsync();
    }
}