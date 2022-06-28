using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels;
using SchoolRegister.Services.Grades;
using SchoolRegister.Services.Students;
using SchoolRegister.Services.Subjects;

namespace SchoolRegister.Controllers;

[Authorize(Roles = "Student")]
public class StudentController : Controller {
    private readonly IStudentsService studentsService;
    private readonly IGradesService gradesService;
    private readonly ISubjectsService subjectsService;
    private readonly UserManager<AppUser> userManager;

    public StudentController(IStudentsService studentsService, IGradesService gradesService, ISubjectsService subjectsService, UserManager<AppUser> userManager) {
        this.studentsService = studentsService;
        this.gradesService = gradesService;
        this.subjectsService = subjectsService;
        this.userManager = userManager;
    }

    public async Task<IActionResult> Index() {
        AppUser user = await userManager.GetUserAsync(User);
        Student student = await studentsService.GetByUser(user);

        IEnumerable<StudentSubject> studentSubjects = await subjectsService.GetStudentSubjectForStudent(student);
        StudentPanelViewModel studentPanelModel = new() {
            Student = student,
            StudentSubjects = studentSubjects
        };
        return View(studentPanelModel);
    }

    [Authorize(Roles = "Student, Teacher")]
    public async Task<IActionResult> GradeView(int gradeId) {
        Grade grade = await gradesService.GetById(gradeId);

        GradeViewModel gradeModel = new() {
            Id = gradeId,
            DateAdd = grade.DateAdd,
            GradeName = grade.GetGradeName(),
            SubjectName = grade.Subject.Name,
            Comment = grade.Comment,
        };

        return View(gradeModel);
    }
}