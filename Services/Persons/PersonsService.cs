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

    public async Task<Student> GetStudentById(int id) {
        return await schoolContext.Students.FindAsync(id);
    }

    public async Task<Teacher> GetTeacherById(int id) {
        return await schoolContext.Teachers.FindAsync(id);
    }

    public async Task<SchoolClass> GetSchoolClassById(int id) {
        return await schoolContext.SchoolClasses.FindAsync(id);
    }

    public async Task AddStudent(Student student) {
        await schoolContext.Students.AddAsync(student);
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

    public Task UpdateStudent(Student student) {
        throw new NotImplementedException();
    }

    public Task DeleteStudent(Student student) {
        throw new NotImplementedException();
    }

    public Task UpdateTeacher(Teacher teacher) {
        throw new NotImplementedException();
    }

    public Task DeleteTeacher(Teacher teacher) {
        throw new NotImplementedException();
    }
}