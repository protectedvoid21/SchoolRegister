using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;

namespace SchoolRegister.Services.Grades;

public class GradesService : IGradesService {
    private readonly SchoolContext dbContext;
    private readonly IMapper mapper;

    public GradesService(SchoolContext dbContext, IMapper mapper) {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task AddAsync(int subjectId, int studentSubjectId, int gradeType, GradeAdditionalInfo gradeInfo, string comment) {
        Grade grade = new() {
            StudentSubjectId = studentSubjectId,
            SubjectId = subjectId,
            DateAdd = DateTime.Now,
            GradeType = gradeType,
            GradeInfo = gradeInfo,
            Comment = comment,
        };

        await dbContext.AddAsync(grade);
        await dbContext.SaveChangesAsync();
    }

    public async Task<TModel> GetById<TModel>(int id) {
        return await dbContext.Grades
            .Where(g => g.Id == id)
            .ProjectTo<TModel>(mapper.ConfigurationProvider)
            .FirstAsync();
    }

    public async Task<Grade> GetById(int id) {
        return await dbContext.Grades.FindAsync(id);
    }

    public async Task UpdateAsync(int id, int gradeType, GradeAdditionalInfo gradeInfo, string comment) {
        Grade? grade = await dbContext.Grades.FindAsync(id);
        if (grade == null) {
            return;
        }

        grade.GradeType = gradeType;
        grade.GradeInfo = gradeInfo;
        grade.Comment = comment;

        dbContext.Grades.Update(grade);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        Grade? grade = await dbContext.Grades.FindAsync(id);
        if (grade == null) {
            return;
        }

        dbContext.Grades.Remove(grade);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Grade>> GetStudentGrades(int studentSubjectId) {
        IEnumerable<Grade> grades = dbContext.Grades.Where(s => s.StudentSubject.Id == studentSubjectId);
        return grades;
    }

    public async Task<bool> IsOwner(int gradeId, int studentId) {
        Grade grade = await dbContext.Grades
            .Include(g => g.StudentSubject.Student)
            .FirstAsync(g => g.Id == gradeId);
        return grade.StudentSubject.Student.Id == studentId;
    }

    public async Task<bool> IsTeaching(int gradeId, int teacherId) {
        Grade grade = await dbContext.Grades
            .Include(g => g.StudentSubject.SchoolSubject.Teacher)
            .FirstAsync(g => g.Id == gradeId);

        return grade.StudentSubject.SchoolSubject.Teacher.Id == teacherId;
    }
}